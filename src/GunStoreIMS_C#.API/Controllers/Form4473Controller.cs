using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GunStoreIMS.Persistence.Data;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Form4473Controller : ControllerBase
    {
        private readonly FirearmsInventoryDB _context;

        public Form4473Controller(FirearmsInventoryDB context)
        {
            _context = context;
        }

        // GET: api/Form4473
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Form4473Record>>> GetAll()
        {
            return await _context.Form4473Records.ToListAsync();
        }

        // GET: api/Form4473/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Form4473Record>> GetById(Guid id)
        {
            var record = await _context.Form4473Records.FindAsync(id);

            if (record == null)
                return NotFound();

            return record;
        }

        // POST: api/Form4473
        [HttpPost]
        public async Task<ActionResult<Form4473Record>> Create(Form4473Record form4473Record)
        {
            form4473Record.Id = Guid.NewGuid();
            _context.Form4473Records.Add(form4473Record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = form4473Record.Id }, form4473Record);
        }

        // PUT: api/Form4473/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Form4473Record form4473Record)
        {
            if (id != form4473Record.Id)
                return BadRequest();

            _context.Entry(form4473Record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Form4473Records.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/Form4473/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var record = await _context.Form4473Records.FindAsync(id);
            if (record == null)
                return NotFound();

            _context.Form4473Records.Remove(record);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
