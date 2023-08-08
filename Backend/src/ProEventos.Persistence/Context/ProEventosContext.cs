using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;

namespace ProEventos.Persistence.Context
{
    public class ProEventosContext: DbContext
    {
        //-migrations passo a passo
        //dotnet ef migrations add Initial -o Data/Migrations
        //--------------------------------------------------------------------
        //-Quando a persistencia estiver em ClassLibery usamos os comandos para referenciar onde contém a classe startup
        // dotnet ef migrations add Initial -p ProEventos.Persistence -s ProEventos.API
        //--------------------------------------------------------------------
        //- update database 
        // dotnet ef database update
        //--------------------------------------------------------------------
        //-Quando a persistencia estiver relacionada ao projeto que contém startup
        //dotnet ef database update -s ProEventos.API                                     
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options){}
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new {PE.EventoId, PE.PalestranteId});
            modelBuilder.Entity<Evento>()
                .HasMany(e => e.RedesSociais).WithOne(rs => rs.Evento)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Palestrante>()
                .HasMany(e => e.RedesSocias).WithOne(rs => rs.Palestrante)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}