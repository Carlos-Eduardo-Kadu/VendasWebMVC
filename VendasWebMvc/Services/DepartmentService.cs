using VendasWebMvc.Data;
using VendasWebMvc.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VendasWebMvc.Services
{
    public class DepartmentService
    {
        private readonly VendasWebMvcContext _context;
        public DepartmentService(VendasWebMvcContext context)
        {
            _context = context;
        }
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}

