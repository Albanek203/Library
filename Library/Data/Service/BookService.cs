using System.Collections.Generic;
using Library.Data.Models;
using Library.Data.Repository;

namespace Library.Data.Service {
    public class BookService {
        private readonly BookRepository _bookRepository;
        public BookService(BookRepository bookRepository) { _bookRepository = bookRepository; }
        //
        public void              Add(Book        data)     => _bookRepository.Add(data);
        public Book              Find(Book       data)     => _bookRepository.Find(data);
        public IEnumerable<Book> FindAll(Book    data)     => _bookRepository.FindAll(data);
        public bool              IsExists(string bookName) => _bookRepository.IsExists(bookName);
        public int               GetId(string    bookName) => _bookRepository.GetId(bookName);
    }
}