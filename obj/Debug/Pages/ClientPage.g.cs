﻿#pragma checksum "..\..\..\Pages\ClientPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0CC7A8DEEE2D2EF99BA11976D9A0FEA6A4F1227248B071D53F04371278F075D7"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using DishesApplication.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace DishesApplication.Pages {
    
    
    /// <summary>
    /// ClientPage
    /// </summary>
    public partial class ClientPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 33 "..\..\..\Pages\ClientPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbPoisk;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Pages\ClientPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbSort;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Pages\ClientPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbFilter;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\Pages\ClientPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock outputQuantityProducts;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\ClientPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock allQuantityProducts;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Pages\ClientPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvProducts;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DishesApplication;component/pages/clientpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\ClientPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tbPoisk = ((System.Windows.Controls.TextBox)(target));
            
            #line 33 "..\..\..\Pages\ClientPage.xaml"
            this.tbPoisk.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbPoisk_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cbSort = ((System.Windows.Controls.ComboBox)(target));
            
            #line 34 "..\..\..\Pages\ClientPage.xaml"
            this.cbSort.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbSort_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cbFilter = ((System.Windows.Controls.ComboBox)(target));
            
            #line 35 "..\..\..\Pages\ClientPage.xaml"
            this.cbFilter.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbFilter_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.outputQuantityProducts = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.allQuantityProducts = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.lvProducts = ((System.Windows.Controls.ListView)(target));
            return;
            case 8:
            
            #line 88 "..\..\..\Pages\ClientPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnBasket);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 89 "..\..\..\Pages\ClientPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnExit);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 74 "..\..\..\Pages\ClientPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnAddBasket);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

