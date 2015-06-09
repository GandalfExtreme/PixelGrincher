using System;
using System.Text;
using System.Threading.Tasks;
using ZBitmap;

namespace PixelGrinch
{
    public class DifferentPixels : PixelGrincher
    {
        public ImageH[] images;
        public int image_count;

        public override void Interface()
        {
            Console.Clear();
            Console.WriteLine("/*--------*/ PIXEL GRINCH /*--------*/");
            Console.WriteLine("           DifferentPixels            ");
            Console.WriteLine("Image-Count(2,4,8)");
            Console.Write("> ");

            image_count = -1;
            if (!int.TryParse(Console.ReadLine(), out image_count))
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            if (image_count % 2 != 0 || image_count < 2 || image_count > 8)
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            images = new ImageH[image_count];
            for (int i = 0; i < image_count; i++)
            {
                images[i] = new ImageH();
                images[i].Load_Image();
            }

            try
            {   
                Load();
                Process_Images();
                Save_Images();
            }
            catch
            {
                Console.WriteLine("Error!");
                Environment.Exit(0);
            }
            images = null;
        }

        // Load the images parallel
        public override void Load()
        {
            for (int i = 0; i < image_count; i++)
            {
                images[i].Load_Parallel();
            }
        }

        // Process the images
        public override void Process_Images()
        {
            for (int i = 0; i < image_count ; i++)
            {
                Parallel.For(0, images[i].p_bmp.rgb.Length, p =>
                {
                    int val = images[i].p_bmp.rgb[p] - (images[i].p_bmp.rgb[p] % image_count) + i;
                    images[i].p_bmp.rgb[p] = (byte)val;
                });
            }
        }

        // Write_Parallel to Bitmap && 
        // Write the images to Disk
        public override void Save_Images()
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].Write_Parallel_To_Bmp();
                images[i].Save(images[i].path.Replace("." + images[i].Get_Image_Type(), "_processed_" + i + ".png"), true);
            }
        }

        public override void Dispose()
        {
            if (images != null)
                for (int i = 0; i < images.Length; i++)
                    images[i].Dispose();

            image_count = -1;
        }
    }
}
