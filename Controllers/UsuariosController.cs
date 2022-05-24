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
    public class UsuariosController : ControllerBase
    {
        private MoisesContext context;
        public UsuariosController(MoisesContext _context)
        {
            context = _context;
        }

        [HttpGet("apiKey")]
        public IActionResult getAllUsuarios(string apiKey)
        {
            try 
            {
                Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);

                if (userSession == null)
                    return BadRequest();

                var response = from usuario in context.Usuarios
                                select new 
                                {
                                        Id = usuario.Id,
                                        Nome = usuario.Nome,
                                        NivelDeAcesso = usuario.NivelDeAcesso,
                                        Reservas = (from reserva in context.Reservas 
                                                where reserva.Usuario.Id == usuario.Id
                                                select new {
                                                    TituloReserva = reserva.TituloReserva,
                                                    InicioReserva = reserva.InicioReserva,
                                                    FimReserva = reserva.FimReserva,
                                                }).ToList()

                                };
                return Ok(response);

            } catch (Exception ex) 
            {
                return StatusCode(500);
            }
        }

        [HttpGet("apiKey/id")]
        public IActionResult GetUsuarioById( int id, string apiKey)
        {
            try
            {
                Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);
                if (userSession == null)
                    return BadRequest();

                var response = (from usuario in context.Usuarios
                                where usuario.Id == id
                                    select new 
                                    {
                                        Id = usuario.Id,
                                        Nome = usuario.Nome,
                                        NivelDeAcesso = usuario.NivelDeAcesso,
                                        Reservas = (from reserva in context.Reservas 
                                                    where reserva.Usuario.Id == usuario.Id
                                                    select new {
                                                        TituloReserva = reserva.TituloReserva,
                                                        InicioReserva = reserva.InicioReserva,
                                                        FimReserva = reserva.FimReserva,
                                                    }).ToList()

                                }).FirstOrDefault();

                if (response == null)
                    return BadRequest();
                
                return Ok(response);

            } catch (Exception ex) 
            {
                return StatusCode(500);
            }
        }

        
        [HttpPost("apiKey")]
        public IActionResult postNewUsuario(object json, string apiKey)
        {
            try
            {
                Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);

                if (userSession == null || json == null)
                    return BadRequest();

                DtoUsuario _novoUsuario = JsonConvert.DeserializeObject<DtoUsuario>(json.ToString());
                
                Usuario novoUsuario = new Usuario(_novoUsuario); 

                context.Usuarios.Add(novoUsuario);
                context.SaveChanges();

                var usuarioAdicionado = context.Usuarios.
                                            Where(user => user.ChaveDeAcesso == novoUsuario.ChaveDeAcesso).Select(user => new {

                                                Id = user.Id,
                                                Nome = user.Nome,
                                                Email = user.Email,
                                                NivelDeAcesso = user.NivelDeAcesso,                    

                                            });

                
                return Ok(usuarioAdicionado);

            } catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("apiKey/id")]
        public IActionResult DeleteUsuario(int id, string apiKey)
        {
            try 
            {
                Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);

                var usuarioDeletado = context.Usuarios.SingleOrDefault(usuario => usuario.Id == id);

                if (usuarioDeletado == null || userSession == null)
                    return StatusCode(400);
                
                var response = new {
                    Id = usuarioDeletado.Id,
                    Nome = usuarioDeletado.Nome,
                    Email = usuarioDeletado.Email,
                    NivelDeAcesso = usuarioDeletado.NivelDeAcesso, 
                    Reservas = (from reserva in context.Reservas 
                                                    where reserva.Usuario.Id == usuarioDeletado.Id
                                                    select new {
                                                        TituloReserva = reserva.TituloReserva,
                                                        InicioReserva = reserva.InicioReserva,
                                                        FimReserva = reserva.FimReserva,
                                                    }).ToList()               
                };

                context.Reservas.RemoveRange(
                    context.Reservas.Where(
                        reserva => reserva.Usuario.Id == usuarioDeletado.Id));

                context.Usuarios.Remove(usuarioDeletado);
                context.SaveChanges();

                return Ok(response);

            } catch (Exception ex)
            {
                return StatusCode(500);
            }
        
        }        
    }
}