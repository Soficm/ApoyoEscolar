using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Apoyo.Server.Helpers;
using Apoyo.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Apoyo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _singInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public CuentaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> singInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _usermanager = userManager;
            _singInManager = singInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("getUser")]
        [Authorize]
        public async Task<ApplicationUser> getUser()
        {
            return await _context.dbDatosGenerales.Where(m => m.UserName == User.Identity.Name).FirstOrDefaultAsync();
        }
        [HttpPut("userEdit")]
        [Authorize]
        public async Task<ApplicationUser> userEdit(ApplicationUser usuario)
        {
            var user = await _context.dbDatosGenerales.Where(m => m.UserName == User.Identity.Name).FirstOrDefaultAsync();
            user.Apellidos = usuario.Apellidos;
            user.Nombre = usuario.Nombre;
            user.PhoneNumber = usuario.PhoneNumber;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo usuario)
        {

            var user = new ApplicationUser { UserName = usuario.Email, Email = usuario.Email, Tipo=usuario.Tipo };
            var result = await _usermanager.CreateAsync(user, usuario.Password);
            if (result.Succeeded)
            {
                if (usuario.Tipo == TipoUsuario.Alumno)
                {
                    bool existe = await _roleManager.RoleExistsAsync("Alumno");
                    if (!existe)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Alumno"));
                    }
                    await _usermanager.AddToRoleAsync(user, "Alumno");
                }
                else
                {
                    bool existe = await _roleManager.RoleExistsAsync("Docente");
                    if (!existe)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Docente"));
                    }
                    await _usermanager.AddToRoleAsync(user, "Docente");
                }
                return BuildToken(usuario);
            }
            else
            {
                return BadRequest("Username o Password inválido");
            }
        }

        [HttpPost("CrearUsuario")]
        public async Task<ActionResult<UserToken>> CrearUsuario([FromBody] ApplicationUser usuario)
        {
            var result = await _usermanager.CreateAsync(usuario, usuario.PasswordHash);
            if (result.Succeeded)
            {
                bool existe = await _roleManager.RoleExistsAsync("Alumno");
                if (!existe)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Alumno"));
                }
                await _usermanager.AddToRoleAsync(usuario, "Alumno");
                return Ok(usuario);
            }
            else
            {
                return BadRequest("Username o Password inválido");
            }
        }



        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo usuario)
        {
            var result = await _singInManager.PasswordSignInAsync(usuario.Email, usuario.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return BuildToken(usuario);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Datos inválidos");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Recovery")]
        [AllowAnonymous]

        public async Task<ActionResult<string>> Recovery([FromBody] string email)
        {
            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Email no encontrado");

            var token = await _usermanager.GeneratePasswordResetTokenAsync(user);
            var call = Url.ActionLink("/Recovery", null, new { token, email = user.Email }, Request.Scheme);
            var body = "<h1>Recuperar contraseña></h1><br />" +
                "<h3>Favor de acceder al siguiente link <a href='http://localhost:62352/resetPassword?token=" + token+"&email="+email+"'>Reestablecer password</a></h3>";
            await Mails.sendMensaje("Recuperación de contraseña", email, body);

            return Ok("mail enviado");
        }
        [HttpPost("Restart")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Restart([FromBody] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _usermanager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        await Mails.sendMensaje("Restablecimiento de contraseña", user.Email, "<div style='bgcolor:black; color:white;' <p> Su contraseña fue <strong>cambiada</strong> con exito</p> </div>");
                        return Ok(true);

                    }

                }
            }
            return BadRequest(false);
        }

        private async Task<IList<string>> getUserRoles(UserInfo userInfo)
        {
            var user= await _usermanager.FindByNameAsync(userInfo.Email);
            return await _usermanager.GetRolesAsync(user);
        }

        private UserToken BuildToken(UserInfo userInfo)
        {
            Task<IList<string>> roles = Task.Run<IList<string>>(async () => await getUserRoles(userInfo));
            /*var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim("Id", ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };*/
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email));
            claims.Add(new Claim(ClaimTypes.Name, userInfo.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            foreach (var item in roles.Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}