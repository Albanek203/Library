using System.Collections.Generic;
using Library.Data.Models;
using Library.Data.Repository;

namespace Library.Data.Service {
    public class UserService {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository) { _userRepository = userRepository; }
        //
        public User              Find(User data)      => _userRepository.Find(data);
        public IEnumerable<User> FindAll()            => _userRepository.FindAll(null);
        public bool              IsExists(int userId) => _userRepository.IsExists(userId);
    }
}