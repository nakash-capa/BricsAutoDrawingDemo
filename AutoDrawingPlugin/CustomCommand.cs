using Teigha.Runtime;
using System.Windows.Forms;

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
            MessageBox.Show("自動作図コマンドが呼び出されました！", "AutoDraw");
        }
    }
}
