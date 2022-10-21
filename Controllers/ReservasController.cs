using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Database.Context;
using api.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

//#pragma warning disable

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private MoisesContext context;
        public ReservasController ( MoisesContext _context)
        {
            context = _context;
        }

        [HttpGet()]
        public IActionResult GetAllReservas ()
        {
            try
            {
                Console.WriteLine("funcionou");
                var reservas = from reserva in context.Reservas
                               from usuario in context.Usuarios
                               where reserva.Usuario.Id == usuario.Id
                               select new
                               {
                                   Id = reserva.Id,
                                   TituloReserva = reserva.TituloReserva,
                                   InicioReserva = reserva.InicioReserva.ToString("yyyy-MM-dd HH:mm"),
                                   FimReserva = reserva.FimReserva.ToString("yyyy-MM-dd HH:mm"),
                                   Usuario = new
                                   {
                                       Nome = usuario.Nome,
                                       Email = usuario.Email,
                                       NivelDeAcesso = usuario.NivelDeAcesso
                                   }
                               };

                return Ok(reservas);

            }
            catch (Exception ex)
            {
                Log.RegistroErro("nicolas", ex);
                return StatusCode(500);

            }

           // return Ok();
        }

        [HttpGet("{apiKey}/{id}")]
        public IActionResult GetById(string apiKey, int id)
        {
            try 
            {
                Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);

                if (userSession == null)
                    return BadRequest();

                var reservas = from reserva in context.Reservas.Where(r => r.Id == id)
                               from usuario in context.Usuarios
                                where reserva.Usuario.Id == usuario.Id
                                select new 
                                {
                                   Id = reserva.Id,
                                   TituloReserva = reserva.TituloReserva,
                                   InicioReserva = reserva.InicioReserva,
                                   FimReserva = reserva.FimReserva,
                                   Usuario = new 
                                                {
                                                    Nome = usuario.Nome,
                                                    Email = usuario.Email,
                                                    NivelDeAcesso = usuario.NivelDeAcesso
                                                }
                                };

                if (userSession == null)
                    return BadRequest();

                return Ok();

            } catch (Exception ex)
            {
                
                return StatusCode(500);

            }
        }

        [HttpGet("reservasUsuario/{apiKey}")]
        public IActionResult GetByUser(string apiKey)
        {
            try 
            {
                Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);

                if (userSession == null)
                    return BadRequest();

                var reservas = from reserva in context.Reservas.Where(r => r.Usuario.Id == userSession.Id)
                               from usuario in context.Usuarios
                                where reserva.Usuario.Id == usuario.Id
                                select new 
                                {
                                   Id = reserva.Id,
                                   TituloReserva = reserva.TituloReserva,
                                   InicioReserva = reserva.InicioReserva,
                                   FimReserva = reserva.FimReserva,
                                   Usuario = new 
                                                {
                                                    Nome = usuario.Nome,
                                                    Email = usuario.Email,
                                                    NivelDeAcesso = usuario.NivelDeAcesso
                                                }
                                };

                return Ok(reservas);

            } catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpPost("{apiKey}")]
        public IActionResult PostReserva (string apiKey, object json)
        {
            bool reservaInvalida = false;

            try 
            {
                Usuario? userSession = context.Usuarios.Where(usuario => usuario.ChaveDeAcesso == apiKey).FirstOrDefault();

                if (userSession == null || json == null)
                    return BadRequest();

                DtoReserva? _novaReserva = JsonConvert.DeserializeObject<DtoReserva>(json.ToString());
                
                Reserva novaReserva = new Reserva(_novaReserva, userSession);

                if(novaReserva.InicioReserva < DateTime.Now || novaReserva.InicioReserva > novaReserva.FimReserva)
                    return StatusCode(400);
                
                foreach (Reserva reserva in context.Reservas.Where(reserva => reserva.InicioReserva > DateTime.Now))
                {
                    if (((novaReserva.InicioReserva < reserva.InicioReserva) && (novaReserva.FimReserva < reserva.InicioReserva)) || 
                            ((novaReserva.InicioReserva > reserva.FimReserva) && (novaReserva.FimReserva > reserva.FimReserva)))
                    {
                        
                    } else 
                    {
                        reservaInvalida = true;
                    }
                }

                if(reservaInvalida)
                    return StatusCode(400);


                context.Reservas.Add(novaReserva);

                context.SaveChanges();

                return Ok();

            } catch (Exception ex)
            {

                return StatusCode(500);
            }
        }

        [HttpDelete("{apiKey}/{id}")]
        public IActionResult DeleteById (string apiKey, int id)
        {
            try 
            {
                Usuario? userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);

                Reserva? reserva = context.Reservas.SingleOrDefault(res => res.Id == id);

                if (userSession?.Id == reserva?.Usuario.Id || userSession?.NivelDeAcesso == "admin")
                {
                    context.Reservas.Remove(reserva);

                    context.SaveChanges();
                } 
                else 
                {
                    return StatusCode(400);
                }

                return Ok();

            } catch (Exception ex)
            {

                return StatusCode(500);
            }
        }
        
        [HttpGet("fechadura/{apiKey}")]
        public IActionResult GetReservaMoment(string apiKey)
        {
            var response = new
            {
                TituloReserva = "Disponível",
                SenhaReserva = "123456",
                InicioReserva = DateTime.Now.AddDays(-1),
                FimReserva = DateTime.Now
            };

            try
            {
                Usuario userSession = context.Usuarios.SingleOrDefault(usuario => usuario.ChaveDeAcesso == apiKey);

                if (userSession == null)
                    return StatusCode(400);
                
                Reserva? reserva = context.Reservas.SingleOrDefault(res => res.InicioReserva <= DateTime.Now && DateTime.Now < res.FimReserva);

                if (reserva != null)
                {
                    response = new
                    {
                        TituloReserva = reserva.TituloReserva,
                        SenhaReserva = reserva.SenhaReserva,
                        InicioReserva = reserva.InicioReserva,
                        FimReserva = reserva.FimReserva
                    };
                }                              

                return Ok(response);

            } catch (Exception ex)
            {
                return StatusCode(500);
            }
           
        }
    }
}