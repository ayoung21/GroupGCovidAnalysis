﻿#pragma checksum "C:\Users\ayoung21\Desktop\GroupGCovidAnalysis\Covid19Analysis\View\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4D5BF0E13506268CADE58811100AF1EE"
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
            case 2: // View\MainPage.xaml line 29
                {
                    this.summaryTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // View\MainPage.xaml line 31
                {
                    this.lowerThresholdTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.lowerThresholdTextBox).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.lowerThresholdTextBox).KeyDown += this.LowerThreshold_KeyDown;
                }
                break;
            case 4: // View\MainPage.xaml line 32
                {
                    this.upperThresholdTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.upperThresholdTextBox).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.upperThresholdTextBox).KeyDown += this.UpperThreshold_KeyDown;
                }
                break;
            case 5: // View\MainPage.xaml line 33
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.clearData_Click;
                }
                break;
            case 6: // View\MainPage.xaml line 34
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.saveData_Click;
                }
                break;
            case 7: // View\MainPage.xaml line 35
                {
                    this.comboboxState = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 8: // View\MainPage.xaml line 36
                {
                    this.textBoxPositiveTests = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.textBoxPositiveTests).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                }
                break;
            case 9: // View\MainPage.xaml line 37
                {
                    this.textBoxNegativeTests = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.textBoxNegativeTests).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                }
                break;
            case 10: // View\MainPage.xaml line 38
                {
                    this.textBoxDeaths = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.textBoxDeaths).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                }
                break;
            case 11: // View\MainPage.xaml line 39
                {
                    this.textBoxHospitalizations = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.textBoxHospitalizations).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                }
                break;
            case 12: // View\MainPage.xaml line 40
                {
                    this.datePickerCovidCase = (global::Windows.UI.Xaml.Controls.CalendarDatePicker)(target);
                }
                break;
            case 13: // View\MainPage.xaml line 41
                {
                    global::Windows.UI.Xaml.Controls.Button element13 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element13).Click += this.buttonAddNewEntry_Click;
                }
                break;
            case 14: // View\MainPage.xaml line 42
                {
                    this.textBlockCovidEntryErrorMessage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 15: // View\MainPage.xaml line 43
                {
                    this.binSizeTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.binSizeTextBox).BeforeTextChanging += this.TextBox_BeforeTextChanging;
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.binSizeTextBox).KeyDown += this.UpperThreshold_KeyDown;
                }
                break;
            case 16: // View\MainPage.xaml line 44
                {
                    this.comboboxLocationSelection = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.comboboxLocationSelection).SelectionChanged += this.comboboxLocationSelection_SelectionChanged;
                }
                break;
            case 17: // View\MainPage.xaml line 45
                {
                    global::Windows.UI.Xaml.Controls.Button element17 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element17).Click += this.onUpdateSummary_Click;
                }
                break;
            case 18: // View\MainPage.xaml line 26
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element18 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element18).Click += this.displayErrors_Click;
                }
                break;
            case 19: // View\MainPage.xaml line 27
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element19 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element19).Click += this.loadFile_Click;
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
