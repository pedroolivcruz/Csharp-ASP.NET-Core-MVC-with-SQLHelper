
namespace Conectasys.Portal.Models
{
    public class ComponenteInfo
    {
        public int IdComponente { get; set; }
        public string TipoMaterial { get; set; }
        public string Descricao { get; set; }
        public string CodigoSAP { get; set; }
        public int Posto { get; set; }
        public int Sequencia { get; set; }
        public bool Ativo { get; set; }
        public int Modelo { get; set; }
    }
}