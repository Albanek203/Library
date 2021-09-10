using System.Collections.Generic;
using Library.Data.Models;
using Library.Data.Repository;

namespace Library.Data.Service {
    public class AuthorService {
        private readonly AuthorRepository _authorRepository;
        public AuthorService(AuthorRepository authorRepository) { _authorRepository = authorRepository; }
        //
        public void Add(string authorName, string authorSurname) =>
            _authorRepository.Add(new Author { Name = authorName, Surname = authorSurname });
        public Author Find(string authorName, string authorSurname) =>
            _authorRepository.Find(new Author { Name = authorName, Surname = authorSurname });
        public IEnumerable<Author> FindAll() => _authorRepository.FindAll(null);
        public int GetId(string authorName, string authorSurname) =>
            _authorRepository.GetId(new Author { Name = authorName, Surname = authorSurname });
    }
}