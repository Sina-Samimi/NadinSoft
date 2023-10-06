using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class AddProductDto
    {

        [DisplayName("نام محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "طول {0} نمی تواند بیشتر از {1} باشد")]
        public string Name { get; set; } = null!;

        [DisplayName("شماره تولید کننده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} را وارد کنید.")]
        [StringLength(11, ErrorMessage = "طول {0} نمی تواند کم تر یا بیشتر از {1} باشد")]
        public string ManufacturePhone { get; set; } = null!;

        [DisplayName("ایمیل تولید کننده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "طول {0} نمی تواند بیشتر از {1} باشد")]
        public string ManufactureEmail { get; set; } = null!;

        [DisplayName("موجود")]
        public bool IsAvailable { get; set; } = true;
    }

    public class AddProductResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ProductDate { get; set; } = DateTime.Now;

        public string ManufacturePhone { get; set; }

        public string ManufactureEmail { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
