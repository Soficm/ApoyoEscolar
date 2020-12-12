using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apoyo.Server
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
       
        public DbSet <ApplicationUser> dbDatosGenerales { get; set; }
        public DbSet<Grupo> dbGrupo { get; set; }
        public DbSet<Materias> dbMaterias { get; set; }
        public DbSet <Actividades> dbActividades { get; set; }
        //public DbSet<ActividadesEntregadas> dbActividadesEntregadas { get; set; }
        public DbSet<Calificaciones> dbCalificaciones { get; set; }
        public DbSet <GrupoAlumnos> dbGrupoAlumnos { get; set; }

    }
}
