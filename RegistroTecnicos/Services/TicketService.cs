using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroTecnicos.DAL;
using RegistroTecnicos.Models;

namespace RegistroTecnicos.Services;

public class TicketService
{
    private readonly IDbContextFactory<Contexto> _dbFactory;

    public TicketService(IDbContextFactory<Contexto> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    // Método para verificar si un Ticket existe por ID
    public async Task<bool> Existe(int id)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Tickets.AnyAsync(t => t.TicketId == id);
    }

    // Método Insertar
    private async Task<bool> Insertar(Tickets ticket)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        contexto.Tickets.Add(ticket);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Método Modificar
    private async Task<bool> Modificar(Tickets ticket)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        // Si 'ValidarTicket' es una función que necesitas, defínela antes de usarla.
        // Si no es necesaria, simplemente elimínala.
        // await ValidarTicket(ticket); 

        contexto.Entry(ticket).State = EntityState.Modified;
        return await contexto.SaveChangesAsync() > 0;
    }

    // Método Guardar (Inserta o Modifica)
    public async Task<bool> Guardar(Tickets ticket)
    {
        return await Existe(ticket.TicketId) ? await Modificar(ticket) : await Insertar(ticket);
    }

    // Método Eliminar
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        var ticket = await contexto.Tickets.FindAsync(id);
        if (ticket != null)
        {
            contexto.Tickets.Remove(ticket);
            return await contexto.SaveChangesAsync() > 0;
        }
        return false;
    }

    // Método Buscar un Ticket por ID con relaciones
    public async Task<Tickets?> Buscar(int id)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TicketId == id);
    }

    // Método Listar Tickets con criterio opcional
    public async Task<List<Tickets>> Listar(Expression<Func<Tickets, bool>> criterio)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Tickets
            .AsNoTracking()
            .Include(p => p.Tecnicos)
            .Where(criterio)
            .ToListAsync();
    }
}