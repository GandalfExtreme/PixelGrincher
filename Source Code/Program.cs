using System;
using System.Text;
using ZBitmap;

namespace PixelGrinch
{
    class Program
    {
        static PixelGrincher pixel_grincher;

        [STAThread]
        static void Main(string[] args)
        {

            Console.Clear();
            Console.WriteLine("/*--------*/ PIXEL GRINCH /*--------*/");
            Console.WriteLine("<1> DifferentPixels");
            Console.WriteLine("<2> ShiftPixels");
            Console.Write("> ");

            int read = -1; 
            if (!int.TryParse(Console.ReadLine(), out read))
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            if (read == 1) pixel_grincher = new DifferentPixels();
            else if (read == 2) pixel_grincher = new ShiftPixels();

            pixel_grincher.Interface();
            pixel_grincher.Load();
            pixel_grincher.Process_Images();
            pixel_grincher.Save_Images();
            pixel_grincher.Dispose();

            Console.WriteLine("Done.");
        }
    }
}
