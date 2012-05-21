using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using com.google.zxing;

namespace WP7.VideoScanZXing.SampleApp
{
    public partial class ScanPage : PhoneApplicationPage
    {
        // Constructor
        public ScanPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WP7.ScanBarCode.BarCodeManager.StartScan(
                // on success
                (b) => Dispatcher.BeginInvoke(() => 
                    {
                        tbScanResultBarCode.Text = b.Text;
                        NavigationService.GoBack();
                    }),
                // on error
                (ex) => Dispatcher.BeginInvoke(() => 
                    {
                        tbScanResultBarCode.Text = ex.Message;
                        NavigationService.GoBack();
                    })
                // Default : please, decode a 13 bar-code
                );
                                         
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WP7.ScanBarCode.BarCodeManager.StartScan(
                // on success
                (b) => Dispatcher.BeginInvoke(() => 
                    {
                        tbScanResultQR.Text = b.Text;
                        NavigationService.GoBack();
                    }),
                // on error
                (ex) => Dispatcher.BeginInvoke(() => 
                    {
                        tbScanResultQR.Text = ex.Message;
                        NavigationService.GoBack();
                    }),
                // Please, decode a QR Code
                BarcodeFormat.QR_CODE);
        }
    }
}