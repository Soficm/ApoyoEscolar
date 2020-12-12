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
using Apoyo.Server.Helpers;

namespace Apoyo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GrupoAlumnosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GrupoAlumnosController(ApplicationDbContext context)
        {
            _context = context;
        }

                // GET: api/GrupoAlumnos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoAlumnos>> GetGrupoAlumnos(int id)
        {
            var grupoAlumnos = await _context.dbGrupoAlumnos.FindAsync(id);

            if (grupoAlumnos == null)
            {
                return NotFound();
            }

            return grupoAlumnos;
        }

        // PUT: api/GrupoAlumnos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupoAlumnos(GrupoAlumnos grupoAlumnos)
        {
            _context.Entry(grupoAlumnos).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch 
            {
                
            }
            return Ok(grupoAlumnos);
        }

        // POST: api/GrupoAlumnos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GrupoAlumnos>> PostGrupoAlumnos(GrupoAlumnos grupoAlumnos)
        {
            var verifica = _context.dbGrupoAlumnos.Where(m => m.IdAlumno.Id == grupoAlumnos.IdAlumno.Id && m.IdGrupo.Id == grupoAlumnos.IdGrupo.Id).FirstOrDefault();
            if (verifica == null)
            {
                try
                {
                    grupoAlumnos.IdAlumno = _context.dbDatosGenerales.Where(m => m.Id == grupoAlumnos.IdAlumno.Id).FirstOrDefault();
                    grupoAlumnos.IdGrupo = _context.dbGrupo.Where(m => m.Id == grupoAlumnos.IdGrupo.Id).FirstOrDefault();
                    _context.dbGrupoAlumnos.Attach(grupoAlumnos);
                    _context.dbGrupoAlumnos.Add(grupoAlumnos);
                    await _context.SaveChangesAsync();
                    //await Mails.sendMensaje("Bienvenido al grupo " + grupoAlumnos.IdGrupo.Descripcion, grupoAlumnos.IdAlumno.Email, "<p><h1>Has sido inscrito al grupo " + grupoAlumnos.IdGrupo.Descripcion + "</h1></p>");
                    return CreatedAtAction("GetGrupoAlumnos", new { id = grupoAlumnos.Id }, grupoAlumnos);
                }
                catch(Exception e)
                {
                    return BadRequest("Error, "+ e.Message);
                }
            }
            else
            {
                return BadRequest("Error, el alumo ya existe en el grupo");
            }
        }

        // DELETE: api/GrupoAlumnos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GrupoAlumnos>> DeleteGrupoAlumnos(int id)
        {
            var grupoAlumnos = await _context.dbGrupoAlumnos.FindAsync(id);
            if (grupoAlumnos == null)
            {
                return NotFound();
            }

            _context.dbGrupoAlumnos.Remove(grupoAlumnos);
            await _context.SaveChangesAsync();

            return grupoAlumnos;
        }
        [HttpDelete]
        public async Task<ActionResult<GrupoAlumnos>> DeleteGrupoAlumnos(int idGrupo, string idUsuario)
        {
            var id = _context.dbGrupoAlumnos.Where(m => m.IdAlumno.Id == idUsuario && m.IdGrupo.Id == idGrupo).FirstOrDefault();
            if (id != null)
            {
                _context.dbGrupoAlumnos.Remove(id);
                await _context.SaveChangesAsync();
                return id;
            }
            else
            {
                return BadRequest("Error al eliminar");
            }
        }

        private bool GrupoAlumnosExists(int id)
        {
            return _context.dbGrupoAlumnos.Any(e => e.Id == id);
        }
    }
}
