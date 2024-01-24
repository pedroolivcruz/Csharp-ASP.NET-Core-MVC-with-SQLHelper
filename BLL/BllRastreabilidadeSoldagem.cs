using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Conectasys.Portal.RepositoriesModels;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;


namespace Conectasys.Portal.BLL
{
    public class BllRastreabilidadeSoldagem
    {
        string fileNameChecklists = Config.RootFolder + "\\Repositories\\RastreabilidadeChecklistsSoldagem.json";
        string fileNameRastreabilidadeSoldagem = Config.RootFolder + "\\Repositories\\RastreabilidadeSoldagem.json";
        string fileNameGraficosSoldagem = Config.RootFolder + "\\Repositories\\vwRastreabilidadeGraficosSoldagem.json";
        string fileNameCordoesSoldagem = Config.RootFolder + "\\Repositories\\vwRastreabilidadeCordoesSoldagem.json";
        string fileNameCordoes = Config.RootFolder + "\\Repositories\\Cordoes.json";


        public RastreabilidadeSoldagemInfo GetDatesByCodigo(string codigo)
        {
            RastreabilidadeSoldagemInfo rastreabilidade = new RastreabilidadeSoldagemInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeSoldagem);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeSoldagemInfo>>(fileText);

                rastreabilidade.Rastreabilidade = data.Where(x => x.Rastreabilidade == codigo).DistinctBy(x => x.Rastreabilidade).First().Rastreabilidade;
                rastreabilidade.DataInicio = data.Where(x => x.Rastreabilidade == codigo).OrderBy(x => x.DataInicio).First().DataInicio;
                rastreabilidade.DataFim = data.Where(x => x.Rastreabilidade == codigo).OrderByDescending(x => x.DataFim).First().DataFim;
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                rastreabilidade = dalRastreabilidade.GetDatesByCodigo(codigo);
            }

            return rastreabilidade;
        }

        public List<RastreabilidadeSoldagemInfo> GetAllRastreabilidadesByCodigo(string codigo)
        {
            List<RastreabilidadeSoldagemInfo> lstRastreabilidade = new List<RastreabilidadeSoldagemInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeSoldagem);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeSoldagemInfo>>(fileText);

                lstRastreabilidade = data.Where(x => x.Rastreabilidade == codigo).OrderBy(x => x.Posto).ToList();
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                lstRastreabilidade = dalRastreabilidade.GetAllRastreabilidadesByCodigo(codigo);
            }

            return lstRastreabilidade;
        }

        public List<string> GetAllRastreabilidadesByData(DateTime dataInicio, DateTime dataFim)
        {
            List<string> lstRastreabilidades = new List<string>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeSoldagem);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeSoldagemInfo>>(fileText);

                lstRastreabilidades = data.Where(x => x.DataInicio >= dataInicio && x.DataFim <= dataFim && x.DataFim > dataInicio).DistinctBy(x => x.Rastreabilidade).Select(x => x.Rastreabilidade).ToList();
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                lstRastreabilidades = dalRastreabilidade.GetAllRastreabilidadesByData(dataInicio, dataFim);
            }

            return lstRastreabilidades;
        }

        public List<string> GetSimilarRastreabilidadesByCodigo(string codigo)
        {
            List<string> lstRastreabilidades = new List<string>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameRastreabilidadeSoldagem);
                var data = JsonConvert.DeserializeObject<List<RastreabilidadeSoldagemInfo>>(fileText);

                lstRastreabilidades = data.Where(x => x.Rastreabilidade.Contains(codigo)).DistinctBy(x => x.Rastreabilidade).Select(x => x.Rastreabilidade).ToList();
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                lstRastreabilidades = dalRastreabilidade.GetSimilarRastreabilidadesByCodigo(codigo);
            }

            return lstRastreabilidades;
        }

        public List<int> GetCordoesByPosto(string codigo, int posto)
        {
            List<int> lstCordoes = new List<int>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameGraficosSoldagem);
                var data = JsonConvert.DeserializeObject<List<JsonRastreabilidadeGraficosSoldagemInfo>>(fileText);

                lstCordoes = data.Where(x => x.Rastreabilidade == codigo && x.Posto == posto).DistinctBy(x => x.Cordao).OrderBy(x => x.Cordao).Select(x => x.Cordao).ToList();
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                lstCordoes = dalRastreabilidade.GetCordoesByPosto(codigo, posto);
            }

            return lstCordoes;
        }

        public bool ExisteSequenciaChecklist(string codigo, int posto, int sequencia)
        {
            bool hasSequencia = false;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameChecklists);
                var data = JsonConvert.DeserializeObject<List<JsonRastreabilidadeChecklistsSoldagemInfo>>(fileText);

                hasSequencia = data.Where(x => x.Codigo == codigo && x.Posto == posto && x.Sequencia == sequencia).Any();
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                hasSequencia = dalRastreabilidade.ExisteSequenciaChecklist(codigo, posto, sequencia);
            }

            return hasSequencia;
        }

        public bool ObterInfoGrafico(string codigo,int posto, ref List<RelatorioGraficoInfo> lstGraficos)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameGraficosSoldagem);

                List<JsonRastreabilidadeGraficosSoldagemInfo> lista = JsonConvert.DeserializeObject<List<JsonRastreabilidadeGraficosSoldagemInfo>>(fileText)
                                                                       .Where(x => x.Rastreabilidade == codigo && x.Posto == posto)
                                                                       .OrderBy(x => x.Cordao)
                                                                       .ToList();

                foreach(var item in lista)
                {
                    lstGraficos.Add(new RelatorioGraficoInfo
                    {
                        sinal = item.IdCordao,
                        tensao = item.Tensao,
                        corrente = item.Corrente,
                        cordao = item.Cordao
                    });
                }
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                retorno = dalRastreabilidade.ObterInfoGraficos(codigo, posto, ref lstGraficos);
            }

            return retorno;
        }

        public bool ObterInfoCordoes(int posto, ref List<RelatorioCordaoInfo> lstCordoes)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameCordoesSoldagem);
                var data = JsonConvert.DeserializeObject<List<RelatorioCordaoInfo>>(fileText).DistinctBy(x => x.Cordao).OrderBy(x => x.Cordao);

                lstCordoes = data.ToList();
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                retorno = dalRastreabilidade.ObterInfoCordoes(posto,ref lstCordoes);
            }

            return retorno;
        }

        public string ObterFotoCordao(int cordao)
        {
            string stringFoto = string.Empty;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileNameCordoes);

                stringFoto = JsonConvert.DeserializeObject<List<CordaoInfo>>(fileText).Where(x => x.Cordao == cordao).First().StringFoto;
            }
            else
            {
                DalRastreabilidadeSoldagem dalRastreabilidade = new DalRastreabilidadeSoldagem();
                stringFoto = dalRastreabilidade.ObterFotoCordao(cordao);
            }

            return stringFoto;
        }
    }
}