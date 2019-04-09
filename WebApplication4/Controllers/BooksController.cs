using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Books
        /// <summary>
        /// Afficher les livres
        /// </summary>
        /// <response code="200">Les livres on bien été affichés</response>
        /// <response code="400">Les données étaient incorrectes</response>
        /// <response code="404">Aucune données trouvées</response>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Books> GetBooks()
        {
            return _context.Books;
        }

        // GET: api/Books/5
        /// <summary>
        /// Afficher un livre
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Le livre à bien été affiché.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooks([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var books = await _context.Books.FindAsync(id);

            if (books == null)
            {
                return NotFound();
            }

            return Ok(books);
        }

        // PUT: api/Books/5
        /// <summary>
        /// Modifier un livre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="books"></param>
        /// <response code="201">Le livre a bien été modifié.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooks([FromRoute] int id, [FromBody] Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != books.Id)
            {
                return BadRequest();
            }

            _context.Entry(books).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
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

        // POST: api/Books
        /// <summary>
        /// AJouter un livre
        /// </summary>
        /// <param name="books"></param>
        /// <response code="201">Le livre à bien été ajouté.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostBooks([FromBody] Books books)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Books.Add(books);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooks", new { id = books.Id }, books);
        }

        // DELETE: api/Books/5
        /// <summary>
        /// Supprimer un utilisateur
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Le livre à bien été supprimé</response>
        /// <response code="400">Les données étaient incorrectes</response>
        /// <response code="404">Aucun livre trouvés</response>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooks([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            _context.Books.Remove(books);
            await _context.SaveChangesAsync();

            return Ok(books);
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}