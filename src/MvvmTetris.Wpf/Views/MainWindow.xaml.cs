using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MvvmTetris.Engine.Models;
using MvvmTetris.Engine.ViewModels;
using MvvmTetris.Linq;
using MvvmTetris.Wpf.Converters;



namespace MvvmTetris.Wpf.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        #region プロパティ
        /// <summary>
        /// ゲームを取得または設定します。
        /// </summary>
        private GameViewModel Game
        {
            get { return this.DataContext as GameViewModel; }
            set { this.DataContext = value; }
        }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public MainWindow()
        {
            this.Game = new GameViewModel();
            this.InitializeComponent();
            SetupField(this.field, this.Game.Field.Cells, 30);
            SetupField(this.nextField, this.Game.NextField.Cells, 18);
            this.SetupKeyEvents();
            this.Game.CommandManager.Execute(Command.Play);
        }
        #endregion


        #region 初期化
        /// <summary>
        /// フィールド準備を行います。
        /// </summary>
        /// <param name="field">フィールドとなる Grid</param>
        /// <param name="cells">セルの描画用モデル</param>
        /// <param name="blockSize">ブロックサイズ</param>
        private static void SetupField(Grid field, CellViewModel[,] cells, byte blockSize)
        {
            //--- 行/列の定義
            for (int r = 0; r < cells.GetLength(0); r++)
                field.RowDefinitions.Add(new RowDefinition { Height = new GridLength(blockSize, GridUnitType.Pixel) });

            for (int c = 0; c < cells.GetLength(1); c++)
                field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(blockSize, GridUnitType.Pixel) });

            //--- セル設定
            var converter = new DrawingToMediaColorConverter();
            foreach (var item in cells.WithIndex())
            {
                //--- 背景色をバインド
                var brush = new SolidColorBrush();
                var control = new TextBlock
                {
                    DataContext = item.Element,
                    Background = brush,
                    Margin = new Thickness(1),
                };
                var binding = new Binding("Color.Value") { Converter = converter };
                BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, binding);

                //--- 位置を決めて子要素として追加
                Grid.SetRow(control, item.X);
                Grid.SetColumn(control, item.Y);
                field.Children.Add(control);
            }
        }


        /// <summary>
        /// イベントの関連付けを行います。
        /// </summary>
        private void SetupKeyEvents()
        {
            this.KeyDown += (s, e) =>
            {
                var manager = this.Game.CommandManager;
                switch (e.Key)
                {
                    case Key.Z: manager.Execute(Command.RotateLeft); break;
                    case Key.X: manager.Execute(Command.RotateRight); break;
                    case Key.Up: manager.Execute(Command.RotateRight); break;
                    case Key.Right: manager.Execute(Command.MoveRight); break;
                    case Key.Down: manager.Execute(Command.MoveDown); break;
                    case Key.Left: manager.Execute(Command.MoveLeft); break;
                    case Key.Escape: manager.Execute(Command.Play); break;
                    case Key.Space: manager.Execute(Command.ForceFix); break;
                }
            };
        }
        #endregion
    }
}
