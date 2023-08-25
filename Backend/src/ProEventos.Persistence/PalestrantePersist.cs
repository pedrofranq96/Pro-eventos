using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence
{
    public class ProventosPersistence : IPalestrantePersist
    {
        private readonly ProEventosContext _context;
        public ProventosPersistence(ProEventosContext context)
        {
            _context = context;
        }       
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais);
            if(includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            query = query.AsNoTracking().OrderBy(p => p.Id);
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesPorNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais);
            if(includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            query = query.AsNoTracking().OrderBy(p => p.Id).Where(p => 
                p.User.PrimeiroNome.ToLower().Contains(nome.ToLower()));
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante> GetPalestrantePorIdAsync(int palestranteId, bool includeEventos)
        {
           IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais);
            if(includeEventos){
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);
            }
            query = query.AsNoTracking().OrderBy(p => p.Id).Where(p => p.Id == palestranteId);
            return await query.FirstOrDefaultAsync();
        }
    }
}