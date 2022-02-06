using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorConteudo.Models
{
    [Table("Usuarios")]
    public class Usuarios
    {   
        [Key]
        public int UserId { get; set; }
        [Required, StringLength(100)]
        public string UserNome { get; set; }
        [Required, EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string UserSenha { get; set; }
        public Usuarios()
        {
        }       
    }
}
