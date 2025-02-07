using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroTecnicos.DAL;
using RegistroTecnicos.Models;
using RegistroTecnicos.Modelsss;

namespace RegistroTecnicos.Services
{
    public class SistemasService(IDbContextFactory<Contexto> DbFactory)
    {
        // Método para evitar que existan dos sistemas con el mismo nombre
        public async Task<bool> BuscarNombre(int Id, string nombre)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sistemas
                .AnyAsync(S => (S.SistemaId != Id && S.Nombre == nombre));
        }

        public async Task<bool> Existe(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sistemas
                .AnyAsync(S => S.SistemaId == id);
        }

        // Guardar sistema
        public async Task<bool> Guardar(Sistemas sistema)
        {
            if (!await Existe(sistema.SistemaId))
            {
                return await Insertar(sistema);
            }
            else
            {
                return await Modificar(sistema);
            }
        }

        // Insertar sistema
        public async Task<bool> Insertar(Sistemas sistema)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Sistemas.Add(sistema);
            return await contexto.SaveChangesAsync() > 0;
        }

        // Eliminar sistema
        public async Task<bool> Eliminar(int sistemaId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sistemas
                .Where(s => s.SistemaId == sistemaId)
                .ExecuteDeleteAsync() > 0;
        }

        // Listar
        public async Task<List<Sistemas>> Listar(Expression<Func<Sistemas, bool>> criterio)
        {
            await using var Contexto = await DbFactory.CreateDbContextAsync();
            return await Contexto.Sistemas
                .Where(criterio)
                .ToListAsync();
        }

        // Buscar por nombre
        public async Task<Sistemas?> BuscarNombre(string nombre)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Sistemas
                .FirstOrDefaultAsync(n => n.Nombre == nombre);
        }

        // Buscar por ID
        public async Task<Sistemas?> Buscar(int Id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            Sistemas sistema = await contexto.Sistemas
                .FirstOrDefaultAsync(s => s.SistemaId == Id);
            return sistema;
        }

        // Editar
        public async Task<bool> Modificar(Sistemas sistema)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(sistema);
            return await contexto.SaveChangesAsync() > 0;
        }
    }
}
