using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Context
{
    public class ProEventosContext : IdentityDbContext<User, Role, int, 
                                                       IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, 
                                                       IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        //-migrations passo a passo
        //dotnet ef migrations add Initial -o Data/Migrations
        //dotnet ef migrations add InitialIdentity -p ProEventos.Persistence -s ProEventos.API
        //--------------------------------------------------------------------
        //-Quando a persistencia estiver em ClassLibery usamos os comandos para referenciar onde contém a classe startup
        // dotnet ef migrations add Initial -p ProEventos.Persistence -s ProEventos.API
        //--------------------------------------------------------------------
        //- update database 
        // dotnet ef database update
        //dotnet ef database update -s ProEventos.API
        //--------------------------------------------------------------------
        //-Quando a persistencia estiver relacionada ao projeto que contém startup
        //dotnet ef database update -s ProEventos.API                                     
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options){}
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)       
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProEventosContext).Assembly);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRole>(userRole => 
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId});
                    userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();
                    userRole.HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                }
            );
            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new {PE.EventoId, PE.PalestranteId});
            modelBuilder.Entity<Evento>()
                .HasMany(e => e.RedesSociais)
                .WithOne(rs => rs.Evento)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Palestrante>()
                .HasMany(e => e.RedesSociais)
                .WithOne(rs => rs.Palestrante)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}