
namespace Conectasys.Portal.Models
{
    public class RelatorioSoldagemInfo
    {
        public int cordao { get; set; }
        public List<double> corrente { get; set; }
        public double correnteMaxima { get; set; }
        public double correnteMinima { get; set; }
        public List<double> tensao { get; set; }
        public double tensaoMaxima { get; set; }
        public double tensaoMinima { get; set; }
        public List<int> sinais { get; set; }
        public string codigoEps { get; set; }
        public string stringFoto { get; set; }
    }
}