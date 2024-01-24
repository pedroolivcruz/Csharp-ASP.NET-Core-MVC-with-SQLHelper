
using System.ComponentModel.DataAnnotations;

namespace Conectasys.Portal.Models
{
    public class CordaoInfo
    {   
        public int IdCordao { get; set; }
        public int Cordao { get; set; }
        public int Posto { get; set; }
        public bool PontoSolda { get; set; }
        public string Descricao { get; set; }
        public double CodigoEps { get; set; }
        public byte[] Foto { get; set; }
        public string StringFoto { get; set; }
    }
}