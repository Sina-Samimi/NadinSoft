using Application.DTOs.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class GetProductByIdDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ManufacturePhone { get; set; }

        public string ManufactureEmail { get; set; }
        public bool IsAvailable { get; set; }

        public UserDto User { get; set; }
    }
}
