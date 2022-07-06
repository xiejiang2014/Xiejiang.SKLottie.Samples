using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Xiejiang.SKLottie.Samples.Avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OpenButton_Clicked
        (
            object?         sender,
            RoutedEventArgs e
        )
        {

            var i = 0;

            var result = await new OpenFileDialog()
                               {
                                   Title = "Open a Lottie json file",
                                   Filters = new List<FileDialogFilter>()
                                             {
                                                 new()
                                                 {
                                                     Name = "Lottie json or zip file",
                                                     Extensions = new List<string>()
                                                                  {
                                                                      "json",
                                                                      "zip"
                                                                  }
                                                 }
                                             },
                                   AllowMultiple = false
                               }.ShowAsync
                             (
                              this
                             );

            if (result is not null &&
                result.Any()       &&
                !string.IsNullOrWhiteSpace
                    (
                     result[0]
                    ) &&
                File.Exists
                    (
                     result[0]
                    )
               )
            {
                LottieViewer.LottieFile = result[0];
            }
        }

        private void PlayButton_Clicked
        (
            object?         sender,
            RoutedEventArgs e
        )
        {
            LottieViewer.IsPlaying = true;
        }

        private void StopButton_Clicked
        (
            object?         sender,
            RoutedEventArgs e
        )
        {
            LottieViewer.IsPlaying = false;
        }
    }
}