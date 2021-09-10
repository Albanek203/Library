using System.Collections.Generic;
using Library.Data.Models;
using Library.Data.Repository;

namespace Library.Data.Service {
    public class ReceivedBooksService {
        private readonly ReceivedBooksRepository _receivedBooksRepository;
        public ReceivedBooksService(ReceivedBooksRepository receivedBooksRepository) {
            _receivedBooksRepository = receivedBooksRepository;
        }
        public void                      Add(ReceivedBook     data) => _receivedBooksRepository.Add(data);
        public IEnumerable<ReceivedBook> FindAll(ReceivedBook data) => _receivedBooksRepository.FindAll(data);
    }
}