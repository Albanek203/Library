using System.Windows.Controls;
using Library.Data.Models;
using Library.Data.Service;

namespace Library.Data.Pages.Library {
    public partial class GlobalLibraryPage {
        public GlobalLibraryPage(BookService bookService) {
            InitializeComponent();
            var receivedBook = bookService.Find(new Book() { Description = "find me" });
            DataContext         = receivedBook;
            BookImg.Source      = receivedBook.Image.Source;
            AuthorFullName.Text = "( " + receivedBook.AuthorName + " " + receivedBook.AuthorSurname + " )";
        }
    }
}