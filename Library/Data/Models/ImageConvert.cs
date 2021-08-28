using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Library.Data.Models {
    public static class ImageConvert {
        public static byte[] FromBitmapImageToBytes(BitmapImage imageC) {
            var memStream = new MemoryStream();
            var encoder   = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        public static BitmapImage FromBytesToBitmapImage(byte[] array) {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(array);
            image.EndInit();
            return image;
        }
        public static BitmapImage LoadImageFromFile(string filename) {
            using (var fs = File.OpenRead(filename)) {
                var img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                // Downscaling to keep the memory footprint low
                img.DecodePixelWidth = (int)SystemParameters.PrimaryScreenWidth;
                img.StreamSource     = fs;
                img.EndInit();
                return img;
            }
        }
    }
}