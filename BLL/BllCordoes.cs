using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;


namespace Conectasys.Portal.BLL
{
    public class BllCordoes
    {
        string fileName = Config.RootFolder + "\\Repositories\\Cordoes.json";


        public List<CordaoInfo> GetAllByPosto(int posto)
        {
            List<CordaoInfo> lstCordoes = new List<CordaoInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText);

                lstCordoes = data.Where(x => x.Posto == posto).OrderBy(x => x.Cordao).ToList();
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                lstCordoes =  dalCordoes.GetAllByPosto(posto);
            }

            return lstCordoes;         
        }

        public List<CordaoInfo> GetAllByCordao(int cordao)
        {
            List<CordaoInfo> lstCordoes = new List<CordaoInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText);

                lstCordoes = data.Where(x => x.Cordao.ToString().Contains(cordao.ToString())).OrderBy(x => x.Cordao).ToList();
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                lstCordoes = dalCordoes.GetAllByCordao(cordao);
            }

            return lstCordoes;
        }

        public List<CordaoInfo> GetAllByEps(double eps)
        {
            List<CordaoInfo> lstCordoes = new List<CordaoInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText);

                lstCordoes = data.Where(x => x.CodigoEps.ToString().Contains(eps.ToString())).OrderBy(x => x.Cordao).ToList(); 
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                lstCordoes = dalCordoes.GetAllByEps(eps);
            }

            return lstCordoes;
        }

        public CordaoInfo GetByCordao(int cordao)
        {
            CordaoInfo cordaoInfo = new CordaoInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                cordaoInfo = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText).Find(x => x.Cordao == cordao);
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                cordaoInfo = dalCordoes.GetByCordao(cordao);
            }
            return cordaoInfo;
        }

        public int GetLastCordao()
        {       
            int lastCordao = 0;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<CordaoInfo> lstCordoes = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText).OrderBy(x => x.Cordao).ToList();
                lastCordao = lstCordoes.Last().Cordao;
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                lastCordao = dalCordoes.GetLastCordao();
            }

            return lastCordao;
        }

        public bool Insert(CordaoInfo cadastroCordaoInfo)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<CordaoInfo> lstCordoes = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText).OrderBy(x => x.Cordao).ToList();

                if (!lstCordoes.Where(x => x.Cordao == cadastroCordaoInfo.Cordao).Any())
                {

                    lstCordoes.Add(new CordaoInfo
                    {   
                        IdCordao = cadastroCordaoInfo.IdCordao,
                        Cordao = cadastroCordaoInfo.Cordao,
                        Posto = cadastroCordaoInfo.Posto,
                        CodigoEps = cadastroCordaoInfo.CodigoEps,
                        Descricao = cadastroCordaoInfo.Descricao,
                        StringFoto =  "data:image/png;base64," + Convert.ToBase64String(cadastroCordaoInfo.Foto, 0, cadastroCordaoInfo.Foto.Length)
                    });

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstCordoes));
                }
                else retorno = false;
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                retorno = dalCordoes.Insert(cadastroCordaoInfo);
            }

            return retorno;
        }

        public bool Update(int codigoAntigo, CordaoInfo cadastroCordaoInfo)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<CordaoInfo> lstCordoes = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText).OrderBy(x => x.Cordao).ToList();

                if (cadastroCordaoInfo.Cordao == codigoAntigo)
                {
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).Cordao = cadastroCordaoInfo.Cordao;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).Descricao = cadastroCordaoInfo.Descricao;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).CodigoEps = cadastroCordaoInfo.CodigoEps;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).Posto = cadastroCordaoInfo.Posto;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).StringFoto = "data:image/png;base64," + Convert.ToBase64String(cadastroCordaoInfo.Foto, 0, cadastroCordaoInfo.Foto.Length);

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstCordoes));
                }
                else if (!lstCordoes.Where(x => x.Cordao == cadastroCordaoInfo.Cordao).Any())
                {
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).Cordao = cadastroCordaoInfo.Cordao;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).Descricao = cadastroCordaoInfo.Descricao;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).CodigoEps = cadastroCordaoInfo.CodigoEps;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).Posto = cadastroCordaoInfo.Posto;
                    lstCordoes.Find(x => x.Cordao == codigoAntigo).StringFoto = "data:image/png;base64," + Convert.ToBase64String(cadastroCordaoInfo.Foto, 0, cadastroCordaoInfo.Foto.Length);

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstCordoes));
                }
                else retorno = false;
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                retorno = dalCordoes.Update(codigoAntigo, cadastroCordaoInfo);
            }

            return retorno;    
        }

        public bool Delete(int cordao)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<CordaoInfo> lstCordoes = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText);

                lstCordoes.RemoveAll(x => x.Cordao == cordao);
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstCordoes));
            }
            else
            {
                DalCordoes dalCordoes = new DalCordoes();
                retorno = dalCordoes.Delete(cordao);
            }

            return retorno;
        }
    }
}