namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// コマンドを表します。
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// ゲーム開始
        /// </summary>
        Play = 0,

        /// <summary>
        /// テトリミノ右回転
        /// </summary>
        RotateRight,

        /// <summary>
        /// テトリミノ左回転
        /// </summary>
        RotateLeft,

        /// <summary>
        /// テトリミノ右移動
        /// </summary>
        MoveRight,

        /// <summary>
        /// テトリミノ左移動
        /// </summary>
        MoveLeft,

        /// <summary>
        /// テトリミノ下移動
        /// </summary>
        MoveDown,

        /// <summary>
        /// テトリミノ即時確定
        /// </summary>
        ForceFix,
    }
}
