using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class GetAllProductsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ProductDate { get; set; } = DateTime.Now;

        public string ManufacturePhone { get; set; }

        public string ManufactureEmail { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
