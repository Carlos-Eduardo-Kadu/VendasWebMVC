using VendasWebMvc.Data;
using VendasWebMvc.Models;
using VendasWebMvc.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace VendasWebMvc.Services
{
    public class SellerService
    {
        // Contexto do banco de dados (Entity Framework Core)
        private readonly VendasWebMvcContext _context;

        // Construtor com injeção de dependência do contexto
        public SellerService(VendasWebMvcContext context)
        {
            _context = context;
        }

        // Retorna todos os vendedores do banco (síncrono)
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        // Insere um novo vendedor no banco (assíncrono)
        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj); // Adiciona ao contexto
            await _context.SaveChangesAsync(); // Persiste no banco
        }

        // Busca um vendedor por ID incluindo seu departamento (síncrono)
        public Seller FindById(int id)
        {
            // Usa Include para carregar o relacionamento Department (eager loading)
            return _context.Seller
                .Include(obj => obj.Department)
                .FirstOrDefault(obj => obj.Id == id);
        }

        // Remove um vendedor do banco (síncrono)
        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id); // Localiza o vendedor
            _context.Seller.Remove(obj); // Marca para remoção
            _context.SaveChanges(); // Executa a remoção
        }

        // Atualiza um vendedor existente (síncrono)
        public void Update(Seller obj)
        {
            // Verifica se o vendedor existe no banco
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj); // Marca como modificado
                _context.SaveChanges(); // Persiste as alterações
            }
            catch (DbUpdateConcurrencyException e)
            {
                // Converte a exceção do EF para uma exceção da camada de serviço
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}