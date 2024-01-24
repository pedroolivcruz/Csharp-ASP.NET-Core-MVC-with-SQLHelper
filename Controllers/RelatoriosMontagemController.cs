using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class RelatoriosMontagemController : Controller
    {
        BllChecklistMontagem bllChecklistsMontagem = new BllChecklistMontagem();
        BllRastreabilidadeMontagem bllRastreabilidade = new BllRastreabilidadeMontagem();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Rastreabilidades(string r, string c, string dI, string dF)
        {
            int index = 0;

            List<string> lstRastreabilidadesInfo = new List<string>();
            List<RastreabilidadeMontagemInfo> lstRastreabilidades = new List<RastreabilidadeMontagemInfo>();

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
            else if (c != string.Empty && c != null) lstRastreabilidadesInfo = bllRastreabilidade.SearchComponente(c);
            else lstRastreabilidadesInfo = bllRastreabilidade.GetAllRastreabilidadesByData(dInicio, dFim);
  
            foreach (string codigo in lstRastreabilidadesInfo)
            {
                RastreabilidadeMontagemInfo rastreabilidadeInfo = bllRastreabilidade.GetDatesByCodigo(codigo);

                index++;
                rastreabilidadeInfo.Index = index;

                int countTesteFinalEsquerda = 0;
                int countTesteFinalDireita = 0;

                List<TesteFinalInfo> lstTesteFinalEsquerda = bllRastreabilidade.GetEsquerdaTesteFinal(codigo);
                List<TesteFinalInfo> lstTesteFinalDireita = bllRastreabilidade.GetDireitaTesteFinal(codigo);

                foreach (var item in lstTesteFinalEsquerda) if (item.Forca <= 26.8 && item.Forca >= 19.8) countTesteFinalEsquerda++;
                foreach (var item in lstTesteFinalDireita) if (item.Forca <= 26.8 && item.Forca >= 19.8) countTesteFinalDireita++;

                if (bllRastreabilidade.CountPostosFinished(codigo) == 5) rastreabilidadeInfo.Descricao = "Concluído";
                else rastreabilidadeInfo.Descricao = "Não Concluído";

                if (countTesteFinalEsquerda > 0 && countTesteFinalDireita > 0) rastreabilidadeInfo.Aprovacao = "Sim";
                else rastreabilidadeInfo.Aprovacao = "Não";

                lstRastreabilidades.Add(rastreabilidadeInfo);
            }

            ViewBag.Rastreabilidade = r;
            ViewBag.Componente = c;
            ViewBag.DataFim = dFim.ToString("yyyy-MM-dd HH:mm");
            ViewBag.DataInicio = dInicio.ToString("yyyy-MM-dd HH:mm");

            return View("~/Views/RelatoriosMontagem/Rastreabilidades.cshtml", lstRastreabilidades);
        }

        public ActionResult Relatorio(string rastreabilidade, string rastreabilidadePesquisa, string dataInicio, string dataFim, string componente)
        {
            List<RastreabilidadeMontagemInfo> lstRastreabilidades = bllRastreabilidade.GetAllRastreabilidadesByCodigo(rastreabilidade);

            int idFotoRala = bllRastreabilidade.GetIdFromFotoTorques(rastreabilidade, "Rala");

            dynamic model = new ExpandoObject();

            if (idFotoRala != 0)
            {
                FotoTorquesInfo fotoTorques = bllRastreabilidade.GetFotoTorquesById(idFotoRala);
                model.UrlFotoTorque = fotoTorques.StringFoto;
                model.DescricaoFotoTorque = fotoTorques.Descricao;
            }
            else
            {
                model.UrlFotoTorque = string.Empty;
                model.DescricaoFotoTorque = string.Empty;
            }
  
            model.CodigoRastreabilidade = rastreabilidade;
            model.Componente = componente;
            model.ListaInfoRastreabilidade = lstRastreabilidades;

            
            if (rastreabilidade.LastOrDefault() == '1') model.DescricaoProduto = "Articulação de ônibus Euro 5 - 6X2";
            else if (rastreabilidade.LastOrDefault() == '2') model.DescricaoProduto = "Articulação de ônibus Euro 5 - 8X2";
            else if (rastreabilidade.LastOrDefault() == '3') model.DescricaoProduto = "Articulação de ônibus Euro 6 - 6X2 sem BSA";
            else model.DescricaoProduto = "Articulação de ônibus Euro 6 - 6X2 e 8X2 com BSA";

            List<RastreabilidadeComponentesMontagemInfo> lstComponentes = bllRastreabilidade.GetAllComponentesByCodigo(rastreabilidade);
            model.ListaComponentes = lstComponentes;
            model.QuantidadeComponentes = lstComponentes.Count;

            List<TorqueInfo> lstTorquesRala = bllRastreabilidade.GetTorquesRala(rastreabilidade);
            model.ListaTorquesRala = lstTorquesRala;
            model.QuantidadeTorquesRala = lstTorquesRala.Count;

            List<TorqueInfo> lstTorquesComponentes = bllRastreabilidade.GetTorquesComponentes(rastreabilidade);
            model.ListaTorquesComponentes = lstTorquesComponentes;
            model.QuantidadeTorquesComponentes = lstTorquesComponentes.Count;

            model.ListaInfoRastreabilidadePosto1 = lstRastreabilidades.Where(i => i.Posto == 1).First();
            model.ListaInfoRastreabilidadePosto2 = lstRastreabilidades.Where(i => i.Posto == 2).First();
            model.ListaInfoRastreabilidadePosto3 = lstRastreabilidades.Where(i => i.Posto == 3).First();
            model.ListaInfoRastreabilidadePosto4 = lstRastreabilidades.Where(i => i.Posto == 4).First();
            model.ListaInfoRastreabilidadePosto5 = lstRastreabilidades.Where(i => i.Posto == 5).First();

            model.ListaChecklistsPosto1 = bllChecklistsMontagem.GetAllByPosto(1);
            model.ListaChecklistsPosto2Etapa1 = bllChecklistsMontagem.GetAllByPosto2(2, 0);
            model.ListaChecklistsPosto2Etapa2 = bllChecklistsMontagem.GetAllByPosto2(2, 1);
            model.ListaChecklistsPosto3 = bllChecklistsMontagem.GetAllByPosto(3);
            model.ListaChecklistsPosto4 = bllChecklistsMontagem.GetAllByPosto(4);
            model.ListaChecklistsPosto5 = bllChecklistsMontagem.GetAllByPosto(5);

            int indexTesteFinal = 0;

            List<TesteFinalInfo> lstTesteFinal = bllRastreabilidade.GetTesteFinal(rastreabilidade);

            List<double> lstAngulos1 = new List<double>();
            List<double> lstForcas1 = new List<double>();
            List<double> lstForcaMaxima1 = new List<double>();
            List<double> lstForcaMinima1 = new List<double>();
            List<double> lstVelocidades1 = new List<double>();

            for (int i = 0; i < lstTesteFinal.Count; i++)
            {
                if (lstTesteFinal[i].Angulo == 50)
                {

                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima1.Add(26.8);
                    lstForcaMinima1.Add(19.8);
                    indexTesteFinal++;
                    break;
                }
                else if (lstTesteFinal[i].Angulo >= 49)
                {
                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima1.Add(26.8);
                    lstForcaMinima1.Add(19.8);
                    indexTesteFinal++;
                    break;
                }
                else if (lstTesteFinal[i].Angulo >= 48)
                {
                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima1.Add(26.8);
                    lstForcaMinima1.Add(19.8);
                    indexTesteFinal++;
                    break;
                }
                else
                {
                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima1.Add(26.8);
                    lstForcaMinima1.Add(19.8);
                    indexTesteFinal++;
                }
            }
            model.ListaAngulos1 = lstAngulos1;
            model.ListaForcas1 = lstForcas1;
            model.ListaVelocidades1 = lstVelocidades1;
            model.ListaForcaMaxima1 = lstForcaMaxima1;
            model.ListaForcaMinima1 = lstForcaMinima1;


            List<double> lstAngulos2 = new List<double>();
            List<double> lstForcas2 = new List<double>();
            List<double> lstForcaMaxima2 = new List<double>();
            List<double> lstForcaMinima2 = new List<double>();
            List<double> lstVelocidades2 = new List<double>();

            for (int i = indexTesteFinal; i < lstTesteFinal.Count; i++)
            {
                if (lstTesteFinal[i].Angulo == -50)
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima2.Add(26.8);
                    lstForcaMinima2.Add(19.8);
                    break;
                }
                else if (lstTesteFinal[i].Angulo <= -49)
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima2.Add(26.8);
                    lstForcaMinima2.Add(19.8);
                    break;
                }
                else if (lstTesteFinal[i].Angulo <= -48)
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima2.Add(26.8);
                    lstForcaMinima2.Add(19.8);
                    break;
                }
                else
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);
                    lstForcaMaxima2.Add(26.8);
                    lstForcaMinima2.Add(19.8);
                }
            }
            model.ListaAngulos2 = lstAngulos2;
            model.ListaForcas2 = lstForcas2;
            model.ListaVelocidades2 = lstVelocidades2;
            model.ListaForcaMaxima2 = lstForcaMaxima2;
            model.ListaForcaMinima2 = lstForcaMinima2;


            int countTesteFinalEsquerda = 0;
            int countTesteFinalDireita = 0;

            List<TesteFinalInfo> lstDadosResultadoTesteFinal = new List<TesteFinalInfo>();
            List<TesteFinalInfo> lstTesteFinalEsquerda = bllRastreabilidade.GetEsquerdaTesteFinal(rastreabilidade);
            List<TesteFinalInfo> lstTesteFinalDireita = bllRastreabilidade.GetDireitaTesteFinal(rastreabilidade);

            TesteFinalInfo testeFinalEsquerda = new TesteFinalInfo();
            testeFinalEsquerda.Descricao = "Esquerda para Direita";
            testeFinalEsquerda.Angulo = 4;
            testeFinalEsquerda.Velocidade = 0;
            testeFinalEsquerda.Forca = 0;

            foreach (var item in lstTesteFinalEsquerda)
            {
                if (Math.Abs(item.Angulo) < Math.Abs(testeFinalEsquerda.Angulo))
                {
                    testeFinalEsquerda.Angulo = item.Angulo;
                    testeFinalEsquerda.Velocidade = item.Velocidade;
                    testeFinalEsquerda.Forca = item.Forca;
                }

                if (Math.Abs(item.Angulo) == Math.Abs(testeFinalEsquerda.Angulo))
                {
                    if (item.Forca > testeFinalEsquerda.Forca)
                    {
                        testeFinalEsquerda.Angulo = item.Angulo;
                        testeFinalEsquerda.Velocidade = item.Velocidade;
                        testeFinalEsquerda.Forca = item.Forca;
                    }
                }

                if (item.Forca <= 26.8 && item.Forca >= 19.8) countTesteFinalEsquerda++;
            }

            lstDadosResultadoTesteFinal.Add(testeFinalEsquerda);

            TesteFinalInfo testeFinalDireita = new TesteFinalInfo();
            testeFinalDireita.Descricao = "Direita para Esquerda";
            testeFinalDireita.Angulo = 4;
            testeFinalDireita.Velocidade = 0;
            testeFinalDireita.Forca = 0;

            foreach (var item in lstTesteFinalDireita)
            {
                if (Math.Abs(item.Angulo) < Math.Abs(testeFinalDireita.Angulo))
                {
                    testeFinalDireita.Angulo = item.Angulo;
                    testeFinalDireita.Velocidade = item.Velocidade;
                    testeFinalDireita.Forca = item.Forca;
                }

                if (Math.Abs(item.Angulo) == Math.Abs(testeFinalDireita.Angulo))
                {
                    if (item.Forca > testeFinalEsquerda.Forca)
                    {
                        testeFinalDireita.Angulo = item.Angulo;
                        testeFinalDireita.Velocidade = item.Velocidade;
                        testeFinalDireita.Forca = item.Forca;
                    }
                }

                if (item.Forca <= 26.8 && item.Forca >= 19.8) countTesteFinalDireita++;
            }

            lstDadosResultadoTesteFinal.Add(testeFinalDireita);

            model.ListaDadosZeroGrau = lstDadosResultadoTesteFinal;

            if (countTesteFinalEsquerda > 0 && countTesteFinalDireita > 0) model.ResultadoTesteFinal = "Peça Aprovada";
            else model.ResultadoTesteFinal = "Peça Reprovada";

            if (/*countTesteFinalEsquerda > 0 && countTesteFinalDireita > 0 &&*/ bllRastreabilidade.CountPostosFinished(rastreabilidade) == 5) model.StatusRastreabilidade = "Concluído";
            else model.StatusRastreabilidade = "Não Concluído";

            model.RastreabilidadePesquisa = rastreabilidadePesquisa;
            model.DataInicio = dataInicio;
            model.DataFim = dataFim;

            return View("~/Views/RelatoriosMontagem/Relatorio.cshtml", model);
        }

        [HttpPost]
        public ActionResult pesquisar(string rastreabilidade, string componente, string dataInicio, string dataFim)
        {
            return RedirectToAction("Rastreabilidades", new { r = rastreabilidade, c = componente, dI = dataInicio, dF = dataFim });
        }
    }
}