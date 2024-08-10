using Microsoft.AspNetCore.Mvc;
using Notes_API_SERVICE.DTOs;

namespace Notes_API_SERVICE.Interfaces
{
    public interface INoteService
    {
        public Task<IEnumerable<ShowNoteDTO>> SearchAndPaginate(int page, int pageSize, int userId, string content);
        public Task<ShowNoteDTO> PostNote(NewNoteDTO newNoteDto, int user_Id);
        public Task<ShowNoteDTO> SearchNoteId(int noteId, int userId);
        public Task<ShowNoteDTO> EditNote(ShowNoteDTO noteEditDto);
        public Task<bool> DeleteNote(int noteDeleteId, int userId);
    }
}
