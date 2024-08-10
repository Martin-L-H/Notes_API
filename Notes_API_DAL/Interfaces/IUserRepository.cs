using Notes_API_CORE.Entities;

namespace Notes_API_DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUserById(int id);
        public Task<bool> IsMailRegistered(string mailToCheck);
        public Task<string> GetSalt(string usermail);
        public Task<User> Login(User user);
        public Task<bool> RegisterUser(User newUser);
        public Task<bool> DeleteUser(int id);
        public Task<User> GetUserByMail(string mailToCheck);
    }
}
