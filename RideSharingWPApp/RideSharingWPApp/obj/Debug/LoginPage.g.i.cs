﻿#pragma checksum "E:\xampp\htdocs\SeniorProject\WindowPhoneApp\RideSharingWPApp\RideSharingWPApp\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1AD8CDCA645C9F93FBD3357DDBB5017D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
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


namespace RideSharingWPApp {
    
    
    public partial class LoginPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtbEmail;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtbPassword;
        
        internal System.Windows.Controls.Button btnLogin;
        
        internal System.Windows.Controls.Button btnRegister;
        
        internal System.Windows.Controls.HyperlinkButton linkForgotPass;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RideSharingWPApp;component/LoginPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtbEmail = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtbEmail")));
            this.txtbPassword = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtbPassword")));
            this.btnLogin = ((System.Windows.Controls.Button)(this.FindName("btnLogin")));
            this.btnRegister = ((System.Windows.Controls.Button)(this.FindName("btnRegister")));
            this.linkForgotPass = ((System.Windows.Controls.HyperlinkButton)(this.FindName("linkForgotPass")));
        }
    }
}

