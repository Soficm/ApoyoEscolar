using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apoyo.Services
{
    
    public class CuentaService
    {
        [Inject]
        private HttpClient http { get; set; }
        public CuentaService(HttpClient http)
        {
            this.http = http;

        }
        public async Task<UserToken>CrearUsuario(UserInfo user)
        {
            return await http.PostJsonAsync<UserToken>("api/cuenta/crear", user);
        }

        public async Task<ApplicationUser> CrearAlumno(ApplicationUser user)
        {
            return await http.PostJsonAsync<ApplicationUser>("api/cuenta/CrearUsuario", user);
        }

        public async Task<UserToken> Login(UserInfo user)
        {
            return await http.PostJsonAsync<UserToken>("api/cuenta/login", user);
        }

        public async Task<bool> recoveryPassword(string email)
        {
            return await http.PostJsonAsync<bool>("api/cuenta/Recovery", email);
        }

        public async Task<bool> resetPassword(ResetPasswordViewModel model)
        {
            return await http.PostJsonAsync<bool>("api/cuenta/Restart", model);
        }

        public async Task<ApplicationUser> getUser()
        {
            return await http.GetJsonAsync<ApplicationUser>("api/cuenta/getUser");
        }
        public async Task<ApplicationUser> guardarUsuario(ApplicationUser usuario)
        {
            return await http.PutJsonAsync<ApplicationUser>("api/cuenta/userEdit", usuario);
        }
    }
}
