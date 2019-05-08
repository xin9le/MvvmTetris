using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Timers;
using MvvmTetris.Linq;
using MvvmTetris.Timers;
using Reactive.Bindings;



namespace MvvmTetris.Engine.Models
{
    /// <summary>
    /// テトリスの場としての機能を提供します。
    /// </summary>
    public class Field
    {
        #region 定数
        /// <summary>
        /// 行数を表します。この値は定数です。
        /// </summary>
        public const byte RowCount = 24;


        /// <summary>
        /// 列数を表します。この値は定数です。
        /// </summary>
        public const byte ColumnCount = 10;
        #endregion


        #region イベント
        /// <summary>
        /// テトリミノが移動されたときに発生します。
        /// </summary>
        public IObservable<Unit> TetriminoMoved => this.tetriminoMoved;
        private readonly Subject<Unit> tetriminoMoved = new Subject<Unit>();


        /// <summary>
        /// テトリミノが回転されたときに発生します。
        /// </summary>
        public IObservable<Unit> TetriminoRotated => this.tetriminoRotated;
        private readonly Subject<Unit> tetriminoRotated = new Subject<Unit>();


        /// <summary>
        /// テトリミノが確定されたときに発生します。
        /// </summary>
        public IObservable<Unit> TetriminoFixed => this.tetriminoFixed;
        private readonly Subject<Unit> tetriminoFixed = new Subject<Unit>();


        /// <summary>
        /// ブロックが消されたときに発生します。
        /// 消された行数が通知されます。
        /// </summary>
        public IObservable<int> BlockRemoved => this.blockRemoved;
        private readonly Subject<int> blockRemoved = new Subject<int>();


        /// <summary>
        /// ブロックがクリアされたときに発生します。
        /// </summary>
        public IObservable<Unit> BlockCleared => this.blockCleared;
        private readonly Subject<Unit> blockCleared = new Subject<Unit>();
        #endregion


        #region プロパティ
        /// <summary>
        /// 配置済みブロックのコレクションを取得します。
        /// </summary>
        public Block[] PlacedBlocks { get; private set; } = Array.Empty<Block>();


        /// <summary>
        /// 現在動かしているテトリミノを取得または設定します。
        /// </summary>
        public ReactivePropertySlim<Tetrimino> Tetrimino { get; } = new ReactivePropertySlim<Tetrimino>();


        /// <summary>
        /// アクティブ状態かどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsActivated => this.isActivated;
        private readonly ReactivePropertySlim<bool> isActivated = new ReactivePropertySlim<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);



        /// <summary>
        /// 上限ラインを超えているかどうかを取得します。
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsUpperLimitOvered => this.isUpperLimitOvered;
        private readonly ReactivePropertySlim<bool> isUpperLimitOvered = new ReactivePropertySlim<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);


        /// <summary>
        /// タイマーを取得します。
        /// </summary>
        private Timer Timer { get; } = new Timer();
        #endregion


        #region コンストラクタ
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public Field()
        {
            var sequence = this.Timer.ElapsedAsObservable();
            var context = System.Threading.SynchronizationContext.Current;
            if (context != null)
                sequence = sequence.ObserveOn(context);
            sequence.Subscribe(x => this.MoveTetrimino(MoveDirection.Down));
        }
        #endregion


        #region 操作
        /// <summary>
        /// 開始します。
        /// </summary>
        /// <param name="kind">最初のテトリミノの種類</param>
        public void Start(TetriminoKind kind)
        {
            this.Reset();
            this.isActivated.Value = true;
            this.Tetrimino.Value = new Tetrimino(kind);
            this.Timer.Start();
        }


        /// <summary>
        /// リセットします。
        /// </summary>
        public void Reset()
        {
            this.Timer.Stop();
            this.Timer.Interval = 1000;
            this.isActivated.Value = false;
            this.isUpperLimitOvered.Value = false;
            this.Tetrimino.Value = null;
            this.PlacedBlocks = Array.Empty<Block>();
            this.blockCleared.OnNext(Unit.Default);
        }


        /// <summary>
        /// 指定された方向にテトリミノを移動します。
        /// </summary>
        /// <param name="direction">移動方向</param>
        public void MoveTetrimino(MoveDirection direction)
        {
            if (!this.isActivated.Value)
                return;

            //--- 下移動は特別処理
            if (direction == MoveDirection.Down)
            {
                this.Timer.Stop();
                if (this.Tetrimino.Value.Move(direction, this.CheckCollision))
                {
                    this.tetriminoMoved.OnNext(Unit.Default);
                }
                else
                {
                    this.FixTetrimino();
                }
                this.Timer.Start();
                return;
            }

            //--- 左右移動の場合は移動に成功したら変更通知
            if (this.Tetrimino.Value.Move(direction, this.CheckCollision))
                this.tetriminoMoved.OnNext(Unit.Default);
        }


        /// <summary>
        /// 指定された方向にテトリミノを回転させます。
        /// </summary>
        /// <param name="direction">回転方向</param>
        public void RotationTetrimino(RotationDirection direction)
        {
            if (!this.isActivated.Value)
                return;

            if (this.Tetrimino.Value.Rotation(direction, this.CheckCollision))
                this.tetriminoRotated.OnNext(Unit.Default);
        }


        /// <summary>
        /// テトリミノを強制的に確定させます。
        /// </summary>
        public void ForceFixTetrimino()
        {
            if (!this.isActivated.Value)
                return;

            this.Timer.Stop();
            while (this.Tetrimino.Value.Move(MoveDirection.Down, this.CheckCollision)) ;  //--- 衝突するまで動かし続ける
            this.tetriminoMoved.OnNext(Unit.Default);
            this.FixTetrimino();
            this.Timer.Start();
        }


        /// <summary>
        /// テトリミノを確定させます。
        /// </summary>
        private void FixTetrimino()
        {
            //--- テトリミノを配置済みとして確定し、ブロックが揃ってたら消す
            var result = this.RemoveAndFixBlock();

            //--- 更新
            this.Tetrimino.Value = null;  //--- 一旦クリア
            this.PlacedBlocks = result.blocks;

            //--- 揃った行数を通知
            if (result.removedRowCount > 0)
                this.blockRemoved.OnNext(result.removedRowCount);

            //--- ブロックが上限を超えていたらゲームオーバー
            if (result.blocks.Any(x => x.Position.Row < 0))
            {
                this.isActivated.Value = false;
                this.isUpperLimitOvered.Value = true;
                return;
            }

            //--- 確定通知
            this.tetriminoFixed.OnNext(Unit.Default);
        }


        /// <summary>
        /// テトリミノが落ちる速度を上げます。
        /// </summary>
        public void SpeedUp()
        {
            const int min = 15;  //--- 最速 15 ミリ秒
            var interval = this.Timer.Interval / 2;  //--- 倍速にしていく
            this.Timer.Interval = Math.Max(interval, min);
        }
        #endregion


        #region 判定 / その他
        /// <summary>
        /// 衝突判定を行います。
        /// </summary>
        /// <param name="block">チェック対象のブロック</param>
        /// <returns>衝突している場合true</returns>
        private bool CheckCollision(Block block)
        {
            //--- 左側の壁にめり込んでいる
            if (block.Position.Column < 0)
                return true;

            //--- 右側の壁にめり込んでいる
            if (ColumnCount <= block.Position.Column)
                return true;

            //--- 床にめり込んでいる
            if (RowCount <= block.Position.Row)
                return true;

            //--- すでに配置済みブロックがある
            return this.PlacedBlocks.Any(x => x.Position == block.Position);
        }


        /// <summary>
        /// ブロックが揃っていたら消し、配置済みブロックを確定します。
        /// </summary>
        /// <returns>確定された配置済みブロック</returns>
        private (int removedRowCount, Block[] blocks) RemoveAndFixBlock()
        {
            //--- 行ごとにブロックをまとめる
            var rows
                = this.PlacedBlocks
                .Concat(this.Tetrimino.Value.Blocks)  //--- 配置済みのブロックとテトリミノを合成
                .GroupBy(x => x.Position.Row)  //--- 行ごとにまとめる
                .Select(x =>
                (
                    row: x.Key,
                    isFilled: ColumnCount <= x.Count(),  //--- 揃っているか
                    blocks: x
                ))
                .ToArray();

            //--- 揃ったブロックを削除して確定
            var blocks
                = rows
                .OrderByDescending(x => x.row)    //--- 深い方から並び替え
                .WithIndex(x => x.isFilled)       //--- 揃っている行が見つかるたびにインクリメント
                .Where(x => !x.Element.isFilled)  //--- 揃っている行は消す
                .SelectMany(x =>
                {
                    //--- ズラす必要がない行はそのまま処理
                    //--- 処理パフォーマンス向上のため特別処理
                    if (x.Index == 0)
                        return x.Element.blocks;

                    //--- 消えた行のぶん下に段をズラす
                    return x.Element.blocks.Select(y =>
                    {
                        var position = new Position(y.Position.Row + x.Index, y.Position.Column);
                        return new Block(y.Color, position);
                    });
                })
                .ToArray();

            //--- 削除した行数
            var removedRowCount = rows.Count(x => x.isFilled);
            return (removedRowCount, blocks);
        }
        #endregion
    }
}
