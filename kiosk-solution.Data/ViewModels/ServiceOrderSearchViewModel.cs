using System;
using kiosk_solution.Data.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kiosk_solution.Data.ViewModels
{
    public class ServiceOrderSearchViewModel
    {
        [BindNever]
        public Guid? Id { get; set; }
        [BindNever]
        public decimal? Total { get; set; }
        [BindNever]
        public decimal? Commission { get; set; }
        [BindNever]
        public decimal? SystemCommission { get; set; }
        [BindNever]
        public DateTime? CreateDate { get; set; }
        [BindNever]
        public string OrderDetail { get; set; }
        [Guid]
        public Guid? KioskId { get; set; }
        [Guid]
        public Guid? ServiceApplicationId { get; set; }
    }
}