using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
namespace Procesta.CvServer
{
    [ValueConversion(typeof(byte[]),typeof(BitmapImage))]
    public class ByteImageConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           byte[] binary = value as byte[];
            if (binary != null)
            {
                using (var stream = new MemoryStream(binary.ToArray()))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            return null; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
