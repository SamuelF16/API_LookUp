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
        public string Nome { get; set; }

        [Column("email")]
        public string Email { get; set; }
        
        [Column("senha_cadastro")]
        public string SenhaCadastrada { get; set; }

        [Column("telefone")]
        public string Telefone { get; set; }

        /*Chave Estrangeira e Relacionamentos*/
        public virtual ICollection<ComprasUsuario> ComprasUsuarios { get; set; }
        public virtual ICollection<EnderecoUsuario> EnderecoUsuarios { get; set; }
    }
}