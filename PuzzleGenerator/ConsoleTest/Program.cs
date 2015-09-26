using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleGenerator;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var filePath = args[0];
                var x = int.Parse(args[1]);
                var y = int.Parse(args[2]);

                var generator = new PuzzleCreator(filePath);
                var images = generator.GetMixedPartsOfImage(x, y);
                Bitmap bitmap = generator.GenerateBitmap(images, x, y);
                bitmap.Save(string.Format(@"puzzle_{0}", filePath));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
