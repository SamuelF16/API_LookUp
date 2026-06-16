using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.Models
{
    [Table("endereco_usuario")]
    public class EnderecoUsuario
    {
        [Key]
        [Column("id_endereco_usuario")]
        public int IdEnderecoUsuario { get; set; }
        
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("cep")]
        public string Cep { get; set; }

        [Column("estado")]
        public string Estado { get; set; } /*Em apenas 2 letras*/

        [Column("municipio")]
        public string Municipio { get; set; }

        [Column("bairro")]
        public string Bairro { get; set; }

        [Column("rua")]
        public string Rua { get; set; }

        [Column("numero_residencial")]
        public int NumeroResidencial { get; set; }

        [Column("complemento")]
        public string Complemento { get; set; }

        [Column("tipo_endereco")]
        public string TipoEndereco { get; set; }

        /*Chave Estrangeira*/
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

    }
}