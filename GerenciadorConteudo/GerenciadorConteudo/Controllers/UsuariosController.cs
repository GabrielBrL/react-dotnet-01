using GerenciadorConteudo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorConteudo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {
        public readonly DatabaseContext _context;
        public UsuariosController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuario()
        {
            return await _context.Usuarios.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuario(int id)
        {
            var userDetail = await _context.Usuarios.FindAsync(id);

            if (userDetail == null)
            {
                return NotFound();
            }

            return userDetail;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuarios userDetail)
        {
            if (id != userDetail.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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
        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUser(Usuarios userDetail)
        {                                   
            _context.Usuarios.Add(userDetail);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCliente", new { id = userDetail.UserId }, userDetail);                     
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userDetail = await _context.Usuarios.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(userDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }        
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UserId == id);
        }
    }
}
