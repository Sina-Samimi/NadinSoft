using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuthorizationRquirmentDto
    {

    }
    public class AuthorizationRquirmentDto<TDto>:AuthorizationRquirmentDto where TDto : class
    {
        public TDto Dto { get; set; }
    }
}
