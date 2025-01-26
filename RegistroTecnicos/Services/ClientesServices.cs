using Microsoft.EntityFrameworkCore;
using RegistroTecnicos.DAL;
using RegistroTecnicos.Models;
using System.Linq.Expressions;

namespace RegistroTecnicos.Services;

public class ClientesServices(IDbContextFactory<Contexto> DbFactory)
{
    //Metodo Existe
    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(t => t.ClienteId == id);
    }

    public async Task<bool> ExisteNombreClientes(string nombre, int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(t => t.Nombres.ToLower().Equals(nombre.ToLower()) && t.ClienteId != id);
    }

    public async Task<bool> ExisteRncClientes(string rnc, int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AnyAsync(t => t.Rnc.Equals(rnc) && t.ClienteId != id);
    }

    private async Task<bool> ValidarCliente(Clientes clientes)
    {
        if (await ExisteNombreClientes(clientes.Nombres ?? string.Empty, clientes.ClienteId))
        {
            throw new Exception("Ya existe un cliente con este nombre.");
        }

        if (await ExisteRncClientes(clientes.Rnc ?? string.Empty, clientes.ClienteId))
        {
            throw new Exception("Ya existe un cliente con este RNC.");
        }

        if (string.IsNullOrWhiteSpace(clientes.Nombres) || string.IsNullOrWhiteSpace(clientes.Rnc) || string.IsNullOrWhiteSpace(clientes.Direccion))
        {
            throw new Exception("Todos los campos son obligatorios.");
        }

        if (clientes.Rnc.Length > 10)
        {
            throw new Exception("El RNC no puede tener más de 10 caracteres.");
        }

        return true;
    }

    //Metodo Insertar
    private async Task<bool> Insertar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await ValidarCliente(clientes);
        contexto.Clientes.Add(clientes);
        return await contexto.SaveChangesAsync() > 0;
    }

    //Metodo Modificar 
    private async Task<bool> Modificar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await ValidarCliente(clientes);
        contexto.Update(clientes);
        return await contexto.SaveChangesAsync() > 0;
    }

    //Metodo Guardar
    public async Task<bool> Guardar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (!await Existe(clientes.ClienteId))
            return await Insertar(clientes);
        else
            return await Modificar(clientes);
    }

    //Metodo Eliminar 
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var EliminarClientes = await contexto.Clientes
            .Include(t => t.Tecnicos)
            .Where(t => t.ClienteId == id)
            .ExecuteDeleteAsync();
        return EliminarClientes > 0;
    }

    //Metodo Buscar 
    public async Task<Clientes?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.ClienteId == id);
    }

    //Metodo Listar
    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .Include(t => t.Tecnicos)           
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();

    }
}