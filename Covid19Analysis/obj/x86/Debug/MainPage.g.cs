﻿#pragma checksum "C:\Users\csuser\Desktop\Covid-Analysis\Covid19Analysis\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "349FD669663E7F9BC3A36E21F7E3D58A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Covid19Analysis
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 18
                {
                    this.summaryTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // MainPage.xaml line 22
                {
                    this.lowerThresholdTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.lowerThresholdTextBox).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.lowerThresholdTextBox).KeyDown += this.LowerThreshold_KeyDown;
                }
                break;
            case 4: // MainPage.xaml line 24
                {
                    this.upperThresholdTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.upperThresholdTextBox).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.upperThresholdTextBox).KeyDown += this.UpperThreshold_KeyDown;
                }
                break;
            case 5: // MainPage.xaml line 25
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.clearData_Click;
                }
                break;
            case 6: // MainPage.xaml line 15
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element6 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element6).Click += this.displayErrors_Click;
                }
                break;
            case 7: // MainPage.xaml line 16
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element7 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element7).Click += this.loadFile_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

