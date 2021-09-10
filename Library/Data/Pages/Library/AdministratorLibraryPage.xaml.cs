using System.Windows;
using System.Windows.Controls;
using Library.Data.Enumeration;
using Library.Data.Models;
using Library.Data.Service;
using Library.View.AdditionalView;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Data.Pages.Library {
    public partial class AdministratorLibraryPage {
        private BookService _bookService;
        public AdministratorLibraryPage(BookService bookService) {
            InitializeComponent();
            _bookService = bookService;
        }
        private void ButtonAddBook_OnClick(object sender, RoutedEventArgs e) {
            var fillBookWindow = App.ServiceProvider.GetService<FillBookWindow>();
            if (fillBookWindow?.ShowDialog() != true) { return; }

            if (_bookService.IsExists(fillBookWindow.Book.BookName)) {
                MessageWindow.Show(null, "This book already exists", TypeWindow.ErrorWindow, MessageButton.Ok);
                return;
            }

            MessageBox.Show(fillBookWindow.Book.YearEdition.ToString());
            _bookService.Add(fillBookWindow.Book);
        }
        private void ButtonAddCategory_OnClick(object sender, RoutedEventArgs e) {
            /*Igron*/
        }
        private void TxtSearchUser_OnTextChanged(object sender, TextChangedEventArgs e) {
            /*Igron*/
        }
        private void AddAdvancedAccess_OnClick(object sender, RoutedEventArgs e) {
            /*Igron*/
        }
    }
}