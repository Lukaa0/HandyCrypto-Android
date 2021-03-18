using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HandyCrypto.Model
{
    class MessageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private string username;
        private ImageView userimage;
        private string content;
        public string Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
            
        }

        

        public string Username {

            get => username;
            set
            {
                username = value;
                OnPropertyChanged("Username");

            }
        }

        public ImageView Userimage
        {
            get => userimage;
            set
            {
                userimage = value;
                OnPropertyChanged("Userimage");
            }
        }

         }


    }
