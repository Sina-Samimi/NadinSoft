using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ManufacturePhone { get; set; }

        public string ManufactureEmail { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
