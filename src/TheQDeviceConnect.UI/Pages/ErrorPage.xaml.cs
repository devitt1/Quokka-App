using System;
using System.Collections.Generic;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TheQDeviceConnect.Core.ViewModels.Error;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheQDeviceConnect.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class ErrorPage : MvxContentPage<ErrorViewModel>
    {
        public ErrorPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
                navigationPage.BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
        }
        
    }
}
