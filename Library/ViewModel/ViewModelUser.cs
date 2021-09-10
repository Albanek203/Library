using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Library.Data.Models;

namespace Library.ViewModel {
    public class ViewModelUser : ViewModelBase {
        public ViewModelUser(User user) {
            _imageBrush = @"Data\Images\unknownUser.jpeg";
            _user       = user;
        }
        private string _imageBrush;
        public string ImageBrush {
            get => _imageBrush;
            set {
                _imageBrush = value;
                OnPropertyChanged(nameof(ImageBrush));
            }
        }
        private User _user;
        public User CurrentUser {
            get => _user;
            set {
                _user = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        private string _name;
        public string Name {
            get => _name;
            set {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _surname;
        public string Surname {
            get => _surname;
            set {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        private int _money;
        public int Money {
            get => _money;
            set {
                _money = value;
                OnPropertyChanged(nameof(Money));
            }
        }
        public void SetUser(User user) {
            CurrentUser.UserId                 = user.UserId;
            CurrentUser.Name                   = user.Name;
            CurrentUser.Surname                = user.Surname;
            CurrentUser.Login                  = user.Login;
            CurrentUser.Email                  = user.Email;
            CurrentUser.Password               = user.Password;
            CurrentUser.SubscriptionName       = user.SubscriptionName;
            CurrentUser.SubscriptionValidUntil = user.SubscriptionValidUntil;
            CurrentUser.Address                = user.Address;
            CurrentUser.AdvancedAccess         = user.AdvancedAccess;
            CurrentUser.Money                  = user.Money;
            CurrentUser.Phone                  = user.Phone;
            CurrentUser.Image                  = user.Image;
            _name                              = user.Name;
            _surname                           = user.Surname;
            _money                             = user.Money;
        }
        public void ChangeImage(string path) {
            ImageBrush        = path;
            CurrentUser.Image = new Image { Source = new BitmapImage(new Uri(path)) };
        }
        public void ChangeName(string newName) {
            CurrentUser.Name = newName;
            Name             = newName;
        }
        public void ChangeSurname(string newSurname) {
            CurrentUser.Surname = newSurname;
            Surname             = newSurname;
        }
        public void ChangeLogin(string    newLogin)    { CurrentUser.Login    = newLogin; }
        public void ChangePassword(string newPassword) { CurrentUser.Password = newPassword; }
    }
}