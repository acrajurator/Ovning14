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
    public class CoursesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;
        public CoursesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourse(bool includeModules = false)
        {
            if (includeModules)
            {
                var courseDto = mapper.ProjectTo<CourseWithModuleDto>(_context.Course.Include(m => m.Modules));
                return Ok(await courseDto.ToListAsync());

            }
            else
            {
                var courseDto = mapper.ProjectTo<CourseDto>(_context.Course.Include(m => m.Modules));

                return Ok(await courseDto.ToListAsync());
            }
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            var courseDto = mapper.Map<CourseDto>(course);


            return Ok(courseDto);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return (CreatedAtAction("GetCourse", new { id = course.Id }, course));
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

           // var dto = new CourseDto();
            //patchDocument.ApplyTo(dto);
            var entity = await _context.Course.FirstOrDefaultAsync(c => c.Id == courseId);

                if (entity == null)
                {
                    return NotFound();
                }

                var patchEntity = mapper.Map<JsonPatchDocument<Course>>(patchDocument);
                patchEntity.ApplyTo(entity, ModelState);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500);
            }

                return Ok(entity);
            



        }
        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
