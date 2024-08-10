namespace Notes_API_CORE.Entities
{
    public class UserNote
    {
        public int Note_Id { get; set; }
        public int User_Id { get; set; }
        public string Note_Content { get; set; }
        public Note _note { get; set; }
        public User _user { get; set; }
    }
}
