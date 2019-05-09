# MvvmTetris

MVVM based Tetris application sample using [ReactiveProperty](https://github.com/runceel/ReactiveProperty). You can study following through this sample.

- How to develop a application for multi platforms
- Simple MVVM (Model - View - ViewModel) architecture
- How to use ReactiveProperty
- Programmable data binding


This sample targets following platforms.

- [WPF](https://github.com/dotnet/wpf)
- [Blazor](https://blazor.net)



## Screen shots

| WPF | Blazor |
| --- | --- |
| ![WPF-ScreenShot](https://raw.githubusercontent.com/xin9le/MvvmTetris/master/screenshot/MvvmTetris.Wpf.png) | ![Blazor-ScreenShot](https://raw.githubusercontent.com/xin9le/MvvmTetris/master/screenshot/MvvmTetris.Blazor.png) |




## Features

- Automatic fall down by timer
- Move / Rotation / Fall down
- Fix tetrimino immediately
- Display next tetrimino
- Display deleted rows information
- Speed-up gradually (when tetrimino is deleted)
- [Super rotation](https://ja.wikipedia.org/wiki/%E3%83%86%E3%83%88%E3%83%AA%E3%82%B9)



## How to play

You can play on web version from [here](http://blazor-tetris.azurewebsites.net/).

| Key | Action |
|---|---|
| ↑ | Rotation right |
| ↓ | Fall down |
| ← | Move left |
| → | Move right |
| X | Rotation right |
| Z | Rotation left |
| Space | Fix tetrimino immediately |
| Esc | Restart |



## Explanation document

- [Tetris Algorithm](https://www.slideshare.net/xin9le/tetris-algorithm)



## License

This sample is provided under MIT License.



## Author

Takaaki Suzuki (a.k.a [@xin9le](https://twitter.com/xin9le)) is software developer in Japan who awarded Microsoft MVP for Developer Technologies (C#) since July 2012.
