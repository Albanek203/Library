using System;
using System.Collections.ObjectModel;
using System.Windows;
using Library.Data.Enumeration;

namespace Library.ViewModel {
    public class ViewModelTheme : ViewModelBase {
        private Themes _theme;
        public Themes Theme {
            get => _theme;
            set {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }
        public ObservableCollection<object> ThemesList { get; }
        public ViewModelTheme() {
            ThemesList = new ObservableCollection<object>();
            foreach(var i in Enum.GetValues(typeof(Themes))) { ThemesList.Add(i); }
            Theme = Themes.Dark;
            ChangeTheme(Theme);
        }
        public void ChangeTheme(Themes theme) {
            _theme = theme;
            var name = theme.ToString();
            var uri  = new Uri(@"Data\Themes\" + name + ".xaml", UriKind.Relative);

            var resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}