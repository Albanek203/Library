using System;
using Library.Data.Models;

namespace Library.ViewModel {
    public class ViewModelRegisterUser : ViewModelBase {
        private string _userName;
        public string UserName {
            get => _userName;
            set {
                _userName = string.Empty;
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("You have not entered a value");
                if(!char.IsUpper(value[0]))
                    throw new ArgumentException("The name must start with a capital letter");
                if (value.Length < 2)
                    throw new ArgumentException("The name cannot be 1 letter");
                for(var i = 1; i < value.Length; i++)
                    if(char.IsUpper(value[i]))
                        throw new ArgumentException("You cannot use Caps Lock in a name");
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        private string _userSurname;
        public string UserSurname {
            get => _userSurname;
            set {
                _userSurname = string.Empty;
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("You have not entered a value");
                if(!char.IsUpper(value[0]))
                    throw new ArgumentException("The surname must start with a capital letter");
                if(value.Length < 2)
                    throw new ArgumentException("The surname cannot be 1 letter");
                for(var i = 1; i < value.Length; i++)
                    if(char.IsUpper(value[i]))
                        throw new ArgumentException("You cannot use Caps Lock in a surname");
                _userSurname = value;
                OnPropertyChanged(nameof(UserSurname));
            }
        }
        private string _userPhone;
        public string UserPhone {
            get => _userPhone;
            set {
                _userPhone = string.Empty;
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("You have not entered a value");
                if(value.Length != 10)
                    throw new ArgumentException("Invalid phone entered");
                _userPhone = value;
                OnPropertyChanged(nameof(UserPhone));
            }
        }
        private string _userAddress;
        public string UserAddress {
            get => _userAddress;
            set {
                _userAddress = string.Empty;
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("You have not entered a value");
                if(!char.IsUpper(value[0]))
                    throw new ArgumentException("The address must start with a capital letter");
                if(value.Length < 2)
                    throw new ArgumentException("The address cannot be 1 letter");
                for(var i = 1; i < value.Length; i++)
                    if(char.IsUpper(value[i]))
                        throw new ArgumentException("You cannot use Caps Lock in a address");
                _userAddress = value;
                OnPropertyChanged(nameof(UserAddress));
            }
        }
        private string _userLogin;
        public string UserLogin {
            get => _userLogin;
            set {
                _userLogin = string.Empty;
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("You have not entered a value");
                if(value.Length < 6)
                    throw new ArgumentException("Minimum login length 6");
                if(char.IsDigit(value[0]))
                    throw new ArgumentException("Login cannot start with a number");
                _userLogin = value;
                OnPropertyChanged(nameof(UserLogin));
            }
        }
        private string _userEmail;
        public string UserEmail {
            get => _userEmail;
            set {
                _userEmail = string.Empty;
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("You have not entered a value");
                if(value.Length < 8)
                    throw new ArgumentException("Not a correct email");
                if(!value.Contains("@") || !value.Contains("."))
                    throw new ArgumentException("Email must contain characters such as \"@\" and \".\"");
                _userEmail = value;
                OnPropertyChanged(nameof(UserEmail));
            }
        }
        private string _userPassword;
        public string UserPassword {
            get => _userPassword;
            set {
                if(!string.IsNullOrWhiteSpace(value))
                    _userPassword = value;
                OnPropertyChanged(nameof(UserPassword));
            }
        }
        private string _confirmation;
        public string Confirmation {
            get => _confirmation;
            set {
                if(!string.IsNullOrWhiteSpace(value))
                    _confirmation = value;
                OnPropertyChanged(nameof(Confirmation));
            }
        }
        public User GetUser =>
            new() {
                Name = _userName
              , Surname = _userSurname
              , Login = _userLogin
              , Password = _userPassword
              , Email = _userEmail
              , Address = _userAddress
              , Phone = _userPhone
            };
    }
}
