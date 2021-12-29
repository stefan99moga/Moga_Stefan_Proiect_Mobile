using Moga_Stefan_Proiect.Models;
//using Moga_Stefan_Proiect.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moga_Stefan_Proiect.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string Title {get; set; }
        private bool _isRefreshing = false;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
