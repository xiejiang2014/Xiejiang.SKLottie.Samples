Demonstrates how to play Lottie animations in WPF using Xiejiang.SKLottie.

Nuget Packages:
<ol>
  <li>LottieLoader:</li>
    All the code is from https://github.com/CommunityToolkit/Lottie-Windows, I removed the UWP-related part, only kept the part that reads Lottie Json as LottieComposition object. Can run on .net core 3.1+
  
  <li>Xiejiang.SKLottie:</li>
    Draw the content in the LottieComposition with SkiaSharp.
  
  <li>Xiejiang.SKLottie.Views.Wpf.</li>
    Use SkiaSharp.Views.Wpf to present Lottie content in WPF.

</ol>
In theory, it can support any environment where .net core3.1+ and SkiaSharp can run. But currently I only make a sample of WPF.

Currently in preview, there are many features of Lottie that are not yet supported. I have selected about 200 animations at https://lottiefiles.com/, of which about 150 are supported.
There are also some known performance issues, but they are not currently prioritized.


Thanks for the great project CommunityToolkit/Lottie-Windows.
