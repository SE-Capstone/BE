﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Domain.Entities
{
    public class User
    {
        public Guid Id { get;  set; }
        public string Email { get;  set; } = string.Empty;
        public string Password { get;  set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string FirstName { get;  set; } = string.Empty;
        public string LastName { get;  set; } = string.Empty;
        public Guid RoleId { get; private set; }
    }
}
