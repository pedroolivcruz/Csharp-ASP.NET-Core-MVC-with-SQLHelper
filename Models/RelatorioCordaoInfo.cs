
namespace Conectasys.Portal.Models
{
    public class RelatorioCordaoInfo
    {  
        public int Cordao { get; set; }
        public double CorrenteMaxima { get; set; }
        public double CorrenteMinima { get; set; }
        public double TensaoMaxima { get; set; }
        public double TensaoMinima { get; set; }
        public string CodigoEps { get; set; }
    }
}
