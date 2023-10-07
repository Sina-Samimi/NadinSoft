using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime ProductDate { get; set; } = DateTime.Now;

        public string ManufacturePhone { get; set; } = null!;

        public string ManufactureEmail { get; set; }=null!;
        public bool IsAvailable { get; set; } = true;

        #region Relation
        public IdentityUser User { get; set; }
        #endregion
    }
}
