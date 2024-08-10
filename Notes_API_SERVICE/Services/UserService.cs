using AutoMapper;
using Notes_API_CORE.Entities;
using Notes_API_DAL.Interfaces;
using Notes_API_SERVICE.DTOs;
using Notes_API_SERVICE.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Notes_API_SERVICE.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        //DEBUGGER
        public async Task<IEnumerable<CompleteUserDTO>> GetAllUsers()
        {
            IEnumerable<User> dbList = await _userRepository.GetAllUsers();
            IEnumerable<CompleteUserDTO> dtoList = _mapper.Map<IEnumerable<CompleteUserDTO>>(dbList);
            return dtoList;
        }
        public async Task<CompleteUserDTO> GetUserById(int id)
        {
            User dbUser;
            dbUser = await _userRepository.GetUserById(id);
            CompleteUserDTO dtoUser;
            dtoUser = _mapper.Map<CompleteUserDTO>(dbUser);
            return dtoUser;
        }
        public async Task<bool> CreateUser(CreateUserDTO dtoNewUser)
        {
            bool isMailRegistered = await _userRepository.IsMailRegistered(dtoNewUser.User_Mail);
            if (isMailRegistered == true)
            {
                return false;
            }
            else
            {
                User newUser = _mapper.Map<User>(dtoNewUser);
                newUser.User_Disabled = false;

                //IF MAIL VERIFICATION ADDED, MAKE THIS TO FALSE
                newUser.User_Verified = true;

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buffer = new byte[64];
                rng.GetBytes(buffer);
                string salt = BitConverter.ToString(buffer);
                newUser.User_Salt = salt;

                string password = salt + dtoNewUser.User_Password;
                byte[] hash = Encoding.UTF8.GetBytes(password);
                var sha512 = SHA512.Create();
                byte[] hashBytes = sha512.ComputeHash(hash);
                newUser.User_Password = Convert.ToBase64String(hashBytes);

                bool registerSuccesful = await _userRepository.RegisterUser(newUser);

                //SEND MAIL VERIFICATION CODE V
                // ¯\_(ツ)_/¯

                return registerSuccesful;
            }
        }
        public async Task<LoginUserResultDTO> UserLogin(LoginUserDTO dtoUser)
        {
            User userAttempt = _mapper.Map<User>(dtoUser);
            userAttempt.User_Salt = await _userRepository.GetSalt(dtoUser.User_Mail);
            if (userAttempt.User_Salt == null)
            {
                return null;
            }
            else
            {
                string passwordWithSalt = userAttempt.User_Salt + userAttempt.User_Password;
                byte[] hash = Encoding.UTF8.GetBytes(passwordWithSalt);
                var sha512 = SHA512.Create();
                byte[] hashBytes = sha512.ComputeHash(hash);
                userAttempt.User_Password = Convert.ToBase64String(hashBytes);

                User result = await _userRepository.Login(userAttempt);
                LoginUserResultDTO dtoResult = _mapper.Map<LoginUserResultDTO>(result);
                return dtoResult;
            }
        }
        public async Task<bool> DeleteUser(int id)
        {
            bool isUserDeleted = await _userRepository.DeleteUser(id);
            return isUserDeleted;
        }
        public async Task<PasswordResetReturnDTO> PasswordReset(PasswordResetDTO dtoMail)
        {
            User dbUser;
            dbUser = await _userRepository.GetUserByMail(dtoMail.User_Mail);
            PasswordResetReturnDTO dtoUser;
            dtoUser = _mapper.Map<PasswordResetReturnDTO>(dbUser);
            if (dbUser != null)
            {
                if (dbUser.User_Verified != true)
                {
                    //RE-SEND CONFIRMATION MAIL JUST TO MAKE SURE
                    return dtoUser;
                }
                else
                {
                    //RECOVERY MAIL LOGIC TO GO HERE
                    return dtoUser;
                }
                
            }
            else
            {
                return null;
            }
        }
    }
}
