using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;

namespace Apoyo.Shared.Models
{
    class Modelos
    {
    }
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public TipoUsuario Tipo { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
    }
    public class UserInfo
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [DefaultValue(TipoUsuario.Alumno)]
        public TipoUsuario Tipo { get; set; }
    }
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }


    public class Grupo
    {
        public int Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public ApplicationUser Docentes { get; set; }
        [DefaultValue(true)]
        public bool Activo { get; set; }
        [Required]
        [StringLength(9, ErrorMessage = "La cadena debe contener el formato XXXX-XXXX", MinimumLength = 9)]
        public string CicloEscolar { get; set; }
    }

    public class GrupoAlumnos
    {
        public int Id { get; set; }
        public Grupo IdGrupo { get; set; }
        public ApplicationUser IdAlumno { get; set; }
    }


    public class Materias
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]


        public int Creditos { get; set; }
        [Required]
        public ApplicationUser Docente { get; set; }
        [ScaffoldColumn(false)]
        public string IdString
        {
            get
            {
                return this.Id.ToString();
            }
        }
        public string Color { get; set; }
    }


    public class Calificaciones {
        public int Id { get; set; }
        public GrupoAlumnos IdGrupoAlumno { get; set; }
        public Materias IdMateria { get; set; }
        [DefaultValue(5)]
        [Range(5,10)]
        public int Calificacion { get; set; }
    }
    
    public class Actividades
    {
        [Key]
        public Guid Id { get; set; }
        public Grupo IdGrupo { get; set; }
        [NotMapped]
        public string IdGrupoString { get; set; }
        public Materias IdMateria { get; set; }
        [NotMapped]
        public string IdMateriaString { get; set; }

        public string Actividad { get; set; }
        public string Descripcion { get; set; }
        [Required]
        public DateTime Inicio { get; set; }
        [Required]
        public DateTime Final { get; set; }
        [DefaultValue(0.1)]
        [Required]
        public float Ponderacion { get; set; }
        public bool IsAllDay { get; set; }
        public string RecurrenceRule { get; set; }
        [NotMapped]
        public List<DateTime> RecurrenceExceptions { get; set; }
        public Guid? RecurrenceId { get; set; }
        public Actividades()
        {
            Id = Guid.NewGuid();
        }
    }


    public class ActividadesEntregadas
    {
        public int Id { get; set; }
        public Actividades IdActividad { get; set; }
        public ApplicationUser IdAlumno { get; set; }
        [Required]
        public DateTime FechaEntrega { get; set; }
        [DefaultValue(false)]
        public bool Calificado { get; set; }
        [DefaultValue(0)]
        public float Calificacion { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }
    }

    public enum TipoUsuario
    {
        Alumno,
        Docente
    }


    public class vistaInvitacionAlumno
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage ="La longitud debe de ser de al menos 3", MinimumLength =4)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "La longitud debe de ser de al menos 3", MinimumLength = 4)]
        public string Apellidos { get; set; }
    }

    public class SchedulerAppointment
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsAllDay { get; set; }
        public string RecurrenceRule { get; set; }
        public List<DateTime> RecurrenceExceptions { get; set; }
        public Guid? RecurrenceId { get; set; }
        [Required]
        public string IdMateria { get; set; }
        public int IdGrupo { get; set; }

        public SchedulerAppointment()
        {
            Id = Guid.NewGuid();
        }
    }
}
