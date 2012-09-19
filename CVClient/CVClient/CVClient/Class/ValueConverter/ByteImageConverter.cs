using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
namespace Procesta.CVClient.Class.ValueConverter
{
    public class ByteImageConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                byte[] bytesOfImage = (byte[])value;
                MemoryStream ms = new MemoryStream();
                ms.Write(bytesOfImage, 0, bytesOfImage.Length);
                ms.Position = 0;
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = ms;
                myBitmapImage.DecodePixelWidth = 200;
                myBitmapImage.EndInit();
                myBitmapImage.Freeze();
                return myBitmapImage;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
