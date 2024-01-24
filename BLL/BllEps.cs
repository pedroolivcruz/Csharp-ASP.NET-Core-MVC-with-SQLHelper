using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Newtonsoft.Json;
using System.Linq;

namespace Conectasys.Portal.BLL
{
    public class BllEps
    {
        string fileName = Config.RootFolder + "\\Repositories\\Eps.json";


        public List<EpsInfo> GetAll()
        {
            List<EpsInfo> lstEps = new List<EpsInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<EpsInfo>>(fileText);
                foreach(var eps in data)
                {
                    eps.DoubleCodigoEps = Convert.ToDouble(eps.StringCodigoEps);
                }

                lstEps = data.OrderBy(x => x.DoubleCodigoEps).ToList();
            }
            else
            {
                DalEps dalEps = new DalEps();
                lstEps = dalEps.GetAll();
            }

            return lstEps;
        }

        public List<EpsInfo> GetAllByCodigo(string codigo)
        {
            List<EpsInfo> lstEps = new List<EpsInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<EpsInfo>>(fileText).Where(x => x.StringCodigoEps.Contains(codigo));
                foreach (var eps in data)
                {
                    eps.DoubleCodigoEps = Convert.ToDouble(eps.StringCodigoEps);
                    eps.StringCodigoEps.ToString().Replace(",", ".");
                }

                lstEps = data.OrderBy(x => x.DoubleCodigoEps).ToList();
            }
            else
            {
                DalEps dalEps = new DalEps();
                lstEps = dalEps.GetAllByCodigo(codigo);
            }

            return lstEps;
        }

        public EpsInfo GetByCodigo(double codigo)
        {
            EpsInfo eps = new EpsInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                eps = JsonConvert.DeserializeObject<List<EpsInfo>>(fileText).Find(x => x.StringCodigoEps == codigo.ToString().Replace(",","."));
                eps.DoubleCodigoEps = Convert.ToDouble(eps.StringCodigoEps);
                eps.StringCodigoEps.ToString().Replace(",", ".");
            }
            else
            {
                DalEps dalEps = new DalEps();
                eps = dalEps.GetByCodigo(codigo);
            }

            return eps;
        }

        public bool Insert(EpsInfo cadastroEps)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<EpsInfo> lstEps = JsonConvert.DeserializeObject<List<EpsInfo>>(fileText);

                if (!lstEps.Where(x => x.StringCodigoEps == cadastroEps.DoubleCodigoEps.ToString().Replace(",", ".")).Any())
                {
                    lstEps.Add(new EpsInfo
                    {   
                        StringCodigoEps = cadastroEps.DoubleCodigoEps.ToString().Replace(",","."),
                        CorrenteMinima = cadastroEps.CorrenteMinima,
                        CorrenteMaxima = cadastroEps.CorrenteMaxima,
                        TensaoMinima = cadastroEps.TensaoMinima,
                        TensaoMaxima = cadastroEps.TensaoMaxima
                    });
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstEps));
                }
                else retorno = false;             
            }
            else
            {
                DalEps dalEps = new DalEps();
                retorno = dalEps.Insert(cadastroEps);
            }

            return retorno;
        }

        public bool Update(double codigoAntigo, EpsInfo cadastroEps)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<EpsInfo> lstEps = JsonConvert.DeserializeObject<List<EpsInfo>>(fileText);

                if (!lstEps.Where(x => x.StringCodigoEps == cadastroEps.DoubleCodigoEps.ToString().Replace(",", ".")).Any())
                {
                    lstEps.Find(x => x.StringCodigoEps == cadastroEps.DoubleCodigoEps.ToString().Replace(",", ".")).CorrenteMinima = cadastroEps.CorrenteMinima;
                    lstEps.Find(x => x.StringCodigoEps == cadastroEps.DoubleCodigoEps.ToString().Replace(",", ".")).CorrenteMaxima = cadastroEps.CorrenteMaxima;
                    lstEps.Find(x => x.StringCodigoEps == cadastroEps.DoubleCodigoEps.ToString().Replace(",", ".")).TensaoMinima = cadastroEps.TensaoMinima;
                    lstEps.Find(x => x.StringCodigoEps == cadastroEps.DoubleCodigoEps.ToString().Replace(",", ".")).TensaoMaxima = cadastroEps.TensaoMaxima;

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstEps));
                }
                else retorno = false;
            }
            else
            {
                DalEps dalEps = new DalEps();
                retorno = dalEps.UpdateEps(codigoAntigo, cadastroEps);
            }

            return retorno;
        }

        public bool Delete(double codigo)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<EpsInfo> lstEps = JsonConvert.DeserializeObject<List<EpsInfo>>(fileText);

                lstEps.RemoveAll(x => x.StringCodigoEps == codigo.ToString().Replace(",","."));
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstEps));
            }
            else
            {
                DalEps dalEps = new DalEps();
                retorno = dalEps.Delete(codigo);
            }

            return retorno;
        }
    }
}