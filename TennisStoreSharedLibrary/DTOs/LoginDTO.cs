﻿using System.ComponentModel.DataAnnotations;

namespace TennisStoreSharedLibrary.DTOs
{
    public class LoginDTO
    {
        [Required , EmailAddress , DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required , DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
