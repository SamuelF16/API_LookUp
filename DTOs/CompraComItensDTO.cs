using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_LookUp.Models;

namespace API_LookUp.DTOs
{
    public class CompraComItensDTO
    {
        public int IdCompra { get; set; }
        public int IdUsuario { get; set; }
        public ComprasStatus StatusCompra { get; set; } = ComprasStatus.Pendente;
        public decimal ValorTotal { get; set; }
        public DateTime DtaCompra { get; set; } = DateTime.Now;

        /*Itens*/
        public IEnumerable<ItensDeCompraDTO> Itens { get; set; } = new List<ItensDeCompraDTO>();
    }
}