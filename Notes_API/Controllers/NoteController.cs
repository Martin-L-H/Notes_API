using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes_API_SERVICE.DTOs;
using Notes_API_SERVICE.Interfaces;
using System.Security.Claims;

namespace Notes_API_WEB.Controllers
{
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [Authorize]
        [HttpGet]
        [Route("SearchAndPaginate")]
        public async Task<IActionResult> SearchAndPaginate([FromQuery] int page = 1, [FromQuery] int pageSize = 5, string content = "")
        {
            IEnumerable<ShowNoteDTO> noteDtoList;
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null || userId < 1)
            {
                return BadRequest("User not found");
            }
            noteDtoList = await _noteService.SearchAndPaginate(page, pageSize, userId, content);
            if (noteDtoList != null)
            {
                return Ok(noteDtoList);
            }
            else
            {
                return BadRequest("User not found");
            }
        }
        [Authorize]
        [HttpPost]
        [Route("PostNote")]
        public async Task<IActionResult> PostNote([FromBody] NewNoteDTO newNoteDto)
        {
            int user_Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ShowNoteDTO noteUploaded;
            noteUploaded = await _noteService.PostNote(newNoteDto, user_Id);

            if (noteUploaded != null)
            {
                return Ok(noteUploaded);
            }
            else
            {
                return BadRequest("Error when adding note");
            }
        }
        [Authorize]
        [HttpGet]
        [Route("CheckNote")]
        public async Task<IActionResult> CheckNote(int noteId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null || userId < 1)
            {
                return BadRequest("User not found");
            }
            ShowNoteDTO noteDto = await _noteService.SearchNoteId(noteId, userId);
            if (noteDto != null)
            {
                return Ok(noteDto);
            }
            else
            {
                return BadRequest("Error Finding Note");
            }
        }
        [Authorize]
        [HttpPut]
        [Route("EditNote")]
        public async Task<IActionResult> EditNote([FromBody] ShowNoteDTO noteEditDto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null || userId < 1)
            {
                return BadRequest("User not found");
            }
            noteEditDto.User_Id = userId;
            ShowNoteDTO result = await _noteService.EditNote(noteEditDto);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Note couldn't be edited");
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("DeleteNote")]
        public async Task<IActionResult> DeleteNote([FromBody] int noteId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null || userId < 1)
            {
                return BadRequest("User not found");
            }
            if (await _noteService.SearchNoteId(noteId, userId) != null)
            {
                if (await _noteService.DeleteNote(noteId, userId) != false)
                {
                    return Ok("Note deleted");
                }
                else
                {
                    return BadRequest("Note was not deleted");
                }
            }
            else
            {
                return BadRequest("Note not found");
            }
        }
    }
}
