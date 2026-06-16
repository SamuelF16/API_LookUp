using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.Models
{
    [Table("compras_usuario")]
    public class ComprasUsuario
    {
        [Key]
        [Column("id_compras_usuario")]
        public int IdComprasUsuario  { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("status_compra")]
        public ComprasStatus StatusCompra { get; set; }

        [Column("valor_total")]
        public decimal ValorTotal { get; set; }

        [Column("dta_compra")]
        public DateTime DtaCompra { get; set; } = DateTime.Now;

        [Column("dta_fechamento")]
        public DateTime DtaFechamento { get; set; }

        /*Chave Estrangeira e Relacionamentos*/
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Itens> Itens { get; set; }
    }
}