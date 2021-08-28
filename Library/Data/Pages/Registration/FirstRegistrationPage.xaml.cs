using System.Text.RegularExpressions;
using System.Windows.Input;
using Library.ViewModel;

namespace Library.Data.Pages.Registration {
    /// <summary>
    /// Interaction logic for FirstRegistrationPage.xaml
    /// </summary>
    public partial class FirstRegistrationPage {
        public FirstRegistrationPage(ViewModelRegisterUser viewModelRegisterUser) {
            InitializeComponent();
            DataContext = viewModelRegisterUser;
        }
        private void TxtUserName_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TxtUserSurname_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TxtUserPhone_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TxtUserAddress_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}