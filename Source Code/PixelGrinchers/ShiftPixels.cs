using System;
using System.Linq;
using System.Text;
using ZBitmap;
using System.Threading.Tasks;
using System.Drawing;

namespace PixelGrinch
{
    public class ShiftPixels : PixelGrincher
    {
        public ImageH base_image;
        public int shifting_count;
        public bool random = false;
        public int shift_offset = 1;
        private Random rand = new Random();

        public override void Interface()
        {
            Console.Clear();
            Console.WriteLine("/*--------*/ PIXEL GRINCH /*--------*/");
            Console.WriteLine("/*           ShiftPixels            */");

            #region Shifting-Count

            Console.WriteLine("Shifting-Count (1-256)");
            Console.Write("> ");

            shifting_count = -1;
            if (!int.TryParse(Console.ReadLine(), out shifting_count))
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            if (shifting_count < 1 || shifting_count > 256)
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            #endregion Shifting-Count


            #region Shift-Offset

            Console.WriteLine("Shift-Offset (1-256)");
            Console.Write("> ");

            shift_offset = -1;
            if (!int.TryParse(Console.ReadLine(), out shift_offset))
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            if (shift_offset < 1 || shift_offset > 256)
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            #endregion Shift-Offset


            #region Random

            Console.WriteLine("Random (true/false)");
            Console.Write("> ");

            string read = Console.ReadLine().ToLower();
            if (read == "true") random = true;
            else if (read == "false") random = false;
            else
            {
                Console.WriteLine("Wrong Input!");
                return;
            }

            #endregion Random

        }

        // Load the images parallel
        public override void Load()
        {
            base_image = new ImageH();
            base_image.Load_Image();
            base_image.Load_Parallel();
        }

        // Process the images
        public override void Process_Images()
        {
            int[] colors = new int[256 * 3];

            Parallel.For(0, 256, i =>
            {
                colors[i * 3] = i;
                colors[i * 3 + 1] = i;
                colors[i * 3 + 2] = i;
            });

            if (random)
                colors = Randomize_Color_Array(colors);

            for (int i = 0; i < shifting_count; i++)
            {
                Console.WriteLine("Processing_" + i + " ...");
                ImageH image = base_image.Clone();
                Parallel.For(0, image.p_bmp.rgb.Length / 3, p =>
                {
                    int offset = (i * shift_offset) % 256;
                    int r = (image.p_bmp.rgb[p * 3] + offset) % 256;
                    int g = (image.p_bmp.rgb[p * 3 + 1] + offset) % 256;
                    int b = (image.p_bmp.rgb[p * 3 + 2] + offset) % 256;


                    image.p_bmp.rgb[p * 3] = (byte)colors[r*3];
                    image.p_bmp.rgb[p * 3 + 1] = (byte)colors[g*3+1];
                    image.p_bmp.rgb[p * 3 + 2] = (byte)colors[b*3+2];
                });

                image.Write_Parallel_To_Bmp();
                image.Save(base_image.path.Replace("." + image.Get_Image_Type(), "_processed_" + i + ".png"), true);
                image.Dispose();
            }
        }

        private int[] Randomize_Color_Array(int[] colors)
        {
            colors = colors.OrderBy(c => rand.Next()).ToArray();

            return colors;
        }

        // Write_Parallel to Bitmap && 
        // Write the images to Disk
        public override void Save_Images()
        {
            // Not used in this particular instance
        }

        public override void Dispose()
        {
            if (base_image != null)
                base_image.Dispose();

            shift_offset = -1;
            shifting_count = -1;
            rand = null;
            random = false;
        }

    }
}
