﻿using System;

namespace Alinta.CustomersApi.Models
{
    public class CustomerReadModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
