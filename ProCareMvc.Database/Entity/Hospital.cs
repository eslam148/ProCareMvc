﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCareMvc.Database.Entity
{
    public class Hospital:BaseEntity
    {

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        
        public ICollection<Drug> Drugs { get; set; }
        public ICollection<Department> Departments { get; set; }

    }
}

