namespace Notes_API_CORE.Entities
{
    public class User
    {
        public int User_Id { get; set; }
        public bool User_Disabled { get; set; }
        public bool User_Verified { get; set; }
        public string User_Mail { get; set; }
        public string User_Salt { get; set; }
        public string User_Password { get; set; }
        public IEnumerable<UserNote> _usersnotes { get; set; }
    }
}
