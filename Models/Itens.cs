using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.Models
{
    [Table("itens")]
    public class Itens
    {
        [Key]
        [Column("id_itens")]
        public int IdItens { get; set; }

        [Column("id_produto")]
        public int IdProduto { get; set; }

        [Column("id_compras_usuario")]
        public int IdComprasUsuario { get; set; }

        [Column("preco_unitario")]
        public decimal PrecoUnitario { get; set; }

        [Column("quantidade_produtos")]
        public int QuantidadeProdutos { get; set; }

        /*Chaves Estrangeiras*/
        [ForeignKey("IdProduto")]
        public virtual Produtos Produto { get; set; }

        [ForeignKey("IdComprasUsuario")]
        public virtual ComprasUsuario CompraUsuario { get; set; }
    }
}