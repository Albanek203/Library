using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Library.ViewModel;

namespace Library.Data.Pages.Registration {
    /// <summary>
    /// Interaction logic for SecondRegistrationPage.xaml
    /// </summary>
    public partial class SecondRegistrationPage {
        private readonly ViewModelRegisterUser _viewModelRegisterUser;
        public SecondRegistrationPage(ViewModelRegisterUser viewModelRegisterUser) {
            InitializeComponent();
            DataContext = viewModelRegisterUser;
            _viewModelRegisterUser = viewModelRegisterUser;
        }
        private void TxtUserPassword_OnPasswordChanged(object sender, RoutedEventArgs e) {
            _viewModelRegisterUser.UserPassword = TxtUserPassword.Password;
        }
        private void TxtConfirmation_OnPasswordChanged(object sender, RoutedEventArgs e) {
            _viewModelRegisterUser.Confirmation = TxtConfirmation.Password;
        }
        private void TxtUserLogin_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^a-zA-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
