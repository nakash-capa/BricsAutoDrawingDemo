using System;
using System.Collections.Generic;
using AutoDrawingShared.Models;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Bricscad.ApplicationServices;
using Bricscad.EditorInput;

namespace AutoDrawingShared.Services
{
    public class RoomDrawingService
    {
        // 各部屋の高さ（部屋4→600、他150）
        private readonly Dictionary<int, double> roomHeights = new Dictionary<int, double>
        {
            { 1, 150.0 },
            { 2, 150.0 },
            { 3, 150.0 },
            { 4, 600.0 }
        };

        // ドアのオフセット（その部屋の原点からのY距離）
        private readonly Dictionary<int, double> doorOffsetY = new Dictionary<int, double>
        {
            { 1, 506.93448 },
            { 2, 369.52370 },
            { 3, 236.33353 },
            { 4, 46.46835 }
        };

        // 窓のオフセット（部屋の原点からのX,Y座標）
        private readonly Dictionary<int, List<Point2d>> windowOffsets = new Dictionary<int, List<Point2d>>
        {
            { 1, new List<Point2d> { new Point2d(156, 576.78245) } },
            { 2, new List<Point2d> { new Point2d(156, 439.33626) } },
            { 3, new List<Point2d> { new Point2d(156, 302.71352) } },
            { 4, new List<Point2d>
                {
                    new Point2d(74.81005, 0),     // 横左
                    new Point2d(288.27935, 0),    // 横右
                    new Point2d(0, 262.93625),    // 縦下
                    new Point2d(0, 412.93625),    // 縦中
                    new Point2d(0, 556.93625)     // 縦上
                }
            }
        };

        public void DrawRoomPlan(List<RoomSetting> roomSettings)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            var ppr = ed.GetPoint("\n配置の基準点を指定してください: ");
            if (ppr.Status != PromptStatus.OK) return;

            Point3d basePoint = ppr.Value;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                var btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                // WALL は1つだけ配置
                InsertBlock(btr, "WALL", basePoint, tr);

                double currentYOffset = 0;

                foreach (var room in roomSettings)
                {
                    int index = room.Index;
                    double height = roomHeights.ContainsKey(index) ? roomHeights[index] : 150.0;

                    Point3d roomOrigin = new Point3d(basePoint.X, basePoint.Y + currentYOffset, 0);

                    // ドア（部屋1～3はX方向+156、Y方向-20調整）
                    if (room.HasDoor && doorOffsetY.TryGetValue(index, out double doorY))
                    {
                        double doorX = basePoint.X;
                        double adjustY = 0;

                        if (index == 4)
                        {
                            doorX -= 18.0;
                        }
                        else if(index <= 3)
                        {
                            doorX += 132.0;
                            //adjustY = -23.0; // 少し下にずらす（調整値）
                        }

                        if (index == 1)
                        {
                            adjustY = -32.0; 
                        }
                        else if (index == 2)
                        {
                            adjustY = -36.0;
                        }
                        else if (index == 3)
                        {
                            adjustY = -44.0;
                        }

                        // 🚪向きのブロック名 → 反転する
                        string doorBlock = room.DoorDirection == "左開き" ? "DOOR_LEFT" : "DOOR_RIGHT";

                        var doorPos = new Point3d(doorX, basePoint.Y + doorY + adjustY, 0);
                        InsertBlock(btr, doorBlock, doorPos, tr);
                    }

                    // 窓
                    if (room.HasWindow && windowOffsets.TryGetValue(index, out List<Point2d> offsets))
                    {
                        foreach (var offset in offsets)
                        {
                            double offsetX = offset.X;
                            double offsetY = offset.Y;

                            double adjustX = 0;
                            double adjustY = 0;
                            double rotation = 0;

                            if (index == 4)
                            {
                                adjustX = 6.0;  // 窓位置6右へ
                                if (offset.X == 0)  // 縦窓だけY位置補正
                                {
                                    adjustY = -70.0;
                                }

                                // 部屋4の窓は向きそのまま
                                rotation = (offset.X == 0) ? Math.PI / 2 : 0;
                            }
                            else if (index <= 3)
                            {
                                // X座標：元から156 → そこから左に162 = -6だけ動かすことになるが、
                                // 今回は絶対位置162で良いなら adjustX = -6 でOK（または直接 X = -6）
                                adjustX = 0.0;

                                if (index == 1)
                                {
                                    adjustY = -77.0;   
                                }
                                else if (index == 2)
                                {
                                    adjustY = -78.0;
                                }
                                else if (index == 3)
                                {
                                    adjustY = -80.0;
                                }

                                rotation = Math.PI / 2; // 全て縦窓へ
                            }

                           

                            var winPos = new Point3d(basePoint.X + offsetX + adjustX, basePoint.Y + offsetY + adjustY, 0);

                            InsertBlock(btr, "WINDOW", winPos, tr, rotation);
                        }
                    }


                    currentYOffset += height;
                }

                tr.Commit();
            }
        }

        private void InsertBlock(BlockTableRecord btr, string blockName, Point3d position, Transaction tr, double rotation = 0)
        {
            var db = btr.Database;
            var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

            if (!bt.Has(blockName))
            {
                Application.ShowAlertDialog($"ブロック「{blockName}」が見つかりません。");
                return;
            }

            var blockDef = (BlockTableRecord)tr.GetObject(bt[blockName], OpenMode.ForRead);
            using (var br = new BlockReference(position, blockDef.ObjectId))
            {
                br.Rotation = rotation;
                btr.AppendEntity(br);
                tr.AddNewlyCreatedDBObject(br, true);
            }
        }
    }
}
