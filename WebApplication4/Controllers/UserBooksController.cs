using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public UserBooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/UserBooks
        /// <summary>
        ///  afficher tout les empreints 
        /// </summary>
        /// <response code="200">Les empreints ont bien été affichés.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>

        [HttpGet]
        public IEnumerable<UserBooks> GetUserBooks()
        {
            return _context.UserBooks;
        }

        // GET: api/UserBooks/5
        /// <summary>
        /// afficher un empreint
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">L'empreint ont bien été affichés.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>
        [Authorize(Roles = "Admin, User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserBooks([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userBooks = await _context.UserBooks.FindAsync(id);

            if (userBooks == null)
            {
                return NotFound();
            }

            return Ok(userBooks);
        }

        // PUT: api/UserBooks/5
        /// <summary>
        /// MOdifier un empreint 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userBooks"></param>
        /// <response code="200">Les empreints ont bien été affichés.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>
        [Authorize(Roles = "Admin, User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBooks([FromRoute] int id, [FromBody] UserBooks userBooks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userBooks.Id)
            {
                return BadRequest();
            }

            _context.Entry(userBooks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserBooks
        /// <summary>
        /// Ajouter un empreint
        /// </summary>
        /// <param name="userBooks"></param>
        /// <response code="201">L'empreint à bien été ajouté.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> PostUserBooks([FromBody] UserBooks userBooks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserBooks.Add(userBooks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserBooks", new { id = userBooks.Id }, userBooks);
        }

        // DELETE: api/UserBooks/5
        /// <summary>
        /// Suprimer un empreint 
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">L'utilisateur à bien été supprimé</response>
        /// <response code="400">Les données étaient incorrectes</response>
        /// <response code="404">Aucun utilisateur trouvés</response>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, User")]

        public async Task<IActionResult> DeleteUserBooks([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userBooks = await _context.UserBooks.FindAsync(id);
            if (userBooks == null)
            {
                return NotFound();
            }

            _context.UserBooks.Remove(userBooks);
            await _context.SaveChangesAsync();

            return Ok(userBooks);
        }

        private bool UserBooksExists(int id)
        {
            return _context.UserBooks.Any(e => e.Id == id);
        }
    }
}