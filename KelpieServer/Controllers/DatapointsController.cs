using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KelpieServer;
using KelpieServer.Models;

// Will need to validate json data from front end
namespace KelpieServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatapointsController : ControllerBase
    {
        private readonly EF_DataContext _context;

        public DatapointsController(EF_DataContext context)
        {
            _context = context;
        }

        // GET: api/Datapoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Datapoint>>> GetDatapoints()
        {
            Debug.Print("Getting datapoints");
            if (_context.Datapoints == null)
            {
                return NotFound();
            }
            return await _context.Datapoints.ToListAsync();
        }

        // GET: api/Datapoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Datapoint>> GetDatapoint(string id)
        {
          if (_context.Datapoints == null)
          {
              return NotFound();
          }
            var datapoint = await _context.Datapoints.FindAsync(id);

            if (datapoint == null)
            {
                return NotFound();
            }

            return datapoint;
        }

        // PUT: api/Datapoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDatapoint(string id, Datapoint datapoint)
        {
            if (id != datapoint.Id)
            {
                return BadRequest();
            }

            _context.Entry(datapoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatapointExists(id))
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

        // POST: api/Datapoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Datapoint>> PostDatapoint(Datapoint datapoint)
        {
            if (_context.Datapoints == null)
            {
                return Problem("Entity set 'EF_DataContext.Datapoints'  is null.");
            }
            _context.Datapoints.Add(datapoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDatapoint", new { id = datapoint.Id }, datapoint);
        }

        // DELETE: api/Datapoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDatapoint(string id)
        {
            if (_context.Datapoints == null)
            {
                return NotFound();
            }
            var datapoint = await _context.Datapoints.FindAsync(id);
            if (datapoint == null)
            {
                return NotFound();
            }

            _context.Datapoints.Remove(datapoint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DatapointExists(string id)
        {
            return (_context.Datapoints?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
