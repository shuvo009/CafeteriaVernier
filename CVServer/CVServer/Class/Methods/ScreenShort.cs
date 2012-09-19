using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
namespace Procesta.CvServer.Class.Methods
{
    public class ScreenShort
    {
        private static ThreadStart startScheenCapture;
        private static Thread screenCaptureThread;
        /// <summary>
        /// Start Screenshot
        /// </summary>
        public static void start()
        {
            startScheenCapture = new ThreadStart(screenCapture);
            screenCaptureThread = new Thread(startScheenCapture);
            if (Properties.Settings.Default.IsCupture)
            {
                if (!screenCaptureThread.IsAlive)
                {
                    screenCaptureThread.Start();
                }
            }
        }
        /// <summary>
        /// Stop screenshot
        /// </summary>
        public static void Stop()
        {
            if (screenCaptureThread!=null && screenCaptureThread.IsAlive)
            {
                screenCaptureThread.Abort();
            }
        }
        /// <summary>
        /// Capture screen after a time
        /// </summary>
        private static void screenCapture()
        {
            try
            {
                while(true)
                {
                    System.Drawing.Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                        }
                        bitmap.Save(ImageNameMaker(), ImageFormat.Jpeg);
                    }
                    Thread.Sleep(Properties.Settings.Default.CuptureTime);
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Create a Image Name
        /// </summary>
        /// <returns></returns>
        private static string ImageNameMaker()
        {
            string diris =System.IO.Path.Combine(Properties.Settings.Default.schreenCapturePath.Equals(string.Empty) ? 
                new MiraculousMethods().GetMyDocumentFolder("CV Screen Capture ") : 
                Properties.Settings.Default.schreenCapturePath,DateTime.Now.ToString("yyyyMMdd"));
            if (!System.IO.Directory.Exists(diris))
            {
                System.IO.Directory.CreateDirectory(diris);
            }
            string fileNameIs = DateTime.Now.ToString("yyyyMMddHHmmss")+".jpg";
            return System.IO.Path.Combine(diris, fileNameIs);
        }
    }
}
