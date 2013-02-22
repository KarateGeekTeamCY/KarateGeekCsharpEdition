using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;


namespace KarateGeek.helpers
{
    class ImageConverter
    {
        public byte[] ToByteTable(string imageSource)
        {
            FileStream fs = new FileStream(imageSource, FileMode.Open, FileAccess.Read);
            BinaryReader binr = new BinaryReader(new BufferedStream(fs));
            byte[] imageinbytes = binr.ReadBytes(Convert.ToInt32(fs.Length));

            return imageinbytes;
        }


        public Image ToImage(byte[] imageBytes)
        {
            Stream productImageStream = new System.IO.MemoryStream(imageBytes);
            return Image.FromStream(productImageStream);
            
        }


        public static System.Windows.Controls.Image ConvertDrawingImageToWPFImage(System.Drawing.Image gdiImg)
        {


            System.Windows.Controls.Image img = new System.Windows.Controls.Image();

            //convert System.Drawing.Image to WPF image
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(gdiImg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            img.Source = WpfBitmap;
            img.Width = 500;
            img.Height = 600;
            img.Stretch = System.Windows.Media.Stretch.Fill;
            return img;
        }


    }
}
