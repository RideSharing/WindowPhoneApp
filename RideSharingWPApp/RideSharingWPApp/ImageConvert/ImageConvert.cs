using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RideSharingWPApp.ImageConvert
{
    class ImageConvert
    {
        public static BitmapImage convertBase64ToImage(string base64string)
        {
            byte[] fileBytes = Convert.FromBase64String(base64string);

            using (MemoryStream ms = new MemoryStream(fileBytes, 0, fileBytes.Length))
            {
                ms.Write(fileBytes, 0, fileBytes.Length);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(ms);
                return bitmapImage;
            }
        }

        public static string convertImageToBase64(Image image)
        {
            byte[] bytearray = null;

            using (MemoryStream ms = new MemoryStream())
            {
                if (image.Source == null)
                {

                }
                else
                {
                    WriteableBitmap wbitmp = new WriteableBitmap((BitmapImage)image.Source);

                    wbitmp.SaveJpeg(ms, 46, 38, 0, 100);
                    bytearray = ms.ToArray();
                }
            }
            string str = Convert.ToBase64String(bytearray);
            return str;
        }
    }
}
