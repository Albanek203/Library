using System.Linq;
using Library.Data.Models;
using Library.Data.Service;

namespace Library.Data.Pages.Account {
    public partial class ReceivedBooksPage {
        public ReceivedBooksPage(ReceivedBooksService receivedBooksService, User user) {
            InitializeComponent();
            var listIssuance =
                receivedBooksService.FindAll(new ReceivedBook { User = new User { UserId = user.UserId } });
            var listBook = listIssuance?.Select(issuance => new OwnBook {
                Name      = issuance.Book.BookName
              , Author    = issuance.Book.AuthorName + " " + issuance.Book.AuthorSurname
              , Receiving = issuance.ReceivingDate.ToString("dd/MM/yyyy")
              , Delivery  = issuance.DeliveryDate.ToString("dd/MM/yyyy")
            }).ToList();
            DataGrid.ItemsSource = listBook;
        }
    }
}