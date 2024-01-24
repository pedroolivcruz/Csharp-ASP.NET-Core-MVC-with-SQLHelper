
namespace Conectasys.Portal.Models
{
    public class RastreabilidadeCordaoInfo
    {
        public string Rastreabilidade { get; set; } 
        public int Cordao { get; set; }
        public int Posto { get; set; }
        public double Corrente { get; set; }
        public double Tensao { get; set; }
        public DateTime Data { get; set; }
        public string Operador { get; set; }
    }
}