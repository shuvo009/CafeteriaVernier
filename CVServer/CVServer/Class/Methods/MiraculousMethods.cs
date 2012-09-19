using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesta.CvServer.Class.Methods;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.ObjectModel;
using Procesta.CvServer.EntityFramework;
namespace Procesta.CvServer.Class.Methods
{
    public class MiraculousMethods
    {

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
        /// Load Munities and Amount
        /// </summary>
        public ObservableCollection<ModelBillConfig> LoadBillConfig()
        {
            try
            {
                string billXmlPath = Path.Combine(GetTempFolder(CvVariables.SOFTWARE_NAME), CvVariables.MUNITIES_FILE);
                if (File.Exists(billXmlPath))
                {
                    XElement billConfigXml = XElement.Load(billXmlPath);
                    return new ObservableCollection<ModelBillConfig>(from bilinginfo in billConfigXml.Elements("Rate") 
                                                                     select new ModelBillConfig 
                                                                                            {
                                                                                                Minutes = bilinginfo.Element("Minutes").Value, 
                                                                                                Amount = bilinginfo.Element("Bill").Value
                                                                                            });
                }
                else
                {
                     return new ObservableCollection<ModelBillConfig>();
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Loading Bill Configuration File", ErrorException);
                return default(ObservableCollection<ModelBillConfig>);
            }
        }
        /// <summary>
        /// Check Cash Date
        /// </summary>
        public void CheckCashDate()
        {
            try
            {
                using (Cafeteria_Vernier_dbEntities cvDatabase = new Cafeteria_Vernier_dbEntities())
                {
                    if (cvDatabase.Cashes.FirstOrDefault(x=>x.EntryDate == DateTime.Today)==null)
                    {
                        cvDatabase.AddToCashes(new Cash 
                                                    {
                                                        EntryDate=DateTime.Today,
                                                        Amount=0
                                                    });
                        cvDatabase.SaveChanges();
                    }
                }
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("Loading Bill Configuration File", ErrorException);
            }
        }
        /// <summary>
        /// Check Minimum Requirement For Create Platform 
        /// </summary>
        public void MinimumRequirement()
        {
            if (Properties.Settings.Default.schreenCapturePath.Equals(string.Empty))
            {
                Properties.Settings.Default.schreenCapturePath = GetMyDocumentFolder("CV Screen Capture ");
                Properties.Settings.Default.Save();
            }
        }

        public string GetMyDocumentFolder(string FolderName)
        {
            string dirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FolderName);
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }

        /// <summary>
        /// Get The Temp Folder Name
        /// </summary>
        /// <param name="SoftwareName"></param>
        /// <returns></returns>
        public string GetTempFolder(string SoftwareName)
        {
            string dirPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SoftwareName);
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
        /// <summary>
        /// Get Server Pc IP Address
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
           return Properties.Settings.Default.ServerIpAddress;
        }
        /// <summary>
        /// Convert Image Box To Byte[]
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
    }
}
