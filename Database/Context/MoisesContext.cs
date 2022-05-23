using api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Database.Context
{
    #pragma warning disable CS1591
    public class MoisesContext: DbContext
    {
        public MoisesContext(DbContextOptions<MoisesContext> options): base(options)
        {

        }
        public DbSet<Usuario>? Usuarios { get; set; }
        public DbSet<Reserva>? Reservas { get; set; }
        

    }
    #pragma warning restore CS1591
}