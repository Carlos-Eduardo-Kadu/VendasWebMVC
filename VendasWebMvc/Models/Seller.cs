using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace VendasWebMvc.Models
{
    public class Seller
    {
        // Propriedades básicas do vendedor

        // ID único do vendedor (chave primária)
        public int Id { get; set; }

        // Nome do vendedor

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }

        // Email do vendedor com validação de formato
        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // Data de nascimento com formatação especial
        [Display(Name = "Birth Date")]            // Nome exibido na view
        [DataType(DataType.Date)]                // Tipo de dado (apenas data)
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] // Formato de exibição (dia/mês/ano)
        public DateTime BirthDate { get; set; }

        // Salário base com formatação
        [Required(ErrorMessage = "{0} required")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]         // Nome exibido na view
        [DisplayFormat(DataFormatString = "{0:F2}")] // Formato com 2 casas decimais
        public double BaseSalary { get; set; }

        // Departamento associado (relacionamento muitos-para-um)
        public Department Department { get; set; }

        // Chave estrangeira para o departamento
        public int DepartmentId { get; set; }

        // Lista de vendas (relacionamento um-para-muitos)
        // Inicializada com nova lista vazia para evitar null reference
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        // Construtor vazio (necessário para o EF Core)
        public Seller()
        {
        }

        // Construtor com parâmetros para facilitar a criação de objetos
        public Seller(int id, string name, string email, DateTime birthDate,
                     double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        // Métodos da classe

        // Adiciona um registro de venda à lista de vendas
        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        // Remove um registro de venda da lista de vendas
        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        // Calcula o total de vendas em um período específico
        public double TotalSales(DateTime initial, DateTime final)
        {
            // Filtra vendas pelo período e soma os valores
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final)
                       .Sum(sr => sr.Amount);
        }
    }
}