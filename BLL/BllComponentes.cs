using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Newtonsoft.Json;
using System.Reflection;


namespace Conectasys.Portal.BLL
{
    public class BllComponentes
    {
        string fileName = Config.RootFolder + "\\Repositories\\Componentes.json";

        public List<ComponenteInfo> SearchComponentes(int cordao, int modelo)
        {
            List<ComponenteInfo> lstComponentes = new List<ComponenteInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<ComponenteInfo>>(fileText);

                lstComponentes = data.OrderBy(x => x.IdComponente).ToList();
            }
            else
            {
                DalComponentes dalComponentes = new DalComponentes();
                lstComponentes = dalComponentes.SearchComponentes(cordao, modelo);
            }

            return lstComponentes;
        }

        public ComponenteInfo GetById(int id)
        {
            ComponenteInfo componente = new ComponenteInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                componente = JsonConvert.DeserializeObject<List<ComponenteInfo>>(fileText).Find(x => x.IdComponente == id);  
            }
            else
            {
                DalComponentes dalComponentes = new DalComponentes();
                componente = dalComponentes.GetById(id);
            }

            return componente;
        }

        public bool Insert(ComponenteInfo componenteInfo)
        {         
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<ComponenteInfo> lstComponentes = JsonConvert.DeserializeObject<List<ComponenteInfo>>(fileText).OrderBy(x => x.IdComponente).ToList();

                int lastId = lstComponentes.Last().IdComponente;

                lstComponentes.Add(new ComponenteInfo
                {
                    IdComponente = lastId + 1,
                    CodigoSAP = componenteInfo.CodigoSAP,
                    TipoMaterial = componenteInfo.TipoMaterial,
                    Descricao = componenteInfo.Descricao,
                    Modelo = componenteInfo.Modelo,
                    Ativo = componenteInfo.Ativo,
                    Sequencia = componenteInfo.Sequencia,
                    Posto = componenteInfo.Posto
                });

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstComponentes));
            }
            else
            {
                DalComponentes dalComponentes = new DalComponentes();
                retorno = dalComponentes.Insert(componenteInfo);
            }

            return retorno;
        }

        public bool Update(ComponenteInfo componenteInfo)
        {           
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<ComponenteInfo> lstComponentes = JsonConvert.DeserializeObject<List<ComponenteInfo>>(fileText);

                lstComponentes.Find(x => x.IdComponente == componenteInfo.IdComponente).TipoMaterial = componenteInfo.TipoMaterial;
                lstComponentes.Find(x => x.IdComponente == componenteInfo.IdComponente).CodigoSAP = componenteInfo.CodigoSAP;
                lstComponentes.Find(x => x.IdComponente == componenteInfo.IdComponente).Descricao = componenteInfo.Descricao;
                lstComponentes.Find(x => x.IdComponente == componenteInfo.IdComponente).Posto = componenteInfo.Posto;
                lstComponentes.Find(x => x.IdComponente == componenteInfo.IdComponente).Sequencia = componenteInfo.Sequencia;
                lstComponentes.Find(x => x.IdComponente == componenteInfo.IdComponente).Ativo = componenteInfo.Ativo;
                lstComponentes.Find(x => x.IdComponente == componenteInfo.IdComponente).Modelo = componenteInfo.Modelo;

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstComponentes));
            }
            else
            {
                DalComponentes dalComponentes = new DalComponentes();
                retorno = dalComponentes.Update(componenteInfo);
            }

            return retorno;        
        }

        public bool Delete(int id)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<ComponenteInfo> lstComponentes = JsonConvert.DeserializeObject<List<ComponenteInfo>>(fileText);

                lstComponentes.RemoveAll(x => x.IdComponente == id);
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstComponentes));
            }
            else
            {
                DalComponentes dalComponentes = new DalComponentes();
                retorno = dalComponentes.Delete(id);
            }

            return retorno;
        }
    }
}