using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Conectasys.Portal.RepositoriesModels;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;


namespace Conectasys.Portal.BLL
{
    public class BllRastreabilidadeMontagem
    {
        string fileNameComponentes = Config.RootFolder + "\\Repositories\\RastreabilidadeComponentesMontagem.json";
        string fileNameChecklists = Config.RootFolder + "\\Repositories\\RastreabilidadeChecklistsMontagem.json";
        string fileNameRastreabilidadeMontagem = Config.RootFolder + "\\Repositories\\RastreabilidadeMontagem.json";
        string fileNameTesteFinal = Config.RootFolder + "\\Repositories\\RastreabilidadeTesteFinal.json";
        string fileNameTorques = Config.RootFolder + "\\Repositories\\RastreabilidadeTorquesMontagem.json";
        string fileNameRelacionaFotoTorques= Config.RootFolder + "\\Repositories\\RelacionaFotoTorques.json";
        string fileNameCadastroFotoTorques = Config.RootFolder + "\\Repositories\\FotoTorques.json";


        public List<RastreabilidadeMontagemInfo> GetAllRastreabilidadesByCodigo(string codigo)
        {
            List<RastreabilidadeMontagemInfo> lstRastreabilidades = new List<RastreabilidadeMontagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeMontagem);
                lstRastreabilidades = JsonConvert.DeserializeObject<List<RastreabilidadeMontagemInfo>>(fileText).Where(x => x.Rastreabilidade == codigo).OrderBy(x => x.Posto).ToList();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstRastreabilidades = dalRastreabilidade.GetAllRastreabilidadesByCodigo(codigo);
            }

            return lstRastreabilidades;
        }

        public int CountPostosFinished(string codigo)
        {
            int numPostosConcluidos = 0;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeMontagem);               
                numPostosConcluidos = JsonConvert.DeserializeObject<List<RastreabilidadeMontagemInfo>>(fileText).Where(x => x.Rastreabilidade == codigo && x.Status == 2).DistinctBy(x => x.Posto).Count();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                numPostosConcluidos = dalRastreabilidade.CountPostosFinished(codigo);
            }

            return numPostosConcluidos;
        }

        public RastreabilidadeMontagemInfo GetDatesByCodigo(string codigo)
        {
            RastreabilidadeMontagemInfo rastreabilidade = new RastreabilidadeMontagemInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeMontagem);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeMontagemInfo>>(fileText);

                rastreabilidade.Rastreabilidade = data.Where(x => x.Rastreabilidade == codigo).DistinctBy(x => x.Rastreabilidade).First().Rastreabilidade;
                rastreabilidade.DataInicio = data.Where(x => x.Rastreabilidade == codigo).OrderBy(x => x.DataInicio).First().DataInicio;
                rastreabilidade.DataFim = data.Where(x => x.Rastreabilidade == codigo).OrderByDescending(x => x.DataFim).First().DataFim;
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                rastreabilidade = dalRastreabilidade.GetDatesByCodigo(codigo);
            }

            return rastreabilidade;
        }

        public List<string> GetAllRastreabilidadesByData(DateTime dataInicio, DateTime dataFim)
        {   
            List<string> lstRastreabilidades = new List<string>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeMontagem);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeMontagemInfo>>(fileText);

                lstRastreabilidades = data.Where(x => x.DataInicio >= dataInicio && x.DataFim <= dataFim && x.DataFim > dataInicio).DistinctBy(x => x.Rastreabilidade).Select(x => x.Rastreabilidade).ToList();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstRastreabilidades = dalRastreabilidade.GetAllRastreabilidadesByData(dataInicio, dataFim);
            }

            return lstRastreabilidades;
        }

        public List<string> GetSimilarRastreabilidadesByCodigo(string codigo)
        {
            List<string> lstRastreabilidades = new List<string>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeMontagem);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeMontagemInfo>>(fileText);

                lstRastreabilidades = data.Where(x => x.Rastreabilidade.Contains(codigo)).DistinctBy(x => x.Rastreabilidade).Select(x => x.Rastreabilidade).ToList();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstRastreabilidades = dalRastreabilidade.GetSimilarRastreabilidadesByCodigo(codigo);
            }

            return lstRastreabilidades;
        }

        public List<RastreabilidadeComponentesMontagemInfo> GetAllComponentesByCodigo(string codigo)
        {
            List<RastreabilidadeComponentesMontagemInfo> lstComponentes = new List<RastreabilidadeComponentesMontagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameComponentes);
                var data = JsonConvert.DeserializeObject<List<JsonRastreabilidadeComponentesMontagemInfo>>(fileText).Where(x => x.Codigo == codigo).OrderBy(x => x.Posto);

                foreach(var item in data)
                {
                    lstComponentes.Add(new RastreabilidadeComponentesMontagemInfo
                    {
                        CodigoComponente = item.CodigoComponente,
                        Descricao = item.Descricao
                    });
                }
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstComponentes = dalRastreabilidade.GetAllComponentesByCodigo(codigo);
            }

            return lstComponentes;
        }

        public List<TorqueInfo> GetTorquesRala(string codigo)
        {   
            List<TorqueInfo> lstTorques = new List<TorqueInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameTorques);
                lstTorques = JsonConvert.DeserializeObject<List<TorqueInfo>>(fileText).Where(x => x.Codigo == codigo && x.Descricao == "Rala").OrderBy(x => x.Data).ToList();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstTorques = dalRastreabilidade.GetTorquesRala(codigo);
            }

            return lstTorques;
        }

        public List<TorqueInfo> GetTorquesComponentes(string codigo)
        {
            List<TorqueInfo> lstTorques = new List<TorqueInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameTorques);
                lstTorques = JsonConvert.DeserializeObject<List<TorqueInfo>>(fileText).Where(x => x.Codigo == codigo && x.Descricao == "Componentes").OrderBy(x => x.Data).ToList();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstTorques = dalRastreabilidade.GetTorquesComponentes(codigo);
            }

            return lstTorques;
        }

        public List<TesteFinalInfo> GetTesteFinal(string codigo)
        {
            List<TesteFinalInfo> lstTesteFinal = new List<TesteFinalInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameTesteFinal);
                var data = JsonConvert.DeserializeObject<List<JsonRastreabilidadeTesteFinalInfo>>(fileText).Where(x => x.Rastreabilidade == codigo).OrderBy(x => x.Data);

                foreach(var item in data)
                {
                    lstTesteFinal.Add(new TesteFinalInfo
                    {
                        Velocidade = item.Velocidade,
                        Forca = item.Forca,
                        Angulo = item.Angulo
                    });
                }
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstTesteFinal = dalRastreabilidade.GetTesteFinal(codigo);
            }
            
            return lstTesteFinal;
        }

        public List<TesteFinalInfo> GetEsquerdaTesteFinal(string codigo)
        {
            List<TesteFinalInfo> lstTesteFinal = new List<TesteFinalInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameTesteFinal);
                var data = JsonConvert.DeserializeObject<List<JsonRastreabilidadeTesteFinalInfo>>(fileText).Where(x => x.Rastreabilidade == codigo && x.Angulo >= -3 && x.Angulo <= 3).OrderBy(x => x.Data).ToList();
                int count = data.Count();
                data = data.Take(count / 2).ToList();

                foreach (var item in data)
                {
                    lstTesteFinal.Add(new TesteFinalInfo
                    {
                        Velocidade = item.Velocidade,
                        Forca = item.Forca,
                        Angulo = item.Angulo
                    });
                }
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstTesteFinal = dalRastreabilidade.GetEsquerdaTesteFinal(codigo);
            }

            return lstTesteFinal;
        }

        public List<TesteFinalInfo> GetDireitaTesteFinal(string codigo)
        {
            List<TesteFinalInfo> lstTesteFinal = new List<TesteFinalInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameTesteFinal);
                var data = JsonConvert.DeserializeObject<List<JsonRastreabilidadeTesteFinalInfo>>(fileText).Where(x => x.Rastreabilidade == codigo && x.Angulo <= 3 && x.Angulo >= -3).OrderByDescending(x => x.Data).ToList();
                int count = data.Count();
                data = data.Take(count / 2).ToList();

                foreach (var item in data)
                {
                    lstTesteFinal.Add(new TesteFinalInfo
                    {
                        Velocidade = item.Velocidade,
                        Forca = item.Forca,
                        Angulo = item.Angulo
                    });
                }
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstTesteFinal = dalRastreabilidade.GetDireitaTesteFinal(codigo);
            }

            return lstTesteFinal;
        }

        public List<string> SearchComponente(string codigo)
        {
            List<string> lstComponentes = new List<string>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameComponentes);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeComponentesMontagemInfo>>(fileText);

                lstComponentes = data.Where(x => x.CodigoComponente.Contains(codigo)).Select(x => x.CodigoComponente).ToList();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                lstComponentes = dalRastreabilidade.SearchComponente(codigo);
            }
            return lstComponentes;
        }

        public int GetIdFromFotoTorques(string rastreabilidade, string descricao)
        {
            int idFoto = 0;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRelacionaFotoTorques);
                var data = JsonConvert.DeserializeObject<List<JsonRelacionaFotoTorquesInfo>>(fileText).Where(x => x.Descricao == descricao && x.Codigo == rastreabilidade).ToList();

                if(data.Count > 0)
                {
                    idFoto = data.First().IdFoto;
                }  
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                idFoto = dalRastreabilidade.GetIdFromFotoTorques(rastreabilidade, descricao);
            }

            return idFoto;
        }

        public FotoTorquesInfo GetFotoTorquesById(int id)
        {
            FotoTorquesInfo fotoTorques = new FotoTorquesInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameCadastroFotoTorques);
                fotoTorques = JsonConvert.DeserializeObject<List<FotoTorquesInfo>>(fileText).Find(x => x.IdFoto == id);
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                fotoTorques = dalRastreabilidade.GetFotoTorquesById(id);
            }

            return fotoTorques;
        }

        public bool HasSequenciaChecklist(string codigo, int posto, int sequencia)
        {
            bool hasSequencia = false;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameChecklists);
                var data = JsonConvert.DeserializeObject<List<JsonRastreabilidadeChecklistsMontagemInfo>>(fileText);

                hasSequencia = data.Where(x => x.Codigo == codigo && x.Posto == posto && x.Sequencia == sequencia).Any();
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                hasSequencia = dalRastreabilidade.HasSequenciaChecklist(codigo, posto, sequencia);
            }
            return hasSequencia;
        }

        public string GetLastRastreabilidade()
        {
            string codigo = string.Empty; 

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameTesteFinal);
                codigo = JsonConvert.DeserializeObject<List<JsonRastreabilidadeTesteFinalInfo>>(fileText).OrderBy(x => x.Data).First().Rastreabilidade;
            }
            else
            {
                DalRastreabilidadeMontagem dalRastreabilidade = new DalRastreabilidadeMontagem();
                codigo = dalRastreabilidade.GetLastRastreabilidade();
            }

            return codigo;
        }
    }
}