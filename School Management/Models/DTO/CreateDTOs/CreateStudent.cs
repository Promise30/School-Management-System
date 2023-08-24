﻿using System.ComponentModel.DataAnnotations;

namespace School_Management.Models.DTO.CreateDTOs
{
    public class CreateStudent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }
    }
}
