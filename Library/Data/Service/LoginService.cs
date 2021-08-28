using Library.Data.Models;
using Library.Data.Repository;

namespace Library.Data.Service {
    public class LoginService {
        private readonly LoginRepository _loginRepository;
        public LoginService(LoginRepository loginRepository) {
            _loginRepository = loginRepository;
        }
        //
        public int Login(string login, string password) => _loginRepository.Login(login, password);
        public bool Register(User user) => _loginRepository.Register(user);
    }
}