namespace api.Database.Models
{
    public class DtoReserva
    {
        public string TituloReserva { get; set; } = string.Empty;
        public string SenhaReserva { get; set; } = string.Empty;
        public DateTime InicioReserva { get; set; }
        public DateTime FimReserva { get; set; }
    }
}