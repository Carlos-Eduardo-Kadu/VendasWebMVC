using VendasWebMvc.Data;
using VendasWebMvc.Models;

namespace VendasWebMvc.Services
{
    public class SellerService
    {
        private readonly VendasWebMvcContext _context;
        public SellerService(VendasWebMvcContext context)
        {
            _context = context;
        }
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

    }
}
