using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ScannerDemo;
using com.google.zxing.qrcode;
using Microsoft.Devices;
using com.google.zxing;
using com.google.zxing.common;
using System.Diagnostics;
using System.Threading;
using WP7_Barcode_Library;
using com.google.zxing.oned;
using System.Windows.Media.Imaging;
using System.IO;

namespace SmartLib.ViewModels
{
    public class ScannerViewModel : BaseViewModel
    {
        private Timer timer;
        private PhotoCameraLuminanceSource luminance;
        private QRCodeReader qrReader;
        private Code39Reader code39Reader;
        private EAN13Reader ean13Reader;
        private PhotoCamera photoCamera = new PhotoCamera();

        public PhotoCamera PhotoCamera 
        {
            get
            {
                return photoCamera;
            }
            set
            {
                if (value != photoCamera)
                {
                    photoCamera = value;
                    OnNotifyPropertyChanged("PhotoCamera");
                }
            }
        }

        public ScannerViewModel()
        {            
            Initialize();
            
        }

        public void Initialize()
        {
            WP7BarcodeManager.ScanMode = BarcodeFormat.CODE_39;

            photoCamera = new PhotoCamera();
            photoCamera.Initialized += OnPhotoCameraInitialized;
            photoCamera.CaptureImageAvailable += new EventHandler<ContentReadyEventArgs>(photoCamera_CaptureImageAvailable);

            CameraButtons.ShutterKeyHalfPressed += (o, arg) => PhotoCamera.Focus();
            

            
        }

        void photoCamera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            throw new NotImplementedException();
        }

        void photoCamera_CaptureStarted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnPhotoCameraInitialized(object sender, CameraOperationCompletedEventArgs e)
        {
            int width = Convert.ToInt32(PhotoCamera.PreviewResolution.Width);
            int height = Convert.ToInt32(PhotoCamera.PreviewResolution.Height);

            luminance = new PhotoCameraLuminanceSource(width, height);
            qrReader = new QRCodeReader();
            ean13Reader = new EAN13Reader();
            code39Reader = new Code39Reader();

            timer = new Timer((s) => ScanPreviewBuffer(), null, 0, 250);
        }

        BitmapImage GetImage(byte[] rawImageBytes)
        {
            BitmapImage imageSource = null;

            try
            {
                using (MemoryStream stream = new MemoryStream(rawImageBytes))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    BitmapImage b = new BitmapImage();
                    b.SetSource(stream);
                    imageSource = b;
                }
            }
            catch (System.Exception ex)
            {
            }

            return imageSource;
        }


        private void ScanPreviewBuffer()
        {
            string returnedString = null;

            photoCamera.GetPreviewBufferY(luminance.PreviewBufferY);
            var binarizer = new HybridBinarizer(luminance);
            var binBitmap = new BinaryBitmap(binarizer);
            var img = GetImage(luminance.PreviewBufferY);
            WP7BarcodeManager.ScanBarcode(img, result =>
                {
                    Debug.WriteLine(result.BarcodeImage);
                });
            
            try
            {                
                var result = ean13Reader.decode(binBitmap);
                returnedString = result.Text;
                Debug.WriteLine("EAN13  " + returnedString);
                return;
            }
            catch(Exception ex)
            {
            }

            try
            {
                var result = qrReader.decode(binBitmap);
                returnedString = result.Text;
                Debug.WriteLine("QR  " + returnedString);
                return;
            }
            catch (Exception ex)
            {
            }

            try
            {        
                var result = code39Reader.decode(binBitmap);
                returnedString = result.Text;
                Debug.WriteLine("CODE39  " + returnedString);
                return;
            }
            catch (Exception ex)
            {

            }
        }

        public void Barcode_Results(WP7_Barcode_Library.BarcodeCaptureResult Results)
        {
            if (Results.State == WP7_Barcode_Library.CaptureState.Success)
            {

            }
            else //Error occured
            {

            }
        }
    }
}
