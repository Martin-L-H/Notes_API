using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Notes_API_SERVICE;
using Notes_API_SERVICE.DTOs;
using Notes_API_SERVICE.Interfaces;
using System.Security.Claims;

namespace Notes_API_WEB.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenJwtHelper _token;
        public UserController(IUserService userService, IOptions<JwtSettings> token)
        {
            _userService = userService;
            _token = new TokenJwtHelper(token);
        }
        //DEBUGGER
        /*
        [Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            IEnumerable<CompleteUserDTO> dtoList = await _userService.GetAllUsers(page, pageSize);
            return Ok(dtoList);
        }
        */
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dtoNewUser)
        {
            bool isMailRegistered = await _userService.CreateUser(dtoNewUser);
            if (isMailRegistered != true)
            {
                return BadRequest("Mail in use");
            }
            else
            {
                return Ok("User created");
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dtoUser )
        {
            LoginUserResultDTO loginAttempt = await _userService.UserLogin(dtoUser);
            if (loginAttempt == null)
            {
                return NotFound("Invalid username or password");
            }
            else if (loginAttempt.User_Disabled == true)
            {
                return BadRequest("This user is suspended");
            }
            else if (loginAttempt.User_Verified == false)
            {
                //MAYBE RE-SEND MAIL VERIFICATION HERE OR THROUGH ANOTHER ENDPOINT
                return BadRequest("You must verify the confirmation mail. Please check your inbox");
            }
            else
            {
                string token = await _token.GenerateToken(loginAttempt);
                loginAttempt.User_Token = token;
                return Ok(loginAttempt);
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] LoginUserDTO dtoUser)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            LoginUserResultDTO loginAttempt = await _userService.UserLogin(dtoUser);
            if (loginAttempt == null || userId != loginAttempt.User_Id)
            {
                return BadRequest("Credential Mismatch");
            }
            bool isUserDeleted = await _userService.DeleteUser(userId);
            if (isUserDeleted != true)
            {
                return BadRequest("Failed to delete profile");
            }
            else
            {
                return Ok("User deleted");
            }
        }
        [HttpPost]
        [Route("PasswordReset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetDTO dtoMail)
        {
            PasswordResetReturnDTO dtoUser = await _userService.PasswordReset(dtoMail);
            if (dtoUser == null)
            {
                return NotFound("No account with this mail could be found");
            }
            if (dtoUser.User_Disabled == true)
            {
                return BadRequest("This user has been banned");
            }
            if (dtoUser.User_Verified == false)
            {
                return BadRequest("You must verify the confirmation mail. Please check your inbox");
            }
            else
            {
                return Ok("Recovery mail sent");
            }
        }
    }
}
