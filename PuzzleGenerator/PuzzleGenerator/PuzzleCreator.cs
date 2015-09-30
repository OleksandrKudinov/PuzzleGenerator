using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace PuzzleGenerator
{
    public class PuzzleCreator
    {
        public PuzzleCreator(Bitmap image)
        {
            _image = image;
        }

        public PuzzleCreator(string imagePath)
        {
            _image = new Bitmap(Image.FromFile(imagePath));
        }

        public PuzzleCreator(Stream imageStream)
        {
            _image = new Bitmap(Bitmap.FromStream(imageStream));
        }

        public IEnumerable<Bitmap> GetPartsOfImage(int xPartsCount, int yPartsCount)
        {
            Bitmap sourceImage = _image;

            var partsOfImage = new List<Bitmap>(xPartsCount * yPartsCount);

            var xSize = sourceImage.Width / xPartsCount;
            var ySize = sourceImage.Height / yPartsCount;
            var blockSize = new Size(xSize, ySize);

            for (var yPart = 0; yPart < yPartsCount; yPart++)
            {
                for (var xPart = 0; xPart < xPartsCount; xPart++)    
                {
                    var srcPoint = new Point(xPart * xSize, yPart * ySize);
                    var r = new Rectangle(srcPoint, blockSize);
                    var image = sourceImage.Clone(r, sourceImage.PixelFormat);
                    partsOfImage.Add(image);
                }
            }

            return partsOfImage;
        }

        public IEnumerable<Bitmap> GetMixedPartsOfImage(int xPartsCount, int yPartsCount)
        {
            var images = GetPartsOfImage(xPartsCount, yPartsCount);
            var mixedImages = MixImages(images);
            return mixedImages;
        }

        public IEnumerable<Bitmap> MixImages(IEnumerable<Bitmap> images)
        {
            var rnd = new Random();
            var mixedImages = images.OrderBy(x => rnd.Next());
            return mixedImages;
        }

        public Bitmap GenerateBitmap(IEnumerable<Bitmap> images, int xParts, int yParts)
        {
            var exampleImage = images.FirstOrDefault(x => x != null);

            if (exampleImage == null)
            {
                return null;
            }

            var xSize = exampleImage.Width;
            var ySize = exampleImage.Height;

            var bitmaps = new Queue<Bitmap>(images);

            var generatedBitmap = new Bitmap(xSize * xParts, ySize * yParts);
            var imageBlockSize = new Size(xSize, ySize);
            var imageBlock = new Rectangle(new Point(0, 0), imageBlockSize);

            using (Graphics generatedBitmapGraphics = Graphics.FromImage(generatedBitmap))
            {
                for (var yPart = 0; yPart < yParts; yPart++)
                {
                    for (var xPart = 0; xPart < xParts; xPart++)
                    {
                        var image = bitmaps.Dequeue();
                        var generatedBitmapPoint = new Point(xPart * xSize, yPart * ySize);
                        var generatedBitmapRegion = new Rectangle(generatedBitmapPoint, imageBlockSize);
                        generatedBitmapGraphics.DrawImage(image, generatedBitmapRegion, imageBlock, GraphicsUnit.Pixel);
                    }
                }
            }

            return generatedBitmap;
        }

        private readonly Bitmap _image;
    }
}
