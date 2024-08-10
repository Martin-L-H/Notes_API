namespace Notes_API_CORE.Entities
{
    public class Note
    {
        public int Note_Id { get; set; }
        public IEnumerable<UserNote> _usersnotes { get; set; }
    }
}
