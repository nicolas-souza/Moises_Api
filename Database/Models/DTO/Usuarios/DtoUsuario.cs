
namespace api.Database.Models
{
    public class DtoUsuario 
    {
        
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string NivelDeAcesso { get; set; } = string.Empty;    

    }
}