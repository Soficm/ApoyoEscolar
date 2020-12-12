using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Apoyo.Server;
using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace Apoyo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActividadesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActividadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Actividades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actividades>>> GetdbActividades()
        {
            return await _context.dbActividades.ToListAsync();
        }

        // GET: api/Actividades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actividades>> GetActividades(int id)
        {
            var actividades = await _context.dbActividades.FindAsync(id);

            if (actividades == null)
            {
                return NotFound();
            }

            return actividades;
        }
        [HttpGet("getActividadesGrupo")]
        public async Task<ActionResult<List<Actividades>>> getActividadesGrupo(int idGrupo)
        {
            var actividades = await _context.dbActividades.Include(m=>m.IdMateria).Where(m => m.IdGrupo.Id == idGrupo).ToListAsync();
            if (actividades == null)
            {
                return NotFound();
            }
            else 
            {
                return Ok(actividades);
            }

        }

        // PUT: api/Actividades/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("editar")]
        public async Task<IActionResult> PutActividades(Actividades actividades)
        {
            actividades.IdGrupo = _context.dbGrupo.Where(m => m.Id == Convert.ToInt32(actividades.IdGrupoString)).FirstOrDefault();
            actividades.IdMateria = _context.dbMaterias.Where(m => m.Id == Convert.ToInt32(actividades.IdMateriaString)).FirstOrDefault();
            _context.Attach(actividades);
            _context.Entry(actividades).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
            }
            return Ok(actividades);
        }

        // POST: api/Actividades
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("crear")]
        public async Task<ActionResult<Actividades>> PostActividades(Actividades actividades)
        {
            try
            {
                actividades.IdGrupo = _context.dbGrupo.Where(m => m.Id == Convert.ToInt32(actividades.IdGrupoString)).FirstOrDefault();
                actividades.IdMateria = _context.dbMaterias.Where(m => m.Id == Convert.ToInt32(actividades.IdMateriaString)).FirstOrDefault();
                _context.dbActividades.Add(actividades);
                await _context.SaveChangesAsync();

                return Ok(actividades);
                
            }
            catch (Exception e)
            {
                return BadRequest("error " + e.Message);
            }
            
        }

        // DELETE: api/Actividades/5
        [HttpDelete("borrar")]
        public async Task<ActionResult<Actividades>> DeleteActividades(Guid id)
        {
            var actividades = await _context.dbActividades.FindAsync(id);
            if (actividades == null)
            {
                return NotFound();
            }

            _context.dbActividades.Remove(actividades);
            await _context.SaveChangesAsync();

            return actividades;
        }

        
        private bool ActividadesExists(Guid id)
        {
            return _context.dbActividades.Any(e => e.Id == id);
        }
        
    }
}
