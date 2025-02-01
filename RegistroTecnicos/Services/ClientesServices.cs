using Microsoft.EntityFrameworkCore;
using RegistroTecnicos.DAL;
using RegistroTecnicos.Models;
using System.Linq.Expressions;

namespace RegistroTecnicos.Services;

public class ClientesServices
{
    private readonly IDbContextFactory<Contexto> DbFactory;

    public ClientesServices(IDbContextFactory<Contexto> dbFactory)
    {
        DbFactory = dbFactory;
    }

    // Método para verificar si un cliente existe por ID
    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AnyAsync(t => t.ClienteId == id);
    }

    public async Task<bool> ExisteNombreClientes(string nombre, int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(t => t.Nombres.ToLower() == nombre.ToLower() && t.ClienteId != id);
    }

    public async Task<bool> ExisteRncClientes(string rnc, int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(c => c.Rnc.ToLower() == rnc.ToLower() && c.ClienteId != id);
    }

    private async Task<bool> ValidarCliente(Clientes cliente)
    {
        if (await ExisteNombreClientes(cliente.Nombres ?? string.Empty, cliente.ClienteId))
            throw new Exception("Ya existe un cliente con este nombre.");

        if (await ExisteRncClientes(cliente.Rnc ?? string.Empty, cliente.ClienteId))
            throw new Exception("Ya existe un cliente con este RNC.");

        if (string.IsNullOrWhiteSpace(cliente.Nombres) ||
            string.IsNullOrWhiteSpace(cliente.Rnc) ||
            string.IsNullOrWhiteSpace(cliente.Direccion))
            throw new Exception("Todos los campos son obligatorios.");

        if (cliente.Rnc.Length > 10)
            throw new Exception("El RNC no puede tener más de 10 caracteres.");

        return true;
    }

    // Método Insertar
    private async Task<bool> Insertar(Clientes cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await ValidarCliente(cliente);
        contexto.Clientes.Add(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Método Modificar
    private async Task<bool> Modificar(Clientes cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await ValidarCliente(cliente);
        contexto.Clientes.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    // Método Guardar
    public async Task<bool> Guardar(Clientes cliente)
    {
        return await Existe(cliente.ClienteId) ? await Modificar(cliente) : await Insertar(cliente);
    }

    // Método Eliminar
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var cliente = await contexto.Clientes.FindAsync(id);
        if (cliente != null)
        {
            contexto.Clientes.Remove(cliente);
            return await contexto.SaveChangesAsync() > 0;
        }
        return false;
    }

    // Método Buscar
    public async Task<Clientes?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes.AsNoTracking().FirstOrDefaultAsync(t => t.ClienteId == id);
    }

    // Método Listar
    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>>? criterio = null)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return criterio != null ?
            await contexto.Clientes.Where(criterio).ToListAsync() :
            await contexto.Clientes.ToListAsync();
    }
}
