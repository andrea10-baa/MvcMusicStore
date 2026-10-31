using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required, StringLength(50)]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required, StringLength(30)]
        public string Role { get; set; }
    }
}