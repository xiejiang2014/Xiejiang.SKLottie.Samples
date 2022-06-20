Demonstrates how to play Lottie animations in WPF or Avalonia using Xiejiang.SKLottie.

Nuget Packages:
<ol>
  <li>LottieLoader:</li>
    All the code is from https://github.com/CommunityToolkit/Lottie-Windows, I removed the UWP-related part, only kept the part that reads Lottie Json as LottieComposition object. Can run on .net core 3.1+
  
  <li>Xiejiang.SKLottie:</li>
    Draw the content in the LottieComposition with SkiaSharp.
  
  <li>Xiejiang.SKLottie.Views.Wpf.</li>
    Use SkiaSharp.Views.Wpf to present Lottie content in WPF.

</ol>
In theory, it can support any environment where .net core3.1+ and SkiaSharp can run. But currently I've only made samples for WPF and Avalonia.

Currently in preview, there are many features of Lottie that are not yet supported. I have selected about 255 animations at https://lottiefiles.com/, of which about 193  can be played correctly.
There are also some known performance issues, but they are not currently prioritized.


Thanks for the great project CommunityToolkit/Lottie-Windows.


=====================

[more screenshots](https://www.cnblogs.com/8u7tgyjire7890/p/15881159.html)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/lottie%20%E6%B5%8B%E8%AF%951.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/lottie%20%E6%B5%8B%E8%AF%952.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/lottie%20%E6%B5%8B%E8%AF%953.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/lottie%20%E6%B5%8B%E8%AF%954.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/lottie%20%E6%B5%8B%E8%AF%955.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/lottie%20%E6%B5%8B%E8%AF%956.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/lottie%20%E6%B5%8B%E8%AF%957.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB8.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB10.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB12.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB13.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB15.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB16.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB17.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB18.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB19.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB20.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB21.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB22.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB23.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB24.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB25.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB26.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB28.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB30.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB32.gif)

![image](https://github.com/xiejiang2014/Xiejiang.SKLottie.Samples/blob/main/Gallery/%E5%8A%A8%E7%94%BB33.gif)

