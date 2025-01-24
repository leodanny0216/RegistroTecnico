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
            .AnyAsync(t => t.clientesId == id);
    }
    public async Task<bool> ExisteNombreClientes(string nombre, int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Tecnicos
            .AnyAsync(t => t.Nombres.ToLower().Equals(nombre.ToLower()) && t.TecnicoId != id);
    }
    //Metodo Insertar
    private async Task<bool> Insertar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Clientes.Add(clientes);
        return await contexto.SaveChangesAsync() > 0;
    }
    //Metodo Modificar 
    private async Task<bool> Modificar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(clientes);
        return await contexto.SaveChangesAsync() > 0;
    }
    //Metodo Guardar
    public async Task<bool> Guardar(Clientes clientes)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (!await Existe(clientes.clientesId))

            return await Insertar(clientes);
        else
            return await Modificar(clientes);
    }
    //Metodo Eliminar 
    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var EliminarClientes = await contexto.Clientes
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
            .FirstOrDefaultAsync(t => t.clientesId == id);
    }
    //Metodo Listar
    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Clientes
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }
}
