using System.Windows;
using System.Windows.Controls;
using Library.Data.Enumeration;
using Library.ViewModel;

namespace Library.View.AdditionalView {
    public partial class SettingsWindow {
        private readonly ViewModelTheme _viewModelTheme;
        public SettingsWindow(ViewModelTheme viewModelTheme) {
            InitializeComponent();
            DataContext     = viewModelTheme;
            _viewModelTheme = viewModelTheme;
        }
        private void ThemeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var themeId = ThemeSelector.SelectedIndex;
            _viewModelTheme.ChangeTheme((Themes)themeId);
        }
        private void ButtonApply_OnClick(object sender, RoutedEventArgs e) { DialogResult = true; }
    }
}