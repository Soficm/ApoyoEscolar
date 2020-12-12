using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apoyo.Services
{
    public class MateriaService
    {
        [Inject]
        private HttpClient http { get; set; }
        public MateriaService(HttpClient http)
        {
            this.http = http;
        }
        public async Task<List<Materias>> getMaterias(string userName)
        {
            return await this.http.GetJsonAsync<List<Materias>>("api/materias/getMateriasDocente?userName=" + userName);
        }
        public async Task<List<Materias>> getMateriasByGrupo(int idGrupo)
        {
            return await this.http.GetJsonAsync<List<Materias>>("api/materias/getMateriasByGrupo?idGrupo=" + idGrupo);
        }
        public async Task<Materias> CrearMateria(Materias materia)
        {
            return await http.PostJsonAsync<Materias>("api/materias", materia);
        }
        public async Task<Materias> EditarMateria(Materias materia)
        {
            return await http.PutJsonAsync<Materias>("api/materias", materia);
        }
        public async Task BorrarMateria(int id)
        {
            await http.DeleteAsync("api/materias/"+id);
        }
    }
}
