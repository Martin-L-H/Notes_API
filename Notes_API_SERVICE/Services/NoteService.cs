using AutoMapper;
using Notes_API_CORE.Entities;
using Notes_API_DAL.Interfaces;
using Notes_API_SERVICE.DTOs;
using Notes_API_SERVICE.Interfaces;

namespace Notes_API_SERVICE.Services
{
    public class NoteService : INoteService
    {
        private readonly IMapper _mapper;
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;
        public NoteService(IMapper mapper, INoteRepository noteRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<ShowNoteDTO>> SearchAndPaginate(int page, int pageSize, int userId, string content)
        {
            if (await _userRepository.GetUserById(userId) != null)
            {
                IEnumerable<UserNote> dbList;
                dbList = await _noteRepository.SearchAndPaginate(page, pageSize, userId, content);
                IEnumerable<ShowNoteDTO> dtoList = _mapper.Map<IEnumerable<ShowNoteDTO>>(dbList);
                return dtoList;
            }
            else
            {
                return null;
            }
        }
        public async Task<ShowNoteDTO> PostNote(NewNoteDTO newNoteDto, int user_Id)
        {
            UserNote newNote = new UserNote();
            newNote.User_Id = user_Id;
            newNote.Note_Content = newNoteDto.Note_Content;
            var user = await _userRepository.GetUserById(newNote.User_Id);
            if (user.User_Disabled == true)
            {
                return null;
            }

            int maxNotes = await _noteRepository.NoteAmount();
            int userNotes = await _noteRepository.GetAllNotesByUser(user_Id);
            if (userNotes == maxNotes)
            {
                int result = await _noteRepository.AddNoteEntry();
                if (result == maxNotes)
                {
                    return null;
                }
                else
                {
                    maxNotes++;
                }
            }
            newNote.Note_Id = (userNotes + 1);
            UserNote noteResult = await _noteRepository.PostNote(newNote);
            ShowNoteDTO dto = _mapper.Map<ShowNoteDTO>(noteResult);
            return dto;
        }
        public async Task<ShowNoteDTO> SearchNoteId(int noteId, int userId)
        {
            UserNote note = await _noteRepository.SearchNoteId(noteId, userId);
            ShowNoteDTO noteDto = _mapper.Map<ShowNoteDTO>(note);
            return noteDto;
        }
        public async Task<ShowNoteDTO> EditNote(ShowNoteDTO noteEditDto)
        {
            var user = await _userRepository.GetUserById(noteEditDto.User_Id);
            if (user.User_Disabled == true)
            {
                return null;
            }
            UserNote noteResult = await _noteRepository.SearchNoteId(noteEditDto.Note_Id, noteEditDto.User_Id);
            if (noteResult!=null)
            {
                UserNote noteEdit = _mapper.Map<UserNote>(noteEditDto);
                noteEdit = await _noteRepository.EditNote(noteEdit);
                ShowNoteDTO editedNoteDto = _mapper.Map<ShowNoteDTO>(noteEdit);
                return editedNoteDto;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> DeleteNote(int noteDeleteId, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user.User_Disabled == true)
            {
                return false;
            }
            UserNote noteToDelete = await _noteRepository.SearchNoteId(noteDeleteId, userId);
            if (noteToDelete != null)
            {
                return await _noteRepository.DeleteNote(noteToDelete);
            }
            else
            {
                return false;
            }
        }
    }
}
