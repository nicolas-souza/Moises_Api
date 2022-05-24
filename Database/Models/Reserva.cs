using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Database.Models
{
    public class Reserva
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string TituloReserva { get; set; } = string.Empty;
        [Required]
        public string SenhaReserva { get; set; } = string.Empty;
        [Required]
        public Usuario Usuario { get; set; } = new Usuario();
        [Required]
        public DateTime InicioReserva { get; set; }
        [Required]
        public DateTime FimReserva { get; set; }

        public Reserva (){ }
        
        public Reserva (DtoReserva novaReserva)
        {
            this.TituloReserva = novaReserva.TituloReserva;
            this.SenhaReserva = novaReserva.SenhaReserva;
            this.InicioReserva = novaReserva.InicioReserva;
            this.FimReserva = novaReserva.FimReserva;

            
        }

        public Reserva (DtoReserva novaReserva, Usuario userSession)
        {
            this.TituloReserva = novaReserva.TituloReserva;
            this.SenhaReserva = novaReserva.SenhaReserva;
            this.InicioReserva = novaReserva.InicioReserva;
            this.FimReserva = novaReserva.FimReserva;

            this.Usuario = userSession;
        }
    }
}