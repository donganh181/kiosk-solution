using System;

namespace kiosk_solution.Data.ViewModels
{
    public class CreateServiceApplicationViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Logo { get; set; }
        public Guid? AppCategoryId { get; set; }
        public bool? IsAffiliate { get; set; }
        public string? Banner { get; set; }
    }
}