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
using KelpieServer.Mappers;

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
            List<DatapointResponseDto> responseList = new List<DatapointResponseDto>();
            var datapointList = await _context.Datapoints.ToListAsync();
            foreach (var datapoint in datapointList)
            {
                responseList.Add(new DatapointResponseDto
                {
                    Id = datapoint.Id,
                    ProjectId = datapoint.ProjectId,
                    Name = datapoint.Name,
                });
            }

            return Ok(responseList);
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

            var datapointResponse = new DatapointResponseDto
            {
                Id = datapoint.Id,
                ProjectId = datapoint.ProjectId,
                Name = datapoint.Name
            };

            return Ok(datapointResponse);
        }

        // PUT: api/Datapoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDatapoint(string id, DatapointDto datapointDto)
        {
            if (id != datapointDto.Id)
            {
                return BadRequest();
            }

            //_context.Entry(datapoint).State = EntityState.Modified;

            try
            {
                var targetDatapoint = await _context.Datapoints.FindAsync(id);

                if (targetDatapoint == null)
                {
                    return NotFound();
                }

                var datapointMapper = new DatapointMapper();
                // Can't create a new Datapoint object here -- would detach targetDatapoint from EF's tracking
                datapointMapper.MapToEntity(datapointDto, ref targetDatapoint);
                _context.Entry(targetDatapoint).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new DatapointResponseDto
                {
                    Id = datapointDto.Id,
                    ProjectId = datapointDto.ProjectId,
                    Name = datapointDto.Name,
                });
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
        }

        // POST: api/Datapoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Datapoint>> PostDatapoint(DatapointDto datapointDto)
        {
            if (_context.Datapoints == null)
            {
                return Problem("Entity set 'EF_DataContext.Datapoints' is null.");
            }
            
            var datapointMapper = new DatapointMapper();
            var datapoint = datapointMapper.MapToEntity(datapointDto);
            //_context.Datapoints.Add(new Datapoint(datapoint));
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
