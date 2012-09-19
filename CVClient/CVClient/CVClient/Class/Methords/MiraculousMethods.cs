using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Procesta.CVClient.Class.Exceptions;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using Procesta.CVClient.ServerNotifaction;
namespace Procesta.CVClient.Class.Methords
{
   public class MiraculousMethods
   {
       public static CounterInformation conterInformation = new CounterInformation();


       public byte[] imageToByteArray(string imagePath)
       {
           if (imagePath == null)
           {
               imagePath = "/Images/NoImage.png";
           }
           Bitmap bitMapImage = new Bitmap(imagePath);
           ImageFormat bmpFormate = bitMapImage.RawFormat;
           var imagetoConvert = Image.FromFile(imagePath);
           using (MemoryStream ms = new MemoryStream())
           {
               imagetoConvert.Save(ms, bmpFormate);
               return ms.ToArray();
           }
       }


       /// <summary>
        /// Convert Image path to Image Source.
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
       public ImageSource StringToImageSource(string imagePath)
        {
            try
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                myBitmapImage.DecodePixelWidth = 200;
                myBitmapImage.EndInit();
                return myBitmapImage;
            }
            catch
            {
                throw new ErrorException(CVsVariables.ERROR_MESSAGES[0, 4]);
            }
        }
       /// <summary>
        /// Byte[] to Image Source Converter
        /// </summary>
        /// <param name="bytesOfImage"></param>
        /// <returns></returns>
       public ImageSource BytesToImageSources(byte[] bytesOfImage)
        {
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
       /// <summary>
       /// Get Image From a Image Box and Convert it in Byte[]
       /// </summary>
       /// <param name="imageC"></param>
       /// <returns></returns>
       public byte[] ImageFromControl(BitmapImage imageC)
       {
           MemoryStream memStream = new MemoryStream();
           JpegBitmapEncoder encoder = new JpegBitmapEncoder();
           encoder.Frames.Add(BitmapFrame.Create(imageC));
           encoder.Save(memStream);
           return memStream.GetBuffer();
       }
       /// <summary>
       /// Read Only Property
       /// </summary>
       public static string ClientHostIp
       {
           get { return Properties.Settings.Default.clientIpAddress; }
       }
       /// <summary>
       /// Check a Email Address Is Valid or Not
       /// </summary>
       /// <param name="emailAddress"></param>
       /// <returns></returns>
       public bool emailAddressValidter(string emailAddress)
       {
           Regex emailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
           if (emailRegex.Match(emailAddress).Success)
           {
               return true;
           }
           else
           {
               throw new ErrorException(CVsVariables.ERROR_MESSAGES[1, 4]);
           }
       }
       /// <summary>
       /// Check a Valid Phone NUmber
       /// </summary>
       /// <param name="phoneNumber"></param>
       /// <returns></returns>
       public bool PhoneNUmberValidter(string phoneNumber)
       {
           Regex phoneRegex = new Regex(@"^01[1-9]{1}(([0-9]{8}){1})$");
           if (phoneRegex.Match(phoneNumber).Success)
           {
               return true;
           }
           else
           {
               throw new ErrorException(CVsVariables.ERROR_MESSAGES[1, 5]);
           }
 
       }
       /// <summary>
       /// Check Ip Address Valid Or Not
       /// </summary>
       /// <param name="IpAddress"></param>
       /// <returns></returns>
       public bool ipAddressValidetor(string IpAddress)
       {
           Regex phoneRegex = new Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
           if (phoneRegex.Match(IpAddress).Success)
           {
               return true;
           }
           else
           {
               throw new ErrorException(CVsVariables.ERROR_MESSAGES[1, 6]);
           }
       }
       
    }
}
