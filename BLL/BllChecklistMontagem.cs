using Conectasys.Portal.Models;
using Conectasys.Portal.DAL;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;



namespace Conectasys.Portal.BLL
{
    public class BllChecklistMontagem
    {
        string fileName = Config.RootFolder + "\\Repositories\\ChecklistsMontagem.json";


        public List<ChecklistMontagemInfo> GetAllByPosto(int posto)
        {   
            List<ChecklistMontagemInfo> lstChecklists = new List<ChecklistMontagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<ChecklistMontagemInfo>>(fileText);

                lstChecklists = data.Where(x => x.Posto == posto).OrderBy(x => x.Sequencia).ToList();
            }
            else
            {
                DalChecklistMontagem dalCheckLists = new DalChecklistMontagem();
                lstChecklists = dalCheckLists.GetAllByPosto(posto);
            }

            return lstChecklists;
        }

        public List<ChecklistMontagemInfo> GetAllByPosto2(int posto, int numPrograma)
        {
            List<ChecklistMontagemInfo> lstChecklists = new List<ChecklistMontagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<ChecklistMontagemInfo>>(fileText);

                lstChecklists = data.Where(x => x.Posto == posto && x.NumeroPrograma == numPrograma).OrderBy(x => x.Sequencia).ToList();
            }
            else
            {
                DalChecklistMontagem dalCheckLists = new DalChecklistMontagem();
                lstChecklists = dalCheckLists.GetAllByPosto2(posto, numPrograma);
            }

            return lstChecklists;
        }

        public ChecklistMontagemInfo GetById(int id)
        {
            ChecklistMontagemInfo checklist = new ChecklistMontagemInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<ChecklistMontagemInfo>>(fileText);

                checklist = data.Find(x => x.IdChecklist == id);
            }
            else
            {
                DalChecklistMontagem dalCheckLists = new DalChecklistMontagem();
                checklist = dalCheckLists.GetById(id);
            }

            return checklist;
        }

        public bool Insert(ChecklistMontagemInfo checkListMontagemInfo)
        {
            bool retorno = true;

            List<ChecklistMontagemInfo> lstChecklists = new List<ChecklistMontagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstChecklists = JsonConvert.DeserializeObject<List<ChecklistMontagemInfo>>(fileText).OrderBy(x => x.IdChecklist).ToList();

                int lastId = lstChecklists.Last().IdChecklist;

                lstChecklists.Add(new ChecklistMontagemInfo
                {
                    IdChecklist = lastId + 1,
                    CodigoMaterial = checkListMontagemInfo.CodigoMaterial,
                    Descricao = checkListMontagemInfo.Descricao,
                    Posto = checkListMontagemInfo.Posto,
                    Sequencia = checkListMontagemInfo.Sequencia,
                    NumeroPrograma = checkListMontagemInfo.NumeroPrograma,
                    Data = DateTime.Now,
                    LerEtiqueta = checkListMontagemInfo.LerEtiqueta,
                    GerarRastreabilidade = checkListMontagemInfo.GerarRastreabilidade,
                    ValidarRastreabilidade = checkListMontagemInfo.ValidarRastreabilidade,
                    ValorConfirmacao = checkListMontagemInfo.ValorConfirmacao,
                    Ativo = checkListMontagemInfo.Ativo,
                    StringFoto = "data:image/png;base64," + Convert.ToBase64String(checkListMontagemInfo.Foto, 0, checkListMontagemInfo.Foto.Length)
                });

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstChecklists));
            }
            else
            {
                DalChecklistMontagem dalCheckLists = new DalChecklistMontagem();
                retorno = dalCheckLists.Insert(checkListMontagemInfo); 
            }

            return retorno;
        }

        public bool Update(int id,ChecklistMontagemInfo checkListMontagemInfo)
        {
            bool retorno = true;

            List<ChecklistMontagemInfo> lstChecklists = new List<ChecklistMontagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstChecklists = JsonConvert.DeserializeObject<List<ChecklistMontagemInfo>>(fileText).OrderBy(x => x.IdChecklist).ToList();

                lstChecklists.Find(x => x.IdChecklist == id).Posto = checkListMontagemInfo.Posto;
                lstChecklists.Find(x => x.IdChecklist == id).CodigoMaterial = checkListMontagemInfo.CodigoMaterial;
                lstChecklists.Find(x => x.IdChecklist == id).Descricao = checkListMontagemInfo.Descricao;
                lstChecklists.Find(x => x.IdChecklist == id).Sequencia = checkListMontagemInfo.Sequencia;
                lstChecklists.Find(x => x.IdChecklist == id).NumeroPrograma = checkListMontagemInfo.NumeroPrograma;
                lstChecklists.Find(x => x.IdChecklist == id).LerEtiqueta = checkListMontagemInfo.LerEtiqueta;
                lstChecklists.Find(x => x.IdChecklist == id).GerarRastreabilidade = checkListMontagemInfo.GerarRastreabilidade;
                lstChecklists.Find(x => x.IdChecklist == id).ValidarRastreabilidade = checkListMontagemInfo.ValidarRastreabilidade;
                lstChecklists.Find(x => x.IdChecklist == id).ValorConfirmacao = checkListMontagemInfo.ValorConfirmacao;
                lstChecklists.Find(x => x.IdChecklist == id).Ativo = checkListMontagemInfo.Ativo;
                lstChecklists.Find(x => x.IdChecklist == id).StringFoto = "data:image/png;base64," + Convert.ToBase64String(checkListMontagemInfo.Foto, 0, checkListMontagemInfo.Foto.Length);

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstChecklists));
            }
            else
            {
                DalChecklistMontagem dalCheckLists = new DalChecklistMontagem();
                retorno = dalCheckLists.Update(id, checkListMontagemInfo);
            }

            return retorno;
        }

        public bool Delete(int id)
        {
            bool retorno = true;

            List<ChecklistMontagemInfo> lstChecklists = new List<ChecklistMontagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstChecklists = JsonConvert.DeserializeObject<List<ChecklistMontagemInfo>>(fileText).OrderBy(x => x.IdChecklist).ToList();

                lstChecklists.RemoveAll(x => x.IdChecklist == id);

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstChecklists));
            }
            else
            {
                DalChecklistMontagem dalCheckLists = new DalChecklistMontagem();
                retorno = dalCheckLists.Delete(id);
            }

            return retorno;
        }
    }
}