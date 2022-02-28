#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;

        public ModulesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModule(string search = "")
        {
            var moduleDto = mapper.ProjectTo<ModuleDto>(_context.Module.Where(m => m.Title.Contains(search)));
            
            //if (moduleDto.Count() == 0)
              //  return NotFound();

            return Ok(await moduleDto.ToListAsync());
        }
        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDto>> GetModule(int id)
        {
            var module = await _context.Module.FindAsync(id);

            if (module == null)
            {
                return NotFound();
            }
            var moduleDto = mapper.Map<ModuleDto>(module);

            return Ok(moduleDto);
        }


        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, Module @module)
        {
            if (id != @module.Id)
            {
                return BadRequest();
            }

            _context.Entry(@module).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500); 
                }
            }

            return NoContent();
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(Module @module)
        {
            _context.Module.Add(@module);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModule", new { id = @module.Id }, @module);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            _context.Module.Remove(@module);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{moduleId}")]
        public async Task<ActionResult<CourseDto>> PatchModule(int moduleId, JsonPatchDocument<ModuleDto> patchDocument)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var dto = new ModuleDto();
            patchDocument.ApplyTo(dto);
            var entity = await _context.Module.FirstOrDefaultAsync(c => c.Id == moduleId);

            if (entity == null)
            {
                return NotFound();
            }

            var patchEntity = mapper.Map<JsonPatchDocument<Module>>(patchDocument);
            patchEntity.ApplyTo(entity, ModelState);
            await _context.SaveChangesAsync();

            return Ok(entity);




        }
        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
