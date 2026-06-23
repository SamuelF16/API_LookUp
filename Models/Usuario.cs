using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;
        
        [Column("senha_cadastro")]
        public string SenhaCadastrada { get; set; } = null!;

        [Column("telefone")]
        public string Telefone { get; set; } = null!;

        /*Chave Estrangeira e Relacionamentos*/
        public virtual ICollection<ComprasUsuario> ComprasUsuarios { get; set; } = new List<ComprasUsuario>();
        public virtual ICollection<EnderecoUsuario> EnderecoUsuarios { get; set; } = new List<EnderecoUsuario>();
    }
}