using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace TheQDeviceConnect.Core.ViewModels.Connection
{
    public interface IWifiNetworkViewModel : IMvxNotifyPropertyChanged
    {
        string ssid { get; set; }
        void OnPropertyChanged(string propertyName);
    }
    public class WifiNetworkViewModel : BaseViewModel, IWifiNetworkViewModel
    {
        public IMvxCommand WifiNetworkRowItemClickedCommand { get; private set; }

        private bool _isClicked;
        public bool IsClicked
        {
            get { return _isClicked; }
            set
            {
                _isClicked = value;
                RaisePropertyChanged(() => IsClicked);
                OnPropertyChanged("IsClicked"); // notify parent view model (e.g. WifiConnectionViewModel)
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
                OnPropertyChanged("IsSelected");
            }
        }
        
        public WifiNetworkViewModel()
        {
            WifiNetworkRowItemClickedCommand = new MvxCommand(HandleWifiNetworkRowItemClicked);
        }

        private void HandleWifiNetworkRowItemClicked()
        {
            IsClicked = true;
        }

        public string ssid { get; set ; }
    }
}
