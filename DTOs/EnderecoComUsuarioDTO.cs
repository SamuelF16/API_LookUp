using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LookUp.DTOs
{
    public class EnderecoComUsuarioDTO
    {
        // Endereco Usuario

        public int IdEnderecoUsuario { get; set; }
        public int IdUsuario { get; set; }
        public string Cep { get; set; }
        public string Estado { get; set; } /*Em apenas 2 letras*/
        public string Municipio { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public int NumeroResidencial { get; set; }
        public string Complemento { get; set; }
        public string TipoEndereco { get; set; }

        // Usuario

        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string TelefoneUsuario { get; set; }
    }
}