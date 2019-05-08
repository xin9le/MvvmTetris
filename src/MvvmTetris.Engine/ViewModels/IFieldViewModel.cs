using System;
using System.Reactive;



namespace MvvmTetris.Engine.ViewModels
{
    /// <summary>
    /// フィールド描画用のモデルとしての基本機能を提供します。
    /// </summary>
    public interface IFieldViewModel
    {
        /// <summary>
        /// 変更されたときに呼び出されます。
        /// </summary>
        IObservable<Unit> Changed { get; }


        /// <summary>
        /// セルのコレクションを取得します。
        /// </summary>
        CellViewModel[,] Cells { get; }
    }
}
