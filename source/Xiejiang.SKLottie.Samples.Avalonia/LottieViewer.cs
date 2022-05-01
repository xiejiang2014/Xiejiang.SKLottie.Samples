using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using Microsoft.Toolkit.Uwp.UI.Lottie.Lottie;
using Xiejiang.SKLottie.Content;
using Xiejiang.SKLottie.Drawer;

namespace Xiejiang.SKLottie.Samples.Avalonia
{
    public class LottieViewer : Control
    {
        public LottieViewer()
        {
            ClipToBounds = true;
        }


        #region SklLottieComposition

        private SklLottieComposition? _sklLottieComposition;

        public SklLottieComposition? SklLottieComposition
        {
            get => _sklLottieComposition;
            private set
            {
                if (!ReferenceEquals
                        (
                         _sklLottieComposition,
                         value
                        ))
                {
                    _sklLottieComposition = value;

                    CurrentFrame = 0d;

                    if (_sklLottieComposition is not null)
                    {
                        _drawingBuffers = new DrawingBuffers
                            (
                             (int)_sklLottieComposition.Width,
                             (int)_sklLottieComposition.Height
                            );

                        MaxFrame = _sklLottieComposition.FrameCount;
                        if (IsPlaying)
                        {
                            _stopwatch.Start();
                        }
                    }
                    else
                    {
                        MaxFrame = 0;
                        _drawingBuffers?.Dispose();
                        _drawingBuffers = null;
                    }


                    _needRendering = true;

                    InvalidateVisual();
                }
            }
        }

        #endregion

        #region LottieFile

        public static readonly DirectProperty<LottieViewer, string> LottieFileProperty =
            AvaloniaProperty.RegisterDirect<LottieViewer, string>
                (
                 nameof(LottieFile),
                 o => o.LottieFile,
                 (
                     o,
                     v
                 ) => o.LottieFile = v
                );


        private string _lottieFile;

        public string LottieFile
        {
            get => _lottieFile;
            set
            {
                _lottieFile = value;

                if (string.IsNullOrWhiteSpace
                        (
                         _lottieFile
                        )
                   )
                {
                    SklLottieComposition = null;
                }
                else if (!File.Exists
                             (
                              _lottieFile
                             ))
                {
                    throw new FileNotFoundException
                        (
                         "Can not open lottie file.",
                         _lottieFile
                        );
                }


                StorageFileLoader.LoadAsync
                                  (
                                   _lottieFile
                                  )
                                 .ContinueWith
                                      (
                                       t =>
                                       {
                                           if (t.IsCompletedSuccessfully)
                                           {
                                               SklLottieComposition = new SklLottieComposition
                                                   (
                                                    t.Result
                                                   );
                                           }
                                           else
                                           {
                                               SklLottieComposition = null;
                                           }
                                       }
                                      );
            }
        }

        #endregion


        public static readonly DirectProperty<LottieViewer, double> MaxFrameProperty =
            AvaloniaProperty.RegisterDirect<LottieViewer, double>
                (
                 nameof(MaxFrame),
                 o => o.MaxFrame
                );

        private double _maxFrame;

        public double MaxFrame
        {
            get => _maxFrame;
            private set
            {
                Dispatcher.UIThread.Post
                    (
                     () =>
                     {
                         SetAndRaise
                             (
                              MaxFrameProperty,
                              ref _maxFrame,
                              value
                             );
                     }
                    );
            }
        }


        #region CurrentFrame

        public static readonly DirectProperty<LottieViewer, double> CurrentFrameProperty =
            AvaloniaProperty.RegisterDirect<LottieViewer, double>
                (
                 nameof(CurrentFrame),
                 o => o.CurrentFrame,
                 (
                     o,
                     v
                 ) => o.CurrentFrame = v
                );


        private double _currentFrame;

        public double CurrentFrame
        {
            get => _currentFrame;
            set
            {
                if (SklLottieComposition is not null)
                {
                    SklLottieComposition.CurrentFrame = value;
                    _needRendering                    = true;
                }

                Dispatcher.UIThread.Post
                    (
                     () =>
                     {
                         SetAndRaise
                             (
                              CurrentFrameProperty,
                              ref _currentFrame,
                              value
                             );
                     }
                    );
            }
        }

        #endregion

        #region Scale

        public static readonly DirectProperty<LottieViewer, float> ScaleProperty =
            AvaloniaProperty.RegisterDirect<LottieViewer, float>
                (
                 nameof(Scale),
                 o => o.Scale,
                 (
                     o,
                     v
                 ) => o.Scale = v
                );


        private float _scale = 1f;

        public float Scale
        {
            get => _scale;
            set
            {

                if (SklLottieComposition is not null)
                {
                    SklLottieComposition.CurrentFrame = value;
                    _needRendering                    = true;
                }
                
                Dispatcher.UIThread.Post
                    (
                     () =>
                     {
                         SetAndRaise
                             (
                              ScaleProperty,
                              ref _scale,
                              value
                             );
                     }
                    );
            }
        }

        #endregion

        #region TimeStretch

        public static readonly DirectProperty<LottieViewer, double> TimeStretchProperty =
            AvaloniaProperty.RegisterDirect<LottieViewer, double>
                (
                 nameof(TimeStretch),
                 o => o.TimeStretch,
                 (
                     o,
                     v
                 ) => o.TimeStretch = v
                );


        private double _timeStretch = 1d;

        public double TimeStretch
        {
            get => _timeStretch;
            set
            {
                _needRendering = true;
                SetAndRaise
                    (
                     TimeStretchProperty,
                     ref _timeStretch,
                     value
                    );
            }
        }

        #endregion

        #region IsPlaying

        private bool _isPlaying;

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;

                if (_isPlaying)
                {
                    _stopwatch.Start();
                }
                else
                {
                    _stopwatch.Stop();
                }
            }
        }

        #endregion

        #region Rendering

        private bool _needRendering;

        private readonly Stopwatch _stopwatch = new();

        private DrawingBuffers? _drawingBuffers;

        public override void Render
        (
            DrawingContext context
        )
        {
            if (SklLottieComposition is null || _drawingBuffers is null)
            {
                return;
            }


            //按照真实时间
            if (_stopwatch.IsRunning)
            {
                if (_stopwatch.Elapsed * TimeStretch >= TimeSpan.FromSeconds
                        (
                         SklLottieComposition.FrameCount / SklLottieComposition.FramesPerSecond
                        ))
                {
                    _stopwatch.Restart();
                }

                SklLottieComposition.Time = _stopwatch.Elapsed * TimeStretch;
                CurrentFrame              = SklLottieComposition.CurrentFrame;
            }

            if (_needRendering)
            {
                _needRendering = false;
            }


            var noSkia = new FormattedText();
            noSkia.Text     = "Current rendering API is not Skia";
            noSkia.FontSize = 12;

            context.Custom
                (
                 new LottieDrawOperation
                     (
                      new Rect
                          (
                           0,
                           0,
                           Bounds.Width,
                           Bounds.Height
                          ),
                      noSkia,
                      _drawingBuffers,
                      SklLottieComposition,
                      Scale
                     )
                );

            Dispatcher.UIThread.InvokeAsync
                (
                 InvalidateVisual,
                 DispatcherPriority.Background
                );
        }

        #endregion
    }


    public class LottieDrawOperation : ICustomDrawOperation
    {
        private readonly FormattedText _noSkia;

        private          DrawingBuffers       _drawingBuffers;
        private          SklLottieComposition _sklLottieComposition;
        private readonly float                _scale;

        public LottieDrawOperation
        (
            Rect                 bounds,
            FormattedText        noSkia,
            DrawingBuffers       drawingBuffers,
            SklLottieComposition sklLottieComposition,
            float                scale
        )
        {
            _noSkia               = noSkia;
            Bounds                = bounds;
            _drawingBuffers       = drawingBuffers;
            _sklLottieComposition = sklLottieComposition;
            _scale                = scale;
        }

        public void Dispose()
        {
            // No-op
        }

        public Rect Bounds { get; }

        public bool HitTest
        (
            Point p
        ) =>
            false;

        public bool Equals
        (
            ICustomDrawOperation other
        ) =>
            false;

        static Stopwatch St = Stopwatch.StartNew();

        public void Render
        (
            IDrawingContextImpl context
        )
        {
            var canvas = (context as ISkiaDrawingContextImpl)?.SkCanvas;
            if (canvas == null)
            {
                using (var drawingContext = new DrawingContext
                           (
                            context,
                            false
                           ))
                {
                    drawingContext.DrawText
                        (
                         new ImmutableSolidColorBrush
                             (
                              Colors.AliceBlue
                             ),
                         new Point(),
                         _noSkia
                        );
                }
            }
            else
            {
                canvas.Save();

                try
                {
                    LottieDrawer.DrawSklLottieComposition
                        (
                         canvas,
                         _sklLottieComposition,
                         _drawingBuffers,
                         _scale,
                         false
                        );
                }
                catch (Exception e)
                {
                    Console.WriteLine
                        (
                         e
                        );
                }


                canvas.Restore();
            }
        }
    }
}