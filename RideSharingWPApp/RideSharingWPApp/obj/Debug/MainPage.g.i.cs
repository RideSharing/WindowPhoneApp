﻿#pragma checksum "E:\xampp\htdocs\SeniorProject\WindowPhoneApp\RideSharingWPApp\RideSharingWPApp\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "70B51C28BE2512501C70A9792A8406C7"
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
using Microsoft.Phone.Maps.Controls;
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
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock txtUserName;
        
        internal Microsoft.Phone.Controls.LongListSelector longlistMainMenu;
        
        internal System.Windows.Controls.TextBox txtboxSearch;
        
        internal Microsoft.Phone.Maps.Controls.Map mapMain;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RideSharingWPApp;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtUserName = ((System.Windows.Controls.TextBlock)(this.FindName("txtUserName")));
            this.longlistMainMenu = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("longlistMainMenu")));
            this.txtboxSearch = ((System.Windows.Controls.TextBox)(this.FindName("txtboxSearch")));
            this.mapMain = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("mapMain")));
        }
    }
}

