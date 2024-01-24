using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Newtonsoft.Json;


namespace Conectasys.Portal.BLL
{
    public class BllChecklistSoldagem
    {
        string fileName = Config.RootFolder + "\\Repositories\\ChecklistsSoldagem.json";


        public List<ChecklistSoldagemInfo> GetAllByPosto(int posto)
        {
            List<ChecklistSoldagemInfo> lstChecklists = new List<ChecklistSoldagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<ChecklistSoldagemInfo>>(fileText);

                lstChecklists = data.Where(x => x.Posto == posto).OrderBy(x => x.Sequencia).ToList();
            }
            else
            {
                DalChecklistSoldagem dalCheckLists = new DalChecklistSoldagem();
                lstChecklists = dalCheckLists.GetAllByPosto(posto);
            }

            return lstChecklists;
        }

        public ChecklistSoldagemInfo GetById(int id)
        {
            ChecklistSoldagemInfo checklist = new ChecklistSoldagemInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<ChecklistSoldagemInfo>>(fileText);

                checklist = data.Find(x => x.IdChecklist == id);
            }
            else
            {
                DalChecklistSoldagem dalCheckLists = new DalChecklistSoldagem();
                checklist = dalCheckLists.GetById(id);
            }

            return checklist;
        }

        public bool Insert(ChecklistSoldagemInfo checkListSoldagemInfo)
        {
            bool retorno = true;

            List<ChecklistSoldagemInfo> lstChecklists = new List<ChecklistSoldagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstChecklists = JsonConvert.DeserializeObject<List<ChecklistSoldagemInfo>>(fileText).OrderBy(x => x.IdChecklist).ToList();

                int lastId = lstChecklists.Last().IdChecklist;

                lstChecklists.Add(new ChecklistSoldagemInfo
                {
                    IdChecklist = lastId + 1,
                    CodigoMaterial = checkListSoldagemInfo.CodigoMaterial,
                    Descricao = checkListSoldagemInfo.Descricao,
                    Posto = checkListSoldagemInfo.Posto,
                    Sequencia = checkListSoldagemInfo.Sequencia,
                    NumeroPrograma = checkListSoldagemInfo.NumeroPrograma,
                    Data = DateTime.Now,
                    LerEtiqueta = checkListSoldagemInfo.LerEtiqueta,
                    GerarRastreabilidade = checkListSoldagemInfo.GerarRastreabilidade,
                    ValorConfirmacao = checkListSoldagemInfo.ValorConfirmacao,
                    Ativo = checkListSoldagemInfo.Ativo,
                    StringFoto = "data:image/png;base64," + Convert.ToBase64String(checkListSoldagemInfo.Foto, 0, checkListSoldagemInfo.Foto.Length)
                });

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstChecklists));
            }
            else
            {
                DalChecklistSoldagem dalCheckLists = new DalChecklistSoldagem();
                retorno = dalCheckLists.Insert(checkListSoldagemInfo);
            }

            return retorno;
        }

        public bool Update(int id, ChecklistSoldagemInfo checklistSoldagemInfo)
        {
            bool retorno = true;

            List<ChecklistSoldagemInfo> lstChecklists = new List<ChecklistSoldagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstChecklists = JsonConvert.DeserializeObject<List<ChecklistSoldagemInfo>>(fileText).OrderBy(x => x.IdChecklist).ToList();

                lstChecklists.Find(x => x.IdChecklist == id).Posto = checklistSoldagemInfo.Posto;
                lstChecklists.Find(x => x.IdChecklist == id).CodigoMaterial = checklistSoldagemInfo.CodigoMaterial;
                lstChecklists.Find(x => x.IdChecklist == id).Descricao = checklistSoldagemInfo.Descricao;
                lstChecklists.Find(x => x.IdChecklist == id).Sequencia = checklistSoldagemInfo.Sequencia;
                lstChecklists.Find(x => x.IdChecklist == id).NumeroPrograma = checklistSoldagemInfo.NumeroPrograma;
                lstChecklists.Find(x => x.IdChecklist == id).LerEtiqueta = checklistSoldagemInfo.LerEtiqueta;
                lstChecklists.Find(x => x.IdChecklist == id).GerarRastreabilidade = checklistSoldagemInfo.GerarRastreabilidade;
                lstChecklists.Find(x => x.IdChecklist == id).ValorConfirmacao = checklistSoldagemInfo.ValorConfirmacao;
                lstChecklists.Find(x => x.IdChecklist == id).Ativo = checklistSoldagemInfo.Ativo;
                lstChecklists.Find(x => x.IdChecklist == id).StringFoto = "data:image/png;base64," + Convert.ToBase64String(checklistSoldagemInfo.Foto, 0, checklistSoldagemInfo.Foto.Length);

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstChecklists));
            }
            else
            {
                DalChecklistSoldagem dalCheckLists = new DalChecklistSoldagem();
                retorno = dalCheckLists.Update(id, checklistSoldagemInfo);
            }

            return retorno;
        }

        public bool Delete(int id)
        {
            bool retorno = true;

            List<ChecklistSoldagemInfo> lstChecklists = new List<ChecklistSoldagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstChecklists = JsonConvert.DeserializeObject<List<ChecklistSoldagemInfo>>(fileText).OrderBy(x => x.IdChecklist).ToList();

                lstChecklists.RemoveAll(x => x.IdChecklist == id);

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstChecklists));
            }
            else
            {
                DalChecklistSoldagem dalCheckLists = new DalChecklistSoldagem();
                retorno = dalCheckLists.Delete(id);
            }

            return retorno;
        }
    }
}