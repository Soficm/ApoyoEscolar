using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Apoyo.Server;
using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Apoyo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GrupoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GrupoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Grupoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetdbGrupo()
        {
            return await _context.dbGrupo.ToListAsync();
        }
        [Authorize(Roles = "Docente, Administrador")]
        [HttpGet("GetGruposByDocente")]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGruposByDocente(string userName)
        {

            var grupo = await _context.dbGrupo.Where(m=>m.Docentes.UserName== userName).ToListAsync();

            if (grupo == null)
            {
                return NotFound();
            }

            return grupo;
        }
        [Authorize(Roles = "Alumno, Administrador")]
        [HttpGet("GetGruposByAlumno")]
        public async Task<ActionResult<IEnumerable<Grupo>>> GetGruposByAlumno(string userName)
        {
            var grupo = await _context.dbGrupoAlumnos.Where(m => m.IdAlumno.UserName == userName).Select(m => m.IdGrupo).ToListAsync();
            if (grupo == null)
            {
                return NotFound();
            }
            return grupo;
        }

        [Authorize]
        [HttpGet("buscarAlumnos")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> buscarAlumnos(string correo)
        {
            var alumnos = await _context.dbDatosGenerales.Where(m => m.Email.Contains(correo) && m.Tipo==TipoUsuario.Alumno).ToListAsync();
            if (alumnos == null)
            {
                return NotFound();
            }
            return Ok(alumnos);
        }


        [Authorize]
        [HttpGet("GetAlumnosGrupo")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAlumnosGrupo(int IdGrupo)
        {
            var grupo = await _context.dbGrupoAlumnos.Include(m=>m.IdAlumno).Where(m => m.IdGrupo.Id == IdGrupo).Select(m=>m.IdAlumno).ToListAsync();
           
            if (grupo == null)
            {
                return NotFound();
            }
           
            return Ok(grupo);
        }
        // GET: api/Grupoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grupo>> GetGrupo(int id)
        {
            var grupo = await _context.dbGrupo.Include(m=>m.Docentes).Where(m=>m.Id==id).FirstOrDefaultAsync();

            if (grupo == null)
            {
                return NotFound();
            }

            return grupo;
        }

        // PUT: api/Grupoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Authorize(Roles = "Docente, Administrador")]
        public async Task<IActionResult> PutGrupo(Grupo grupo)
        {
            grupo.Docentes = _context.dbDatosGenerales.Where(m => m.UserName == grupo.Docentes.UserName).FirstOrDefault();
            _context.Entry(grupo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok(grupo);
        }

        // POST: api/Grupoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "Docente, Administrador")]
        public async Task<ActionResult<Grupo>> PostGrupo(Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                grupo.Docentes = _context.dbDatosGenerales.Where(m => m.UserName == grupo.Docentes.UserName).FirstOrDefault();
                _context.dbGrupo.Add(grupo);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetGrupo", new { id = grupo.Id }, grupo);
            }
            else
            {
                return BadRequest("Error");
            }
            
        }

        // DELETE: api/Grupoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Grupo>> DeleteGrupo(int id)
        {
            var grupo = await _context.dbGrupo.FindAsync(id);
            if (grupo == null)
            {
                return NotFound();
            }

            _context.dbGrupo.Remove(grupo);
            await _context.SaveChangesAsync();

            return grupo;
        }

        private bool GrupoExists(int id)
        {
            return _context.dbGrupo.Any(e => e.Id == id);
        }
    }
}
