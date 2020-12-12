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
using Microsoft.AspNetCore.Identity;

namespace Apoyo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MateriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _usermanager;
        public MateriasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _usermanager = userManager;
            _context = context;
        }

        // GET: api/Materias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materias>>> GetdbMaterias()
        {
            return await _context.dbMaterias.ToListAsync();
        }

        [HttpGet("getMateriasDocente")]
        [Authorize(Roles ="Docente, Administrador")]
        public async Task<ActionResult<IEnumerable<Materias>>> getMateriasDocente(string userName)
        {
            return await _context.dbMaterias.Where(m => m.Docente.UserName == userName).ToListAsync();
        }

        [HttpGet("getMateriasByGrupo")]
        public async Task<ActionResult<IEnumerable<Materias>>> getMateriasByGrupo(int idGrupo)
        {
            return await _context.dbActividades.Where(m => m.IdGrupo.Id == idGrupo).Select(m => m.IdMateria).Distinct().ToListAsync();
        }

        // GET: api/Materias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materias>> GetMaterias(int id)
        {
            var materias = await _context.dbMaterias.FindAsync(id);

            if (materias == null)
            {
                return NotFound();
            }

            return materias;
        }

        // PUT: api/Materias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Authorize(Roles = "Docente, Administrador")]
        public async Task<ActionResult<Materias>> PutMaterias(Materias materias)
        {
            materias.Docente= _context.dbDatosGenerales.Where(m => m.UserName == materias.Docente.UserName).FirstOrDefault();
            _context.Entry(materias).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Error"+ e.Message);
            }
            return Ok(materias);
        }

        // POST: api/Materias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Materias>> PostMaterias(Materias materias)
        {
            try
            {
                var random = new Random();
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                materias.Color = color;
                materias.Docente = _context.dbDatosGenerales.Where(m => m.UserName == materias.Docente.UserName).FirstOrDefault();
                _context.dbMaterias.Add(materias);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetMaterias", new { id = materias.Id }, materias);
            }
            catch(Exception e)
            {
                return BadRequest("Error al crear la materia"+ e.Message);
            }
                    
            
        }

        // DELETE: api/Materias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Materias>> DeleteMaterias(int id)
        {
            var materias = await _context.dbMaterias.FindAsync(id);
            if (materias == null)
            {
                return NotFound();
            }

            _context.dbMaterias.Remove(materias);
            await _context.SaveChangesAsync();

            return materias;
        }

        private bool MateriasExists(int id)
        {
            return _context.dbMaterias.Any(e => e.Id == id);
        }
    }
}
