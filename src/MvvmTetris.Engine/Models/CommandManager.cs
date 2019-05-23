using System;



namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// コマンド管理機構を提供します。
    /// </summary>
    public sealed class CommandManager
    {
        #region プロパティ
        /// <summary>
        /// ゲームを取得します。
        /// </summary>
        private Game Game { get; }
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game"></param>
        internal CommandManager(Game game)
            => this.Game = game ?? throw new ArgumentNullException(nameof(game));
        #endregion


        #region メソッド
        /// <summary>
        /// 指定されたコマンドを実行します。
        /// </summary>
        /// <param name="command">コマンド</param>
        public void Execute(Command command)
        {
            switch (command)
            {
                case Command.Play: this.Game.Play(); break;
                case Command.RotateRight: this.Game.Field.RotationTetrimino(RotationDirection.Right); break;
                case Command.RotateLeft: this.Game.Field.RotationTetrimino(RotationDirection.Left); break;
                case Command.MoveRight: this.Game.Field.MoveTetrimino(MoveDirection.Right); break;
                case Command.MoveLeft: this.Game.Field.MoveTetrimino(MoveDirection.Left); break;
                case Command.MoveDown: this.Game.Field.MoveTetrimino(MoveDirection.Down); break;
                case Command.ForceFix: this.Game.Field.ForceFixTetrimino(); break;
                default: throw new ArgumentOutOfRangeException(nameof(command));
            }
        }
        #endregion
    }
}
