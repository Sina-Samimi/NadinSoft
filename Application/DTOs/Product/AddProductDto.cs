using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class AddProductDto
    {
       
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int Price { get; set; }
    }

    public class AddProductResultDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public int Price { get; set; }
    }
}
