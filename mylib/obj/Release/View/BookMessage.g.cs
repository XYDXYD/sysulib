﻿#pragma checksum "F:\wp7\sysulib\mylib\View\BookMessage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9FD213333BAC98BC317D39473159CAA4"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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


namespace mylib.View {
    
    
    public partial class BookMessage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock bookTxt;
        
        internal System.Windows.Controls.TextBlock authorTxt;
        
        internal System.Windows.Controls.TextBlock publishTxt;
        
        internal System.Windows.Controls.TextBlock searchNumTxt;
        
        internal System.Windows.Controls.TextBlock isbnTxt;
        
        internal System.Windows.Controls.Image bookCover;
        
        internal System.Windows.Controls.TextBlock textblock1;
        
        internal System.Windows.Controls.ListBox BookStatus;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/mylib;component/View/BookMessage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.bookTxt = ((System.Windows.Controls.TextBlock)(this.FindName("bookTxt")));
            this.authorTxt = ((System.Windows.Controls.TextBlock)(this.FindName("authorTxt")));
            this.publishTxt = ((System.Windows.Controls.TextBlock)(this.FindName("publishTxt")));
            this.searchNumTxt = ((System.Windows.Controls.TextBlock)(this.FindName("searchNumTxt")));
            this.isbnTxt = ((System.Windows.Controls.TextBlock)(this.FindName("isbnTxt")));
            this.bookCover = ((System.Windows.Controls.Image)(this.FindName("bookCover")));
            this.textblock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textblock1")));
            this.BookStatus = ((System.Windows.Controls.ListBox)(this.FindName("BookStatus")));
        }
    }
}

