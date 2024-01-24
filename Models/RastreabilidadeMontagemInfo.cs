
namespace Conectasys.Portal.Models
{
    public class RastreabilidadeMontagemInfo
    {
        public int Index { get; set; }
        public string Rastreabilidade { get; set; }
        public int Posto { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Operador { get; set; }
        public int Status { get; set; }
        public string Descricao { get; set; }
        public string Aprovacao { get; set; }

    }
}