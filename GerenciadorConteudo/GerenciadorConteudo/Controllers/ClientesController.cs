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
    public class ClientesController : Controller
    {
        public readonly DatabaseContext _context;
        public ClientesController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetCliente()
        {
            return await _context.Clientes.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Clientes>> GetCliente(int id)
        {
            var clientDetail = await _context.Clientes.FindAsync(id);

            if (clientDetail == null)
            {
                return NotFound();
            }

            return clientDetail;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Clientes clientDetail)
        {
            if (id != clientDetail.ClienteId)
            {
                return BadRequest();
            }

            _context.Entry(clientDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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
        public async Task<ActionResult<Clientes>> PostCliente(Clientes clientDetail)
        {
            _context.Clientes.Add(clientDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = clientDetail.ClienteId }, clientDetail);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var clienteDetail = await _context.Clientes.FindAsync(id);
            if (clienteDetail == null)
            {
                return NotFound();
            }
            _context.Clientes.Remove(clienteDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }        
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
