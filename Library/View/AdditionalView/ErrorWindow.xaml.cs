using System;
using System.Windows;
using System.Media;
using Library.Data.Enumeration;

namespace Library.View.AdditionalView {
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow {
        public ErrorWindow(string errorText, MessageButton messageButton) {
            InitializeComponent();
            switch (messageButton) {
                case MessageButton.Ok:
                    FistButton.Visibility = Visibility.Visible;
                    FistButton.Content    = messageButton.ToString();
                    break;
                case MessageButton.YesNo:
                    FistButton.Visibility   = Visibility.Visible;
                    FistButton.Content      = "Yes";
                    SecondButton.Visibility = Visibility.Visible;
                    SecondButton.Content    = "No";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageButton), messageButton, null);
            }
            ErrorTextBlock.Text = errorText;
            var s = new SoundPlayer();
            s.Play();
            s.Dispose();
        }
        private void FirstButton_OnClick(object  sender, RoutedEventArgs e) { DialogResult = true; }
        private void SecondButton_OnClick(object sender, RoutedEventArgs e) { DialogResult = false; }
    }
}