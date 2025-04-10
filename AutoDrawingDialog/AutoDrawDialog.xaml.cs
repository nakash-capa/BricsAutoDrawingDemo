using System.Windows;
using System.Windows.Controls;


namespace AutoDrawingDialog
{
    /// <summary>
    /// AutoDrawDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class AutoDrawDialog : Window
    {
        public AutoDrawDialog()
        {
            // XAMLで定義されたUIを読み込む
            InitializeComponent();
            // ウィンドウが表示されたときに呼ばれるイベントを登録
            Loaded += WindowLoaded;
        }

        /// <summary>
        /// ウィンドウ読み込み時に、部屋ごとの設定UI（ドア・窓・開き方向）を動的に生成して配置
        /// </summary>
        /// <param name="sender">イベントの送信元（ウィンドウ自身）</param>
        /// <param name="e"></param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // 部屋1～4までの設定UIを生成
            for (int i = 1; i <= 4; i++)
            {
                // 各部屋のグループ枠を作成（GroupBoxのヘッダーが「部屋1」などになる）
                var group = new GroupBox
                {
                    Header = $"部屋 {i}",
                    Margin = new Thickness(0, 0, 0, 10)
                };

                // 各設定項目（チェックボックス、ラジオボタン）を縦に並べるパネル
                var panel = new StackPanel { Orientation = Orientation.Vertical };

                // ドアの有無を指定するチェックボックス（初期値：あり）
                var doorCheck = new CheckBox { Content = "ドア", Margin = new Thickness(5), IsChecked = true };

                // ドアの開き方向（右開き／左開き）を指定するラジオボタン（水平並び）（初期値：左開き）
                var directionPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
                directionPanel.Children.Add(new TextBlock { Text = "向き: ", VerticalAlignment = VerticalAlignment.Center });
                var rightOpen = new RadioButton
                {
                    Content = "右開き", GroupName = $"Direction{i}", Margin = new Thickness(5, 0, 5, 0), IsChecked = false
                };
                var leftOpen = new RadioButton
                {
                    Content = "左開き", GroupName = $"Direction{i}", Margin = new Thickness(5, 0, 5, 0) , IsChecked = true
                };
                directionPanel.Children.Add(leftOpen);
                directionPanel.Children.Add(rightOpen);

                // 窓の有無を指定するチェックボックス（初期値：あり）
                var windowCheck = new CheckBox { Content = "窓", Margin = new Thickness(5), IsChecked = true };

                // 項目をStackPanelに追加
                panel.Children.Add(doorCheck);
                panel.Children.Add(directionPanel);
                panel.Children.Add(windowCheck);

                // StackPanelをGroupBoxに設定し、右側のUI領域に追加
                group.Content = panel;
                RoomSettingsPanel.Children.Add(group);
            }
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            // ウィンドウを閉じる
            this.Close();
        }

        private void OnClickOK(object sender, RoutedEventArgs e)
        {
            // 後で自動作図ロジックをここに実装
            MessageBox.Show("自動作図を実行します。");
        }

    }
}
