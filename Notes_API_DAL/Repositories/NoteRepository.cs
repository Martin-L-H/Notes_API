using Microsoft.EntityFrameworkCore;
using Notes_API_CORE.Entities;
using Notes_API_DAL.Context;
using Notes_API_DAL.Interfaces;

namespace Notes_API_DAL.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;
        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserNote>> SearchAndPaginate(int page, int pageSize, int userId, string content)
        {
            IQueryable<UserNote> query;
            query = _context.UsersNotes.Where(p => p.User_Id == userId);
            query = _context.UsersNotes.Where(p => p.Note_Content.Contains(content));
            int recordsToSkip = (page - 1) * pageSize;
            IQueryable<UserNote> queryPaged;
            queryPaged = query.Skip(recordsToSkip).Take(pageSize);
            IEnumerable<UserNote> list = await queryPaged.AsNoTracking().ToListAsync();
            return list;
        }
        public async Task<int> NoteAmount()
        {
            return await _context.Notes.AsNoTracking().CountAsync();
        }
        public async Task<int> GetAllNotesByUser(int userId)
        {
            return await _context.UsersNotes.Where(p=>p.User_Id==userId).AsNoTracking().CountAsync();
        }
        public async Task<int> AddNoteEntry()
        {
            Note newEntry = new Note();
            await _context.Notes.AddAsync(newEntry);
            try
            {
                await _context.SaveChangesAsync();
                return await _context.Notes.CountAsync();
            }
            catch
            {
                return await _context.Notes.CountAsync();
            }
        }
        public async Task<UserNote> PostNote(UserNote newNote)
        {
            await _context.UsersNotes.AddAsync(newNote);
            try
            {
                await _context.SaveChangesAsync();
                return newNote;
            }
            catch
            {
                return null;
            }
        }
        public async Task<UserNote> SearchNoteId(int noteId, int userId)
        {
            UserNote query;
            query = await _context.UsersNotes.Where(p => p.User_Id == userId).Where(p => p.Note_Id == noteId).AsNoTracking().FirstOrDefaultAsync();
            return query;
        }
        public async Task<UserNote> EditNote(UserNote noteEdit)
        {
            UserNote noteOld = await _context.UsersNotes.Where(p => p.User_Id == noteEdit.User_Id).Where(p => p.Note_Id == noteEdit.Note_Id).FirstOrDefaultAsync();
            noteOld.Note_Content = noteEdit.Note_Content;
            try
            {
                await _context.SaveChangesAsync();
                return noteOld;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> DeleteNote(UserNote noteToDelete)
        {
            UserNote noteFound = await _context.UsersNotes.Where(p => p.User_Id == noteToDelete.User_Id).Where(p => p.Note_Id == noteToDelete.Note_Id).FirstOrDefaultAsync();
            try
            {
                _context.Remove(noteFound);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
