using ImageResizerCore.Utils;
using System;
using System.Drawing;

namespace TestsImages
{
    class Program
    {
        static void Main(string[] args)
        {
            Image image1 = Image.FromFile("D:\\1.jpg");
            image1.ResizeImageAndRatio(40, 150).Save("D:\\2.jpg");
            Console.WriteLine("Hello World!");
        }
    }
}
