
namespace Conectasys.Portal.Models
{
    public class ChecklistSoldagemInfo
    {
        public int IdChecklist { get; set; }
        public string CodigoMaterial { get; set; }
        public int Posto { get; set; }
        public int Sequencia { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public bool LerEtiqueta { get; set; }
        public bool GerarRastreabilidade { get; set; }
        public bool TipoConfirmacao { get; set; }
        public int ValorConfirmacao { get; set; }
        public int NumeroPrograma { get; set; }
        public byte[] Foto { get; set; }
        public string StringFoto { get; set; }
        public bool Ativo { get; set; }
    }
}