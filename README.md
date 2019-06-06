# Image Resizer Core

A handy and simple image resizer for your project on asp.net core 2.+.

# What is it?

* An IIS/ASP.NET HttpModule & image server. On-demand image manipulation, delivery, and optimization &mdash; with low latency &mdash; makes responsive images easy
* An image processing library optimized and secured for server-side use

Image Resizer Core has a very simple (and powerful) URL API.

![Fit modes](https://pp.userapi.com/c850236/v850236013/165568/fo_3-yxaZLE.jpg)

# Getting Started

1. Create foldedr in wwwroot -> cache
2. Load nuget and install System.Drawing.Common
3. Build Project and add lib to project ImageResizerCore.dll
4. Add in startup.cs record
            app.UseImageResizerMiddleware();
