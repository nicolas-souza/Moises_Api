using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Database.Context;
using api.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

#pragma warning disable

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private MoisesContext context;
        public AuthController ( MoisesContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult Login(DtoLogin login)
        {
            var response = (from usuario in context.Usuarios
                            where usuario.Email == login.Email && usuario.Senha == login.Senha
                            select new 
                            {
                                ApiKey = usuario.ChaveDeAcesso,
                                NivelDeAcesso = usuario.NivelDeAcesso

                            }).FirstOrDefault();
                            
            return Ok(response);
        }
    }
}