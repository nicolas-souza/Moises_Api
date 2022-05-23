using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Database.Models
{
    public class Usuario
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        [Required]
        public string NivelDeAcesso { get; set; } = string.Empty;
        [Required]
        public string ChaveDeAcesso { get; set; } = string.Empty;

        public Usuario()
        {

        }

        public Usuario (DtoUsuario novoUsuario)
        {
            
            this.Nome = novoUsuario.Nome;
            this.Email = novoUsuario.Email;
            this.Senha = novoUsuario.Senha;
            this.NivelDeAcesso = novoUsuario.NivelDeAcesso; 
            this.ChaveDeAcesso = CodeService.Encoder(new DtoDecoderUsuario (){
                Nome = this.Nome,
                Email = this.Email, 
                NivelDeAcesso = this.NivelDeAcesso
                });          
        }

    

    }
}