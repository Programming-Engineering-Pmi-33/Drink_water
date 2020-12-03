namespace DrinkWater.SettingServices
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Media.Imaging;

    public class ImageHandler
    {
        private static byte[] ImageArray { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageHandler"/> class.
        /// </summary>
        public ImageHandler()
        {
        }

        /// <summary>
        /// This function is getter for <c>ImageArray</c> property.
        /// </summary>
        /// <returns>image represented by bite array.</returns>
        public byte[] GetImage()
        {
            return ImageArray;
        }

        /// <summary>
        /// This method convert byte array image to BitmapImage.
        /// </summary>
        /// <param name="imageData">image represented as byte array.</param>
        /// <returns>Image as optimized object that can be shown in XAML.</returns>
        public BitmapImage GetImagefromDB(byte[] imageData)
        {
            if (imageData == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream(imageData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }

        /// <summary>
        /// This function get image from path and convert it to BitmapImage class object.
        /// </summary>
        /// <param name="bitmap">Image get from choosen path.</param>
        /// <returns>Image as optimized object that can be shown in XAML.</returns>
        public BitmapImage ConvertBitmap(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ImageArray = ms.ToArray();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}
