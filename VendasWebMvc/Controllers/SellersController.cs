// Importações necessárias para o controller
using Microsoft.AspNetCore.Mvc;          // Para classes Controller, IActionResult, etc.
using Microsoft.EntityFrameworkCore;    // Para DbConcurrencyException
using VendasWebMvc.Models;              // Modelos de domínio (Seller, Department)
using VendasWebMvc.Models.ViewModels;   // ViewModels (SellerFormViewModel)
using VendasWebMvc.Services;            // Serviços de aplicação
using VendasWebMvc.Services.Exceptions; // Exceções personalizadas
using System.Diagnostics;               // Para Activity (log de erros)
using Azure.Core;                       // Possivelmente para integração com Azure
using NuGet.Protocol.Plugins;

namespace VendasWebMvc.Controllers
{
    public class SellersController : Controller
    {
        // Injeção de dependência dos serviços
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        // Construtor com injeção de dependência
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        // GET: Sellers
        // Lista todos os vendedores
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        // GET: Sellers/Create
        // Mostra o formulário de criação de vendedor
        public IActionResult Create()
        {
            // Carrega os departamentos para o dropdown
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        // POST: Sellers/Create
        // Processa o formulário de criação
        [HttpPost]
        [ValidateAntiForgeryToken]  // Proteção contra CSRF
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            await _sellerService.InsertAsync(seller);  // Insere assincronamente
            return RedirectToAction(nameof(Index));    // Redireciona para a lista
        }

        // GET: Sellers/Delete/5
        // Mostra tela de confirmação de exclusão
        public IActionResult Delete(int? id)
        {
            if (id == null)  // Verifica se ID foi fornecido
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)  // Verifica se existe o vendedor
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        // POST: Sellers/Delete/5
        // Processa a exclusão
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));  // Redireciona para a lista
        }

        // GET: Sellers/Details/5
        // Mostra detalhes do vendedor
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        // GET: Sellers/Edit/5
        // Mostra formulário de edição
        public IActionResult Edit(int? id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departmenTs = _departmentService.FindAll();
                var viewModeL = new SellerFormViewModel { Seller = seller, Departments = departmenTs };
                return View(viewModeL);
            }
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // Carrega departamentos para o dropdown
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        // POST: Sellers/Edit/5
        // Processa o formulário de edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)  // Verifica consistência do ID
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)  // Se o vendedor não existir
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            catch (DbConcurrencyException)  // Se houve conflito de concorrência
            {
                return BadRequest();
            }
        }

        // Tratamento de erros genérico
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                // Identificador único para rastreamento do erro
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}