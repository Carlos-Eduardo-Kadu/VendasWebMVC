using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VendasWebMvc.Data;
using VendasWebMvc.Models;

namespace VendasWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        // Contexto do banco de dados (Entity Framework Core)
        private readonly VendasWebMvcContext _context;

        // Injeção de dependência do contexto
        public DepartmentsController(VendasWebMvcContext context)
        {
            _context = context;
        }

        // GET: Departments
        // Lista todos os departamentos (assíncrono)
        public async Task<IActionResult> Index()
        {
            // Verifica se a tabela Department existe antes de consultar
            return _context.Department != null ?
                        View(await _context.Department.ToListAsync()) :
                        Problem("Entity set 'VendasWebMvcContext.Department' is null.");
        }

        // GET: Departments/Details/5
        // Mostra detalhes de um departamento específico
        public async Task<IActionResult> Details(int? id)
        {
            // Validações de segurança
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            // Busca o departamento no banco (assíncrona)
            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        // Exibe o formulário de criação
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // Processa o formulário de criação
        [HttpPost]
        [ValidateAntiForgeryToken] // Proteção contra CSRF
        public async Task<IActionResult> Create(
            [Bind("Id,Name")] Department department) // Bind para proteção contra overposting
        {
            if (ModelState.IsValid) // Validação do modelo
            {
                _context.Add(department);
                await _context.SaveChangesAsync(); // Persiste no banco
                return RedirectToAction(nameof(Index)); // Redireciona para lista
            }
            return View(department); // Se inválido, mostra novamente com erros
        }

        // GET: Departments/Edit/5
        // Exibe formulário de edição
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FindAsync(id); // Busca pelo ID
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // Processa o formulário de edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id) // Verifica consistência do ID
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync(); // Atualiza no banco
                }
                catch (DbUpdateConcurrencyException) // Trata concorrência
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Relança outras exceções
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        // Exibe tela de confirmação de exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        // Processa a exclusão (usa DeleteConfirmed para evitar conflito de rotas)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Department == null)
            {
                return Problem("Entity set 'VendasWebMvcContext.Department' is null.");
            }

            var department = await _context.Department.FindAsync(id);
            if (department != null)
            {
                _context.Department.Remove(department); // Marca para exclusão
            }

            await _context.SaveChangesAsync(); // Efetiva a exclusão
            return RedirectToAction(nameof(Index));
        }

        // Método auxiliar para verificar existência de departamento
        private bool DepartmentExists(int id)
        {
            return (_context.Department?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}