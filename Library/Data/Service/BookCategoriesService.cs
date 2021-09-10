using System.Collections.Generic;
using Library.Data.Repository;

namespace Library.Data.Service {
    public class BookCategoriesService {
        private readonly BookCategoriesRepository _bookCategoriesRepository;
        public BookCategoriesService(BookCategoriesRepository bookCategoriesRepository) {
            _bookCategoriesRepository = bookCategoriesRepository;
        }
        //
        public void                Add(string categoryName)   => _bookCategoriesRepository.Add(categoryName);
        public IEnumerable<string> FindAll()                  => _bookCategoriesRepository.FindAll(null);
        public int                 GetId(string categoryName) => _bookCategoriesRepository.GetId(categoryName);
    }
}