using System;
using Reactive.Bindings;
using MvvmTetris.Engine.Models;



namespace MvvmTetris.Engine.ViewModels
{
    /// <summary>
    /// ゲーム全体を描画するためのモデルを提供します。
    /// </summary>
    public class GameViewModel
    {
        #region プロパティ
        /// <summary>
        /// ゲーム本体を取得します。
        /// </summary>
        private Game Game { get; } = new Game();


        /// <summary>
        /// ゲームスコアを取得します。
        /// </summary>
        public ScoreViewModel Score { get; }


        /// <summary>
        /// フィールドの描画用モデルを取得します。
        /// </summary>
        public FieldViewModel Field { get; }


        /// <summary>
        /// 次のテトリミノフィールドの描画用モデルを取得します。
        /// </summary>
        public NextFieldViewModel NextField { get; }


        /// <summary>
        /// 入力操作コマンドを取得します。
        /// </summary>
        public ReactiveCommand<Command> InputCommand { get; } = new ReactiveCommand<Command>();


        /// <summary>
        /// プレイ中かどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsPlaying => this.Game.IsPlaying;


        /// <summary>
        /// ゲームオーバー状態になっているかどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsOver => this.Game.IsOver;
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public GameViewModel()
        {
            this.Score = new ScoreViewModel(this.Game.Score);
            this.Field = new FieldViewModel(this.Game.Field);
            this.NextField = new NextFieldViewModel(this.Game.NextTetrimino);
            this.InputCommand.Subscribe(x =>
            {
                switch (x)
                {
                    case Command.Play: this.Game.Play(); break;
                    case Command.RotateRight: this.Game.Field.RotationTetrimino(RotationDirection.Right); break;
                    case Command.RotateLeft: this.Game.Field.RotationTetrimino(RotationDirection.Left); break;
                    case Command.MoveRight: this.Game.Field.MoveTetrimino(MoveDirection.Right); break;
                    case Command.MoveLeft: this.Game.Field.MoveTetrimino(MoveDirection.Left); break;
                    case Command.MoveDown: this.Game.Field.MoveTetrimino(MoveDirection.Down); break;
                    case Command.ForceFix: this.Game.Field.ForceFixTetrimino(); break;
                    default: throw new ArgumentOutOfRangeException(nameof(x));
                }
            });
        }
        #endregion
    }
}
