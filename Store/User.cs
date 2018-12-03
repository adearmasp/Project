﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    public sealed class User
    {
        public int Id { get; set; }

        public bool IsAdmin { get; set; }
       
        public string Name { get; set; }

        public string Email { get; set; }
      
        public string Password { get; set; }
    }
}
