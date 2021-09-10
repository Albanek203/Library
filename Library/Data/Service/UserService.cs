using System.Collections.Generic;
using Library.Data.Models;
using Library.Data.Repository;

namespace Library.Data.Service {
    public class UserService {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository) { _userRepository = userRepository; }
        //
        public User Find(User data) => _userRepository.Find(data);
        public IEnumerable<User> FindAll() => _userRepository.FindAll(null);
        public bool IsExists(int userId) => _userRepository.IsExists(userId);
        public void ChangePhoto(int userId, string url) => _userRepository.ChangePhoto(userId, url);
        public void ChangeName(int userId, string newName) => _userRepository.ChangeName(userId, newName);
        public void ChangeSurname(int userId, string newSurname) => _userRepository.ChangeSurname(userId, newSurname);
        public void ChangeLogin(int userId, string newLogin) => _userRepository.ChangeLogin(userId, newLogin);
        public void ChangePassword(int userId, string newPassword) =>
            _userRepository.ChangePassword(userId, newPassword);
        public void AddAdvancedAccess(int userId) => _userRepository.AddAdvancedAccess(userId);
    }
}