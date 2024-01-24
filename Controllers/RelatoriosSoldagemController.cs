using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;


namespace Conectasys.Portal.Controllers
{
    public class RelatoriosSoldagemController : Controller
    {
        BllChecklistSoldagem bllChecklistsSoldagem = new BllChecklistSoldagem();
        BllRastreabilidadeSoldagem bllRastreabilidade = new BllRastreabilidadeSoldagem();
        BllCordoes bllCordoes = new BllCordoes();
        BllEps bllEps = new BllEps();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Rastreabilidades(string r, string dI, string dF)
        {
            int index = 0;

            List<string> lstRastreabilidadesInfo = new List<string>();
            List<RastreabilidadeSoldagemInfo> lstRastreabilidades = new List<RastreabilidadeSoldagemInfo>();

            DateTime dInicio = new DateTime();
            DateTime dFim = new DateTime();

            if (dI != string.Empty && dF != string.Empty && dI != null && dF != null)
            {
                dInicio = Convert.ToDateTime(dI);
                dFim = Convert.ToDateTime(dF);
            }
            else if (Config.IsDemostration)
            {
                dInicio = new DateTime(22, 3, 3);
                dFim = DateTime.Today.AddDays(1);
            }
            else
            {
                dInicio = DateTime.Today.AddDays(-1);
                dFim = DateTime.Today.AddDays(1);
            }

            if (r != string.Empty && r != null) lstRastreabilidadesInfo = bllRastreabilidade.GetSimilarRastreabilidadesByCodigo(r);
            else  lstRastreabilidadesInfo = bllRastreabilidade.GetAllRastreabilidadesByData(dInicio, dFim);

            foreach (string codigo in lstRastreabilidadesInfo)
            {
                RastreabilidadeSoldagemInfo rastreabilidadeInfo = bllRastreabilidade.GetDatesByCodigo(codigo);

                index++;
                rastreabilidadeInfo.Index = index;

                int count = 0;

                List<RastreabilidadeSoldagemInfo> lstInfoRastreabilidade = bllRastreabilidade.GetAllRastreabilidadesByCodigo(codigo);

                foreach (var item in lstInfoRastreabilidade)
                {
                    if (item.Status != 2) count++;
                }

                if (codigo.Contains("F"))
                {
                    if (count > 0 || lstInfoRastreabilidade.Count != 2) rastreabilidadeInfo.Descricao = "Não Concluído";
                    else rastreabilidadeInfo.Descricao = "Concluído";
                }
                else
                {
                    if (count > 0 || lstInfoRastreabilidade.Count != 3) rastreabilidadeInfo.Descricao = "Não Concluído";
                    else rastreabilidadeInfo.Descricao = "Concluído";
                }

                lstRastreabilidades.Add(rastreabilidadeInfo);
            }

            ViewBag.Rastreabilidade = r;
            ViewBag.DataFim = dFim.ToString("yyyy-MM-dd HH:mm");
            ViewBag.DataInicio = dInicio.ToString("yyyy-MM-dd HH:mm");

            return View("~/Views/RelatoriosSoldagem/Rastreabilidades.cshtml", lstRastreabilidades);
        }

        public ActionResult RelatorioFrontal(string rastreabilidade, string rastreabilidadePesquisa, string dataInicio, string dataFim)
        {
            dynamic model = new ExpandoObject();

            model.CodigoRastreabilidade = rastreabilidade;

            List<RastreabilidadeSoldagemInfo> lstRastreabilidades = bllRastreabilidade.GetAllRastreabilidadesByCodigo(rastreabilidade);
            model.ListaInfoRastreabilidade = lstRastreabilidades;

            int count = 0;

            foreach (var item in lstRastreabilidades) if (item.Status != 2) count++;

            if (count > 0 || lstRastreabilidades.Count != 2) model.StatusRastreabilidade = "Não Concluído";
            else model.StatusRastreabilidade = "Concluído";

            model.ListaInfoRastreabilidadePosto1 = lstRastreabilidades.Where(i => i.Posto == 1).First();
            model.ListaInfoRastreabilidadePosto5 = lstRastreabilidades.Where(i => i.Posto == 5).First();

            List<RelatorioSoldagemInfo> lstInfoSoldagemPosto5 = new List<RelatorioSoldagemInfo>();
            List<int> lstCordoesPosto5 = bllRastreabilidade.GetCordoesByPosto(rastreabilidade, 5);

            List<RelatorioGraficoInfo> lstInfoGraficosPosto5 = new List<RelatorioGraficoInfo>(); 
            bllRastreabilidade.ObterInfoGrafico(rastreabilidade,5, ref lstInfoGraficosPosto5);

            List<RelatorioCordaoInfo> lstInfoCordoesPosto5 = new List<RelatorioCordaoInfo>();
            bllRastreabilidade.ObterInfoCordoes(5, ref lstInfoCordoesPosto5);

            foreach (var item in lstCordoesPosto5)
            {
                RelatorioSoldagemInfo graficoInfo = new RelatorioSoldagemInfo();
                RelatorioCordaoInfo infoCordao = lstInfoCordoesPosto5.Where(cordao => cordao.Cordao == item).First();
                
                graficoInfo.cordao = item;
                graficoInfo.sinais = lstInfoGraficosPosto5.Where(cordao => cordao.cordao == item).Select(grp => grp.sinal).ToList();
                graficoInfo.corrente = lstInfoGraficosPosto5.Where(cordao => cordao.cordao == item).Select(grp => grp.corrente).ToList();
                graficoInfo.tensao = lstInfoGraficosPosto5.Where(cordao => cordao.cordao == item).Select(grp => grp.tensao).ToList();
                graficoInfo.codigoEps = infoCordao.CodigoEps;
                graficoInfo.correnteMinima = infoCordao.CorrenteMinima;
                graficoInfo.correnteMaxima = infoCordao.CorrenteMaxima;
                graficoInfo.tensaoMinima = infoCordao.TensaoMinima;
                graficoInfo.tensaoMaxima = infoCordao.TensaoMaxima;
                graficoInfo.stringFoto = bllRastreabilidade.ObterFotoCordao(item);

                lstInfoSoldagemPosto5.Add(graficoInfo);
            }

            double numPaginasCordoesPosto5 = lstCordoesPosto5.Count / 4;
            model.QuantidadePaginasCordoesPosto5 = Math.Floor(numPaginasCordoesPosto5);
            model.QuantidadeExtraCordoesPosto5 = lstCordoesPosto5.Count % 4;
            model.ListaInfoGraficosPosto5 = lstInfoSoldagemPosto5;

            model.ListaChecklistsPosto1 = bllChecklistsSoldagem.GetAllByPosto(1);
            model.ListaChecklistsPosto5 = bllChecklistsSoldagem.GetAllByPosto(5);

            model.RastreabilidadePesquisa = rastreabilidadePesquisa;
            model.DataInicio = dataInicio;
            model.DataFim = dataFim;

            return View("~/Views/RelatoriosSoldagem/RelatorioFrontal.cshtml", model);
        }

        public ActionResult RelatorioTraseira(string rastreabilidade, string rastreabilidadePesquisa, string dataInicio, string dataFim)
        {
            dynamic model = new ExpandoObject();

            model.CodigoRastreabilidade = rastreabilidade;

            List<RastreabilidadeSoldagemInfo> lstRastreabilidades = bllRastreabilidade.GetAllRastreabilidadesByCodigo(rastreabilidade);
            model.ListaInfoRastreabilidade = lstRastreabilidades;

            int count = 0;

            foreach (var item in lstRastreabilidades) if (item.Status != 2) count++;

            model.ListaInfoRastreabilidadePosto2 = lstRastreabilidades.Where(i => i.Posto == 2).First();
            model.ListaInfoRastreabilidadePosto3 = lstRastreabilidades.Where(i => i.Posto == 3).First();
            model.ListaInfoRastreabilidadePosto4 = lstRastreabilidades.Where(i => i.Posto == 4).First();

            List<RelatorioSoldagemInfo> lstInfoSoldagemPosto3 = new List<RelatorioSoldagemInfo>();
            List<int> lstCordoesPosto3 = bllRastreabilidade.GetCordoesByPosto(rastreabilidade,3);

            List<RelatorioGraficoInfo> lstInfoGraficosPosto3 = new List<RelatorioGraficoInfo>();
            bllRastreabilidade.ObterInfoGrafico(rastreabilidade,3, ref lstInfoGraficosPosto3);

            List<RelatorioCordaoInfo> lstInfoCordoesPosto3 = new List<RelatorioCordaoInfo>();
            bllRastreabilidade.ObterInfoCordoes(3,ref lstInfoCordoesPosto3);

            foreach (var item in lstCordoesPosto3)
            {
                RelatorioSoldagemInfo graficoInfo = new RelatorioSoldagemInfo();
                RelatorioCordaoInfo infoCordao = lstInfoCordoesPosto3.Where(cordao => cordao.Cordao == item).First();

                graficoInfo.cordao = item;
                graficoInfo.sinais = lstInfoGraficosPosto3.Where(cordao => cordao.cordao == item).Select(grp => grp.sinal).ToList();
                graficoInfo.corrente = lstInfoGraficosPosto3.Where(cordao => cordao.cordao == item).Select(grp => grp.corrente).ToList();
                graficoInfo.tensao = lstInfoGraficosPosto3.Where(cordao => cordao.cordao == item).Select(grp => grp.tensao).ToList();
                graficoInfo.codigoEps = infoCordao.CodigoEps;
                graficoInfo.correnteMinima = infoCordao.CorrenteMinima;
                graficoInfo.correnteMaxima = infoCordao.CorrenteMaxima;
                graficoInfo.tensaoMinima = infoCordao.TensaoMinima;
                graficoInfo.tensaoMaxima = infoCordao.TensaoMaxima;
                graficoInfo.stringFoto = bllRastreabilidade.ObterFotoCordao(item);

                lstInfoSoldagemPosto3.Add(graficoInfo);
            }

            double numPaginasCordoesPosto3 = lstCordoesPosto3.Count / 4;
            model.QuantidadePaginasCordoesPosto3 = Math.Floor(numPaginasCordoesPosto3);
            model.QuantidadeExtraCordoesPosto3 = lstCordoesPosto3.Count % 4;
            model.ListaInfoGraficosPosto3 = lstInfoSoldagemPosto3;

            List<RelatorioSoldagemInfo> lstInfoSoldagemPosto4 = new List<RelatorioSoldagemInfo>();
            List<int> lstCordoesPosto4 = bllRastreabilidade.GetCordoesByPosto(rastreabilidade,4);

            List<RelatorioGraficoInfo> lstInfoGraficosPosto4 = new List<RelatorioGraficoInfo>();
            bllRastreabilidade.ObterInfoGrafico(rastreabilidade,4,ref lstInfoGraficosPosto4);

            List<RelatorioCordaoInfo> lstInfoCordoesPosto4 = new List<RelatorioCordaoInfo>();
            bllRastreabilidade.ObterInfoCordoes(4,ref lstInfoCordoesPosto4);

            foreach (var item in lstCordoesPosto4)
            {
                RelatorioSoldagemInfo graficoInfo = new RelatorioSoldagemInfo();
                RelatorioCordaoInfo infoCordao = lstInfoCordoesPosto4.Where(cordao => cordao.Cordao == item).First();

                graficoInfo.cordao = item;
                graficoInfo.sinais = lstInfoGraficosPosto4.Where(cordao => cordao.cordao == item).Select(grp => grp.sinal).ToList();
                graficoInfo.corrente = lstInfoGraficosPosto4.Where(cordao => cordao.cordao == item).Select(grp => grp.corrente).ToList();
                graficoInfo.tensao = lstInfoGraficosPosto4.Where(cordao => cordao.cordao == item).Select(grp => grp.tensao).ToList();
                graficoInfo.codigoEps = infoCordao.CodigoEps;
                graficoInfo.correnteMinima = infoCordao.CorrenteMinima;
                graficoInfo.correnteMaxima = infoCordao.CorrenteMaxima;
                graficoInfo.tensaoMinima = infoCordao.TensaoMinima;
                graficoInfo.tensaoMaxima = infoCordao.TensaoMaxima;
                graficoInfo.stringFoto = bllRastreabilidade.ObterFotoCordao(item);

                lstInfoSoldagemPosto4.Add(graficoInfo);
            }

            double numPaginasCordoesPosto4 = lstCordoesPosto4.Count / 4;
            model.QuantidadePaginasCordoesPosto4 = Math.Floor(numPaginasCordoesPosto4);
            model.QuantidadeExtraCordoesPosto4 = lstCordoesPosto4.Count % 4;
            model.ListaInfoGraficosPosto4 = lstInfoSoldagemPosto4;

            model.ListaChecklistsPosto2 = bllChecklistsSoldagem.GetAllByPosto(2);
            model.ListaChecklistsPosto3 = bllChecklistsSoldagem.GetAllByPosto(3);
            model.ListaChecklistsPosto4 = bllChecklistsSoldagem.GetAllByPosto(4);

            if (count > 0 || lstRastreabilidades.Count != 3) model.StatusRastreabilidade = "Não Concluído";
            else model.StatusRastreabilidade = "Concluído";


            List<RelatorioSoldagemInfo> lstInfoGraficos = new List<RelatorioSoldagemInfo>();
            lstInfoGraficos.AddRange(lstInfoSoldagemPosto3);
            lstInfoGraficos.AddRange(lstInfoSoldagemPosto4);

            model.ListaInfoGraficos = lstInfoGraficos;

            model.RastreabilidadePesquisa = rastreabilidadePesquisa;
            model.DataInicio = dataInicio;
            model.DataFim = dataFim;

            return View("~/Views/RelatoriosSoldagem/RelatorioTraseira.cshtml", model);            
        }

        [HttpPost]
        public ActionResult pesquisar(string rastreabilidade, string dataInicio, string dataFim)
        {
            return RedirectToAction("Rastreabilidades", new { r = rastreabilidade, dI = dataInicio, dF = dataFim });
        }
    }
}