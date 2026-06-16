using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.DTOs
{
    public class CompraComUsuarioDTO
    {
        // Compra Usuario

        public int IdComprasUsuario  { get; set; }
        public int IdUsuario { get; set; }
        public string StatusCompra { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DtaCompra { get; set; }
        public DateTime DtaFechamento { get; set; }
        
        // Usuario

        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string TelefoneUsuario { get; set; }
    }
}