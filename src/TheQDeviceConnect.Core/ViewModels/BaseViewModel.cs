using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MvvmCross.ViewModels;

namespace TheQDeviceConnect.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public BaseViewModel()
        {

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // MVVM Commands

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
