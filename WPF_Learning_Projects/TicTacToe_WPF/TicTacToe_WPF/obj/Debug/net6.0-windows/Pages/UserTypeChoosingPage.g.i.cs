// Updated by XamlIntelliSenseFileGenerator 9/28/2024 1:36:00 PM
#pragma checksum "..\..\..\..\Pages\UserTypeChoosingPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5738ED98D5DA5512162186370676D608E927067B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TicTacToe_WPF.Pages;


namespace TicTacToe_WPF.Pages
{


    /// <summary>
    /// UserTypeChoosingPage
    /// </summary>
    public partial class UserTypeChoosingPage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {


#line 12 "..\..\..\..\Pages\UserTypeChoosingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ServerChoiceBtn;

#line default
#line hidden


#line 13 "..\..\..\..\Pages\UserTypeChoosingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClientChoiceBtn;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.10.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TicTacToe_WPF;component/pages/usertypechoosingpage.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\Pages\UserTypeChoosingPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.ServerChoiceBtn = ((System.Windows.Controls.Button)(target));

#line 12 "..\..\..\..\Pages\UserTypeChoosingPage.xaml"
                    this.ServerChoiceBtn.Click += new System.Windows.RoutedEventHandler(this.ServerChoiceBtn_Click);

#line default
#line hidden
                    return;
                case 2:
                    this.ClientChoiceBtn = ((System.Windows.Controls.Button)(target));

#line 13 "..\..\..\..\Pages\UserTypeChoosingPage.xaml"
                    this.ClientChoiceBtn.Click += new System.Windows.RoutedEventHandler(this.ClientChoiceBtn_Click);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.TextBlock WaitingTextBlock;
    }
}

