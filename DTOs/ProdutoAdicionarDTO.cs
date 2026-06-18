using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_LookUp.Models;

namespace API_LookUp.DTOs
{
    public class ProdutoAdicionarDTO
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public ModeloProduto Modelo { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public bool? OfertaDisponivel { get; set; }
        public int? OfertaDesconto { get; set; }
    }
}