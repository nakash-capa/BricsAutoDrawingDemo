using Teigha.Runtime;
using AutoDrawingDialog;

namespace AutoDrawingPlugin
{
    /// <summary>
    /// カスタムコマンドクラス
    /// </summary>
    public class CustomCommand
    {
        /// <summary>
        /// 自動作図処理を実行するコマンド
        /// </summary>
        [CommandMethod("AutoDraw")]  
        public static void AutoDraw()
        {
            var dialog = new AutoDrawDialog();
            dialog.ShowDialog();
        }
    }
}
