using Apoyo.Client;
using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apoyo.Services
{
    public class GruposService
    {
        [Inject]
        private HttpClient http { get; set; }
        public GruposService(HttpClient http)
        {
            this.http = http;
        }
        public async Task<List<Grupo>> getGruposByDocente(string userName)
        {
            return await this.http.GetJsonAsync<List<Grupo>>("api/grupoes/GetGruposByDocente?userName=" + userName);
        }
        
        public async Task<List<Grupo>> getGruposByAlumno(string userName)
        {
            return await this.http.GetJsonAsync<List<Grupo>>("api/grupoes/GetGruposByAlumno?userName=" + userName);
        }
       
       
        public async Task<Grupo> CrearGrupo(Grupo grupo)
        {
            return await http.PostJsonAsync<Grupo>("api/grupoes", grupo);
        }
        public async Task<Grupo> EditarGrupo(Grupo grupo)
        {
            return await http.PutJsonAsync<Grupo>("api/grupoes", grupo);
        }
        public async Task BorrarGrupo(int id)
        {
            await http.DeleteAsync("api/grupoes/" + id);
        }
        public async Task<Grupo> getGrupo(int id)
        {
            return await http.GetJsonAsync<Grupo>("api/grupoes/"+id);
        }


        //manejo de alumnos en el grupo

        public Task<List<ApplicationUser>> getAlumnosGrupo(int idGrupo)
        {
            return this.http.GetJsonAsync<List<ApplicationUser>>("api/grupoes/GetAlumnosGrupo?IdGrupo=" + idGrupo);
        }
        public async Task<List<ApplicationUser>> buscarAlumnos(string correo)
        {
            return await this.http.GetJsonAsync<List<ApplicationUser>>("api/grupoes/buscarAlumnos?correo=" + correo);
        }

        public async Task BorrarGrupoAlumno(int idGrupo, string idUsuario)
        {
            await this.http.DeleteAsync("api/GrupoAlumnos?IdGrupo=" + idGrupo+"&IdUsuario="+idUsuario);
        }

        public async Task<GrupoAlumnos> CrearGrupoAlumno(GrupoAlumnos grupo)
        {
            return await http.PostJsonAsync<GrupoAlumnos>("api/GrupoAlumnos", grupo);
        }
        
        public async Task BorrarGrupoAlumno(int id)
        {
            await http.DeleteAsync("api/GrupoAlumnos/" + id);
        }
        public async Task<GrupoAlumnos> getGrupoAlumno(int id)
        {
            return await http.GetJsonAsync<GrupoAlumnos>("api/GrupoAlumnos/" + id);
        }
    }
}
