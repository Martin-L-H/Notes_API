using Notes_API_SERVICE.DTOs;

namespace Notes_API_SERVICE.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<CompleteUserDTO>> GetAllUsers();
        public Task<CompleteUserDTO> GetUserById(int id);
        public Task<bool> CreateUser(CreateUserDTO dtoNewUser);
        public Task<LoginUserResultDTO> UserLogin(LoginUserDTO dtoUser);
        public Task<bool> DeleteUser(int id);
        public Task<PasswordResetReturnDTO> PasswordReset(PasswordResetDTO dtoMail);
    }
}
