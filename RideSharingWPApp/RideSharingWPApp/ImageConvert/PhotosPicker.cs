using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RideSharingWPApp.ImageConvert
{
    class PhotosPicker
    {
        public static string photoPick;

        public static void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {


                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);

                //MessageBox.Show(bmp.ToString());
                Image myImgage = new Image();
                myImgage.Source = bmp;
                photoPick = ImageConvert.convertImageToBase64(myImgage);

                //myImg.Source = ImageConvert.convertBase64ToImage(str);
                //MessageBox.Show(str);
            }
        }
    }
}
