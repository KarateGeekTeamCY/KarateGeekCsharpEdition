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




    }
}
