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
        
    }
}