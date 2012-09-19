using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebCam_Capture;
using DevExpress.Xpf.Core;
using System.IO;

namespace Procesta.CvServer
{
	/// <summary>
	/// Interaction logic for WebCamWindow.xaml
	/// </summary>
	public partial class WebCamWindow : Window
	{
        private WebCamCapture webCam;
        private Image _FrameImage;
        private int frameNumber = 30;
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handel);
        public static BitmapSource bs;
        public static IntPtr ip;
        private byte[] _ImageInBytes;
		public WebCamWindow()
		{
			this.InitializeComponent();
            webCam = new WebCamCapture();
            webCam.FrameNumber=((ulong)(0ul));
            webCam.TimeToCapture_milliseconds = frameNumber;
            webCam.ImageCaptured += new WebCamCapture.WebCamEventHandler(webCam_ImageCaptured);
            _FrameImage = this.SourceImage;
        }

        void webCam_ImageCaptured(object source, WebcamEventArgs e)
        {
            try
            {
                ip = ((System.Drawing.Bitmap)e.WebCamImage).GetHbitmap();
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(ip);
                _FrameImage.Source = bs;
            }
            catch (Exception ErrorException)
            {
                LogFileWriter.ErrorToLog("webCam_ImageCaptured", ErrorException);
                DXMessageBox.Show(ErrorException.Message, CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand StartCoomand
        {
            get { return new MainWindow.ReplayCommand(new Action<object>(this.camStart)); }
        }

        public ICommand StopCommand
        {
            get { return new MainWindow.ReplayCommand(new Action<object>(this.camStop)); }
        }

        public ICommand ContinueComand
        {
            get { return new MainWindow.ReplayCommand(new Action<object>(this.camContinue)); }
        }

        public ICommand CaptureCommand
        {
            get { return new MainWindow.ReplayCommand(new Action<object>(this.camCapture)); }
        }

        public ICommand VideoFromateCommand
        {
            get { return new MainWindow.ReplayCommand(new Action<object>(this.videoFormate)); }
        }

        public ICommand VideoSourceCommand
        {
            get { return new MainWindow.ReplayCommand(new Action<object>(this.videoSource)); }
        }

        public ICommand DoneCommand
        {
            get { return new MainWindow.ReplayCommand(new Action<object>(this.done)); }
        }

        private void camStart(object obj)
        {
            webCam.TimeToCapture_milliseconds = frameNumber;
            webCam.Start(0);
        }

        private void camStop(object obj)
        {
            webCam.Stop();
        }

        private void camContinue(object obj) 
        {
            webCam.TimeToCapture_milliseconds = frameNumber;
            webCam.Start(this.webCam.FrameNumber);
        }

        private void camCapture(object obj)
        {
            this.GetImage.Source = this.SourceImage.Source;
        }

        private void videoFormate(object obj)
        {
            webCam.Config();
        }

        private void videoSource(object obj)
        {
            webCam.Config2();
        }

        private void done(object obj)
        {
            if (obj!=null)
            {
                BitmapSource captuteImage = (obj as BitmapSource);
                using (MemoryStream memStrem= new MemoryStream())
                {
                    JpegBitmapEncoder jpgeEncoder = new JpegBitmapEncoder();
                    jpgeEncoder.Frames.Add(BitmapFrame.Create(captuteImage));
                    jpgeEncoder.Save(memStrem);
                    this._ImageInBytes = memStrem.GetBuffer();
                }
                webCam.Stop();
                this.Close();
            }
            else
            {
                DXMessageBox.Show("Please capture image first.", CvVariables.SOFTWARE_NAME, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static byte[] CaptureImage()
        {
            WebCamWindow camCapture = new WebCamWindow();
            camCapture.ShowDialog();
            if (camCapture._ImageInBytes !=null)
            {
                return camCapture._ImageInBytes;
            }
            else
            {
                return default(byte[]);
            }
        }
	}
}