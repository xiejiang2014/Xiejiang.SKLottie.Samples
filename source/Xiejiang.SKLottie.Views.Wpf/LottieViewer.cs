#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using CommunityToolkit.WinUI.Lottie;
using CommunityToolkit.WinUI.Lottie.LottieData;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using Xiejiang.SKLottie.Content;
using Xiejiang.SKLottie.Drawer;
using Path = System.IO.Path;

namespace Xiejiang.SKLottie.Views.Wpf
{
    [TemplatePart
        (
            Name = "PART_SkElement",
            Type = typeof(SKElement)
        )
    ]
    public class LottieViewer : Control
    {
        static LottieViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata
                (
                 typeof(LottieViewer),
                 new FrameworkPropertyMetadata
                     (
                      typeof(LottieViewer)
                     )
                );
        }

        private DrawingBuffers? _drawingBuffers;

        private SKElement SkElement;

        public LottieViewer()
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        public override void OnApplyTemplate()
        {
            var partSkElement = GetTemplateChild
                (
                 "PART_SkElement"
                );

            if (partSkElement is SKElement skElement)
            {
                SkElement              =  skElement;
                SkElement.PaintSurface += SkElement_PaintSurface;
            }


            base.OnApplyTemplate();
        }

        #region LottieFile

        [Obsolete
            (
                "Use \"Source\" property to instead."
            )]
        public string LottieFile
        {
            get =>
                (string)GetValue
                    (
                     LottieFileProperty
                    );
            set =>
                SetValue
                    (
                     LottieFileProperty,
                     value
                    );
        }

        public static readonly DependencyProperty LottieFileProperty =
            DependencyProperty.Register
                (
                 "LottieFile",
                 typeof(string),
                 typeof(LottieViewer),
                 new PropertyMetadata
                     (
                      default(string),
                      LottieFilePropertyChanged
                     )
                );

        private static async void LottieFilePropertyChanged
        (
            DependencyObject                   d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is LottieViewer thisLottieViewer)
            {
                if (e.NewValue is not string newValue)
                {
                    thisLottieViewer.SklLottieComposition = null;
                    return;
                }

                thisLottieViewer.LottieFileLoadingFailed = false;

                if (string.IsNullOrWhiteSpace
                        (
                         newValue
                        )
                   )
                {
                    thisLottieViewer.SklLottieComposition = null;
                }
                else if (!File.Exists
                             (
                              newValue
                             ))
                {
                    throw new FileNotFoundException
                        (
                         "Can not open lottie file.",
                         newValue
                        );
                }

                try
                {
                    var lottieComposition = await StorageFileLoader.LoadAsync
                                                (
                                                 newValue
                                                );


                    thisLottieViewer.SklLottieComposition = new SklLottieComposition
                        (
                         lottieComposition
                        );
                }
                catch (Exception)
                {
                    thisLottieViewer.SklLottieComposition    = null;
                    thisLottieViewer.LottieFileLoadingFailed = true;
                }
            }
        }

        #endregion


        public bool LottieFileLoadingFailed { get; private set; }


        #region Source

        public Uri Source
        {
            get =>
                (Uri)GetValue
                    (
                     SourceProperty
                    );
            set =>
                SetValue
                    (
                     SourceProperty,
                     value
                    );
        }

        public static Uri GetSource
        (
            DependencyObject element
        ) =>
            element != null ?
                (Uri)element.GetValue
                    (
                     SourceProperty
                    ) :
                throw new ArgumentNullException
                    (
                     nameof(element)
                    );

        public static void SetSource
        (
            DependencyObject element,
            Uri              value
        )
        {
            if (element == null)
                throw new ArgumentNullException
                    (
                     nameof(element)
                    );
            element.SetValue
                (
                 SourceProperty,
                 (object)value
                );
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register
                (
                 "Source",
                 typeof(Uri),
                 typeof(LottieViewer),
                 new FrameworkPropertyMetadata
                     (
                      default(Uri),
                      FrameworkPropertyMetadataOptions.Inherits,
                      SourcePropertyChanged
                     )
                );


        private static async void SourcePropertyChanged
        (
            DependencyObject                   d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is LottieViewer thisLottieViewer)
            {
                if (e.NewValue is not Uri uri)
                {
                    thisLottieViewer.SklLottieComposition = null;
                    return;
                }


                try
                {
                    thisLottieViewer.LottieFileLoadingFailed = false;

                    LottieComposition? lottieComposition = null;


                    if (uri.IsAbsoluteUri)
                    {
                        if (uri.IsFile)
                        {
                            lottieComposition = await StorageFileLoader.LoadAsync
                                                    (
                                                     uri.AbsolutePath
                                                    );
                        }
                        else if (uri.Scheme.Equals
                                     (
                                      "pack",
                                      StringComparison.OrdinalIgnoreCase
                                     ))
                        {
                            var streamResourceInfo = Application.GetResourceStream
                                (
                                 uri
                                );

                            if (streamResourceInfo is not null)
                            {
                                lottieComposition = await StorageFileLoader.LoadAsync
                                                        (
                                                         streamResourceInfo.Stream
                                                        );
                            }
                        }
                    }
                    else
                    {
                        var file = Path.Combine
                            (
                             AppDomain.CurrentDomain.BaseDirectory,
                             uri.OriginalString.Replace
                                     (
                                      "/",
                                      "\\"
                                     )
                                .TrimStart
                                     (
                                      '\\'
                                     )
                            );

                        if (File.Exists
                                (
                                 file
                                ))
                        {
                            lottieComposition = await StorageFileLoader.LoadAsync
                                                    (
                                                     file
                                                    );
                        }
                        else
                        {
                            var baseUri = BaseUriHelper.GetBaseUri
                                (
                                 d
                                );


                            var newUri = new Uri
                                (
                                 baseUri,
                                 uri
                                );

                            var streamResourceInfo = Application.GetResourceStream
                                (
                                 newUri
                                );

                            if (streamResourceInfo is not null)
                            {
                                lottieComposition = await StorageFileLoader.LoadAsync
                                                        (
                                                         streamResourceInfo.Stream
                                                        );
                            }
                        }
                    }


                    thisLottieViewer.SklLottieComposition = lottieComposition is null ?
                                                                null :
                                                                new SklLottieComposition
                                                                    (
                                                                     lottieComposition
                                                                    );
                }
                catch (Exception exception)
                {
                    thisLottieViewer.SklLottieComposition    = null;
                    thisLottieViewer.LottieFileLoadingFailed = true;
                }
            }
        }

        #endregion


        #region MaxFrame

        public double MaxFrame
        {
            get =>
                (double)GetValue
                    (
                     MaxFrameProperty
                    );
            private set =>
                SetValue
                    (
                     MaxFrameKey,
                     value
                    );
        }

        private static readonly DependencyPropertyKey MaxFrameKey
            = DependencyProperty.RegisterReadOnly
                (
                 "MaxFrame",
                 typeof(double),
                 typeof(LottieViewer),
                 new PropertyMetadata
                     (
                      default(double),
                      MaxFramePropertyChanged
                     )
                );

        private static void MaxFramePropertyChanged
        (
            DependencyObject                   d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is LottieViewer lottieViewer)
            {
            }
        }

        #endregion


        public static readonly DependencyProperty MaxFrameProperty
            = MaxFrameKey.DependencyProperty;


        #region CurrentFrame

        public double CurrentFrame
        {
            get =>
                (double)GetValue
                    (
                     CurrentFrameProperty
                    );
            set =>
                SetValue
                    (
                     CurrentFrameProperty,
                     value
                    );
        }

        public static readonly DependencyProperty CurrentFrameProperty =
            DependencyProperty.Register
                (
                 "CurrentFrame",
                 typeof(double),
                 typeof(LottieViewer),
                 new PropertyMetadata
                     (
                      default(double),
                      CurrentFramePropertyChanged
                     )
                );

        private static void CurrentFramePropertyChanged
        (
            DependencyObject                   d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is LottieViewer thisLottieViewer &&
                e.OldValue is double oldValue      &&
                e.NewValue is double newValue)
            {
                if (thisLottieViewer.SklLottieComposition is not null)
                {
                    thisLottieViewer.SklLottieComposition.CurrentFrame = newValue;
                    thisLottieViewer._needRendering                    = true;
                }
            }
        }

        #endregion

        #region Scale

        public float Scale
        {
            get =>
                (float)GetValue
                    (
                     ScaleProperty
                    );
            set =>
                SetValue
                    (
                     ScaleProperty,
                     value
                    );
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register
                (
                 "Scale",
                 typeof(float),
                 typeof(LottieViewer),
                 new PropertyMetadata
                     (
                      1f,
                      ScalePropertyChanged
                     )
                );

        private static void ScalePropertyChanged
        (
            DependencyObject                   d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is LottieViewer thisLottieViewer &&
                e.OldValue is float oldValue       &&
                e.NewValue is float newValue)
            {
                thisLottieViewer._needRendering = true;
            }
        }

        #endregion


        #region TimeStretch

        public double TimeStretch
        {
            get =>
                (double)GetValue
                    (
                     TimeStretchProperty
                    );
            set =>
                SetValue
                    (
                     TimeStretchProperty,
                     value
                    );
        }

        public static readonly DependencyProperty TimeStretchProperty =
            DependencyProperty.Register
                (
                 "TimeStretch",
                 typeof(double),
                 typeof(LottieViewer),
                 new PropertyMetadata
                     (
                      1d,
                      TimeStretchPropertyChanged
                     )
                );

        private static void TimeStretchPropertyChanged
        (
            DependencyObject                   d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is LottieViewer thisLottieViewer &&
                e.OldValue is double oldValue      &&
                e.NewValue is double newValue)
            {
                thisLottieViewer._needRendering = true;
            }
        }

        #endregion


        #region SklLottieComposition

        private SklLottieComposition? _sklLottieComposition;

        private SklLottieComposition? SklLottieComposition
        {
            get => _sklLottieComposition;
            set
            {
                _sklLottieComposition = value;
                CurrentFrame          = 0d;

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
            }
        }

        #endregion


        #region IsPlaying

        public bool IsPlaying
        {
            get =>
                (bool)GetValue
                    (
                     IsPlayingProperty
                    );
            set =>
                SetValue
                    (
                     IsPlayingProperty,
                     value
                    );
        }

        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register
                (
                 "IsPlaying",
                 typeof(bool),
                 typeof(LottieViewer),
                 new PropertyMetadata
                     (
                      true,
                      IsPlayingPropertyChanged
                     )
                );

        private static void IsPlayingPropertyChanged
        (
            DependencyObject                   d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is LottieViewer thisLottieViewer &&
                e.OldValue is bool oldValue        &&
                e.NewValue is bool newValue)
            {
                if (newValue)
                {
                    thisLottieViewer._stopwatch.Start();
                }
                else
                {
                    thisLottieViewer._stopwatch.Stop();
                }
            }
        }

        #endregion


        #region 绘制

        private bool _needRendering;

        private void CompositionTarget_Rendering
        (
            object?   sender,
            EventArgs e
        )
        {
            if (SklLottieComposition is null)
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
                SkElement?.InvalidateVisual();
            }
        }

        private void SkElement_PaintSurface
        (
            object?                 sender,
            SKPaintSurfaceEventArgs e
        )
        {
            DrawCanvas
                (
                 e.Surface,
                 e.Info.Width,
                 e.Info.Height
                );
        }


        private readonly Stopwatch _stopwatch = new();

        private void DrawCanvas
        (
            SKSurface surface,
            int       width,
            int       height
        )
        {
            if (SklLottieComposition is null || _drawingBuffers is null)
            {
                return;
            }

            try
            {
                LottieDrawer.DrawSklLottieComposition
                    (
                     surface.Canvas,
                     SklLottieComposition,
                     _drawingBuffers,
                     Scale
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine
                    (
                     e
                    );
            }
        }

        #endregion
    }
}