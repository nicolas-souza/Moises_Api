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
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private MoisesContext context;
        public UsuariosController(MoisesContext _context)
        {
            context = _context;
        }

        [HttpGet("apiKey={apiKey}")]
        public IActionResult getAllUsuarios()
        {
            var response = from usuario in context.Usuarios
                           select new 
                           {
                                Id = usuario.Id,
                                Nome = usuario.Nome,
                                NivelDeAcesso = usuario.NivelDeAcesso

                           };
            return Ok(response);
        }

        [HttpGet("apiKey={apiKey}/id={id}")]
        public IActionResult GetUsuarioById( int id)
        {
            var response = (from usuario in context.Usuarios
                          where usuario.Id == id
                          select new 
                          {
                                Id = usuario.Id,
                                Nome = usuario.Nome,
                                NivelDeAcesso = usuario.NivelDeAcesso

                          }).FirstOrDefault();

            if (response == null)
                return BadRequest();
            
            return Ok(response);
        }

        // [HttpPost()]
        [HttpPost("apiKey={apiKey}")]
        public IActionResult postNewUsuario(object json, string apiKey)
        {
            Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);
            if (userSession == null)
                return BadRequest();

            try
            {
                if (json == null)
                    return BadRequest();

                DtoUsuario NovoUsuario = JsonConvert.DeserializeObject<DtoUsuario>(json.ToString());
                
                Usuario novoUsuario = new Usuario(NovoUsuario); 

                context.Usuarios.Add(novoUsuario);
                context.SaveChanges();

                var usuarioAdicionado = context.Usuarios.Where(user => user.ChaveDeAcesso == novoUsuario.ChaveDeAcesso);

                return Ok(usuarioAdicionado);

            } catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("apiKey={apiKey}/id={id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuarioDeletado = context.Usuarios.SingleOrDefault(usuario => usuario.Id == id);

            if (usuarioDeletado == null)
                return StatusCode(400);
            

            context.Usuarios.Remove(usuarioDeletado);
            context.SaveChanges();

            var response = new {
                Id = usuarioDeletado.Id,
                Nome = usuarioDeletado.Nome,
                Email = usuarioDeletado.Email,
                NivelDeAcesso = usuarioDeletado.NivelDeAcesso
            };

            return Ok(response);
        }


        
    }
}