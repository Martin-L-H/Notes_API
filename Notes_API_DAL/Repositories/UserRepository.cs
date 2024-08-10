using Microsoft.EntityFrameworkCore;
using Notes_API_CORE.Entities;
using Notes_API_DAL.Context;
using Notes_API_DAL.Interfaces;

namespace Notes_API_DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        //DEBUGGER
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            IEnumerable<User> list;
            list = await _context.Users.AsNoTracking().ToListAsync();
            return list;
        }
        public async Task<User> GetUserById(int id)
        {
            User selectedUser;
            selectedUser = await _context.Users.FindAsync(id);
            return selectedUser;
        }
        public async Task<bool> IsMailRegistered(string mailToCheck)
        {
            List<User> list = await _context.Users.Where(p => p.User_Mail == mailToCheck).AsNoTracking().ToListAsync();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> GetSalt(string usermail)
        {
            User userAttempt = await _context.Users.Where(p => p.User_Mail == usermail).AsNoTracking().FirstOrDefaultAsync();
            if (userAttempt==null)
            {
                return null;
            }
            else
            {
                return userAttempt.User_Salt;
            }
        }
        public async Task<User> Login(User user)
        {
            User userAttempt = await _context.Users.Where(p => p.User_Mail == user.User_Mail && p.User_Password == user.User_Password).AsNoTracking().FirstOrDefaultAsync();
            if (userAttempt!=null)
            {
                return userAttempt;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> RegisterUser(User newUser)
        {
            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var userToDelete = await _context.Users.FindAsync(id);
                if (userToDelete.User_Verified == true)
                {
                    userToDelete.User_Disabled = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<User> GetUserByMail(string mailToCheck)
        {
            User selectedUser;
            selectedUser = await _context.Users.Where(p => p.User_Mail == mailToCheck).AsNoTracking().FirstOrDefaultAsync();
            return selectedUser;
        }
    }
}
