using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public UsersController(LibraryContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Afficher les utilisateurs
        /// </summary>
        /// <response code="200">Les utilisateurs ont bien été affichés.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>
        // GET: api/Users

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<Users> GetUsers()
        {
            return _context.Users;
        }

        // GET: api/Users/5
        /// <summary>
        /// Afficher un utilisateur
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">L'utilisateur ont bien été affiché.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/Users/5
        /// <summary>
        /// Modifier un utilisateur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="users"></param>
        /// <response code="201">L'utilisateur a bien été modifié.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <response code="404">Aucune données trouvées.</response>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]

        public async Task<IActionResult> PutUsers([FromRoute] int id, [FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        /// <summary>
        /// Ajouter un utilisateur
        /// </summary>
        /// <param name="users"></param>
        /// <response code="201">L'utilisateur à bien été ajouté.</response>
        /// <response code="400">Les données étaient incorrectes.</response>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Supprimer un utilisateur
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">L'utilisateur à bien été supprimé</response>
        /// <response code="400">Les données étaient incorrectes</response>
        /// <response code="404">Aucun utilisateur trouvés</response>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return Ok(users);
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}