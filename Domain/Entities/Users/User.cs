﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class User:IdentityUser<Guid>
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
    }
}
