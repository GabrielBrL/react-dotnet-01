using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorConteudo.Models
{
    [Table("Clientes")]
    public class Clientes
    {
        [Key]
        public int ClienteId { get; set; }
        [Required, StringLength(100), MinLength(5)]
        public string ClienteNome { get; set; }
        [Required, StringLength(11), MinLength(11)]        
        public string CPF { get; set; }
        [Required]
        public string Telefone { get; set; }
        public Clientes()
        {
        }
    }
}
