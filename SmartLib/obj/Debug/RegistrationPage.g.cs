﻿#pragma checksum "C:\Users\chovik\Dropbox\BakalarskaPraca\BakalarskaPraca\SmartLib\SmartLib\RegistrationPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DE81C66510C8ACEC6C611A238DDABAF8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.544
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SmartLib {
    
    
    public partial class RegistrationPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Grid grid1;
        
        internal System.Windows.Controls.TextBox titleTextBox;
        
        internal System.Windows.Controls.TextBox authorTextBox;
        
        internal System.Windows.Controls.TextBox isbnTextBox;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/SmartLib;component/RegistrationPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.grid1 = ((System.Windows.Controls.Grid)(this.FindName("grid1")));
            this.titleTextBox = ((System.Windows.Controls.TextBox)(this.FindName("titleTextBox")));
            this.authorTextBox = ((System.Windows.Controls.TextBox)(this.FindName("authorTextBox")));
            this.isbnTextBox = ((System.Windows.Controls.TextBox)(this.FindName("isbnTextBox")));
        }
    }
}

