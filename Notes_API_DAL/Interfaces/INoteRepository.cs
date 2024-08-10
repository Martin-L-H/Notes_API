using Notes_API_CORE.Entities;

namespace Notes_API_DAL.Interfaces
{
    public interface INoteRepository
    {
        public Task<IEnumerable<UserNote>> SearchAndPaginate(int page, int pageSize, int userId, string content);
        public Task<int> NoteAmount();
        public Task<int> GetAllNotesByUser(int userId);
        public Task<int> AddNoteEntry();
        public Task<UserNote> PostNote(UserNote newNote);
        public Task<UserNote> SearchNoteId(int noteId, int userId);
        public Task<UserNote> EditNote(UserNote noteEdit);
        public Task<bool> DeleteNote(UserNote noteDelete);
    }
}
