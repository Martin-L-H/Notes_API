namespace Notes_API_SERVICE.DTOs
{
    public class PasswordResetReturnDTO
    {
        public string User_Mail { get; set; }
        public bool User_Disabled { get; set; }
        public bool User_Verified {  get; set; }
    }
}
