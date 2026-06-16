using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.Models
{
    [Table("produtos")]
    public class Produtos
    {
        [Key]
        [Column("id_produto")]
        public int IdProduto { get; set; }
        
        [Column("nome")]
        public string Nome { get; set; }
        
        [Column("descricao")]
        public string Descricao { get; set; }
        
        [Column("imagem")]
        public string Imagem { get; set; }
        
        [Column("modelo")]
        public ModeloProduto Modelo { get; set; }
        
        [Column("preco")]
        public decimal Preco { get; set; }
        
        [Column("quantidade_estoque")]
        public int QuantidadeEstoque { get; set; }
        
        [Column("oferta_disponivel")]
        public bool? OfertaDisponivel { get; set; }
        
        [Column("oferta_desconto")]
        public int? OfertaDesconto { get; set; } /*Não sei se é int*/

        /*Chave Estrangeira e Relacionamentos*/
        public virtual ICollection<Itens> Itens { get; set; }
    }
}