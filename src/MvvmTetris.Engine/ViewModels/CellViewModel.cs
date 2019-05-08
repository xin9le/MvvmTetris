using System.Drawing;
using Reactive.Bindings;



namespace MvvmTetris.Engine.ViewModels
{
    /// <summary>
    /// セル描画用のモデルを提供します。
    /// </summary>
    public class CellViewModel
    {
        #region プロパティ
        /// <summary>
        /// 色を取得します。
        /// </summary>
        public ReactivePropertySlim<Color> Color { get; } = new ReactivePropertySlim<Color>();
        #endregion
    }
}
