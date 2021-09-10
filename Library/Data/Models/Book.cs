using System.Windows.Controls;

namespace Library.Data.Models {
    public class Book {
        public int    BookId        { get; set; }
        public string BookName      { get; set; }
        public string AuthorName    { get; set; }
        public string AuthorSurname { get; set; }
        public Image  Image         { get; set; }
        public string BookCategory  { get; set; }
        public int    YearEdition   { get; set; }
        public string Description   { get; set; }
    }
}