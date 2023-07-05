using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Xiejiang.SKLottie.Samples.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnDragEnter
        (
            object        sender,
            DragEventArgs e
        )
        {
            e.Effects = e.Data.GetDataPresent
                            (
                             DataFormats.FileDrop
                            ) ?
                            DragDropEffects.Link :
                            DragDropEffects.None;
        }


        private async void MainWindow_OnDrop
        (
            object        sender,
            DragEventArgs e
        )
        {
            if (e.Data.GetData
                    (
                     DataFormats.FileDrop
                    ) is string[] fileList &&
                fileList.Any())
            {
                LottieFile = fileList.First();
            }
        }


        private string _lottieFile = "pack://application:,,,/Assets/lottielogo1.json";

        public string LottieFile
        {
            get => _lottieFile;
            set
            {
                _lottieFile = value;
                OnPropertyChanged();
            }
        }

        private void Play_OnClick
        (
            object          sender,
            RoutedEventArgs e
        )
        {
            LottieViewer.IsPlaying = true;
        }

        private void Stop_OnClick
        (
            object          sender,
            RoutedEventArgs e
        )
        {
            LottieViewer.IsPlaying = false;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged
        (
            [CallerMemberName] string? propertyName = null
        )
        {
            PropertyChanged?.Invoke
                (
                 this,
                 new PropertyChangedEventArgs
                     (
                      propertyName
                     )
                );
        }

        private void LottieViewer_OnPlayComplete(object sender, RoutedEventArgs e)
        {
            Debug.Print("动画播放完成.");
        }
    }
}