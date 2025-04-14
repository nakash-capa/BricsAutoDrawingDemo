
namespace AutoDrawingShared.Models
{
    public class RoomSetting
    {
        /// <summary>部屋のインデックス</summary>
        public int Index { get; set; }
        /// <summary>ドアの有無</summary>
        public bool HasDoor { get; set; }
        /// <summary>窓の有無</summary>
        public bool HasWindow { get; set; }
        /// <summary>"左開き" or "右開き"</summary>
        public string DoorDirection { get; set; }
    }
}
