using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apoyo.Services
{
    public class ActividadesService
    {
        [Inject]
        private HttpClient http { get; set; }
        public ActividadesService(HttpClient http)
        {
            this.http = http;
        }
        public async Task<List<SchedulerAppointment>> getActividadesGrupo(int IdGrupo)
        {
            List<Actividades> lista = new List<Actividades>();
            lista = await this.http.GetJsonAsync<List<Actividades>>("api/Actividades/getActividadesGrupo?idGrupo=" + IdGrupo);
            List<SchedulerAppointment> actividades = new List<SchedulerAppointment>();
            foreach (var item in lista)
            {
                SchedulerAppointment x = new SchedulerAppointment { Description = item.Descripcion, End = item.Final, Id = item.Id, IsAllDay = item.IsAllDay, RecurrenceExceptions = item.RecurrenceExceptions, RecurrenceId = item.RecurrenceId, RecurrenceRule = item.RecurrenceRule, Start = item.Inicio, IdMateria = item.IdMateria.Id.ToString(), Title = item.Actividad };
                actividades.Add(x);
            }
            return actividades;
        }

        public async Task<SchedulerAppointment> guardaActividad(SchedulerAppointment tarea)
        {
            Actividades x = new Actividades();
            x.Actividad = tarea.Title;
            x.Descripcion = tarea.Description;
            x.Final = tarea.End;
            x.IdGrupoString = tarea.IdGrupo.ToString();
            x.IdMateriaString = tarea.IdMateria;
            x.Inicio = tarea.Start;
            x.IsAllDay = tarea.IsAllDay;
            x.RecurrenceExceptions = tarea.RecurrenceExceptions;
            x.RecurrenceId = tarea.RecurrenceId;
            x.RecurrenceRule = tarea.RecurrenceRule;
            x = await this.http.PostJsonAsync<Actividades>("api/Actividades/crear", x);

            tarea.Id = x.Id;
            return tarea;
        }

        public async Task<SchedulerAppointment> editarActividad(SchedulerAppointment tarea)
        {
            Actividades x = new Actividades { Actividad = tarea.Title, Descripcion = tarea.Description, Final = tarea.End, Inicio = tarea.Start, IdGrupoString = tarea.IdGrupo.ToString(), IdMateriaString = tarea.IdMateria, IsAllDay = tarea.IsAllDay, RecurrenceId = tarea.RecurrenceId, RecurrenceRule = tarea.RecurrenceRule, RecurrenceExceptions = tarea.RecurrenceExceptions, Id = tarea.Id };
            await http.PutJsonAsync<Actividades>("api/Actividades/editar", x);
            return tarea;
        }
        public async void eliminarActividad(SchedulerAppointment tarea)
        {
            await http.DeleteAsync("api/Actividades/borrar?id=" + tarea.Id);
        }
    }
}
