﻿using VendasWebMvc.Models.Enums;

namespace VendasWebMvc.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public SalesStatus Status { get; set; }

        // Chave estrangeira para Seller
        public int SellerId { get; set; }

        // Propriedade de navegação
        public Seller Seller { get; set; }

        public SalesRecord()
        {

        }

        public SalesRecord(int id, DateTime date, double amount, SalesStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
            SellerId = seller.Id;
        }
    }
}
