using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;



namespace Conectasys.Portal.Controllers
{
    public class DashboardMontagemController : Controller
    {
        BllRastreabilidadeMontagem bllRastreabilidade = new BllRastreabilidadeMontagem();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Dashboard(string r)
        {
            dynamic model = new ExpandoObject();

            if(r != null & r != string.Empty)
            {
                model.Rastreabilidade = r;
            }
            else
            {
                r = bllRastreabilidade.GetLastRastreabilidade();
                model.Rastreabilidade = r;
            }

            int indexTesteFinal = 0;

            List<TesteFinalInfo> lstTesteFinal = bllRastreabilidade.GetTesteFinal(r);
            List<TesteFinalInfo> lstGrafico1 = new List<TesteFinalInfo>();
            List<TesteFinalInfo> lstGrafico2 = new List<TesteFinalInfo>();

            List<double> lstAngulos1 = new List<double>();
            List<double> lstForcas1 = new List<double>();
            List<double> lstVelocidades1 = new List<double>();

            List<double> lstAngulos2 = new List<double>();
            List<double> lstForcas2 = new List<double>();
            List<double> lstVelocidades2 = new List<double>();

            for (int i = 0; i < lstTesteFinal.Count; i++)
            {
                if (lstTesteFinal[i].Angulo == 50)
                {

                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico1.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });

                    indexTesteFinal++;
                    break;
                }
                else if (lstTesteFinal[i].Angulo >= 49)
                {
                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico1.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });

                    indexTesteFinal++;
                    break;
                }
                else if (lstTesteFinal[i].Angulo >= 48)
                {
                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico1.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });

                    indexTesteFinal++;
                    break;
                }
                else
                {
                    lstAngulos1.Add(lstTesteFinal[i].Angulo);
                    lstForcas1.Add(lstTesteFinal[i].Forca);
                    lstVelocidades1.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico1.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });
                    indexTesteFinal++;
                }
            }

            for (int i = indexTesteFinal; i < lstTesteFinal.Count; i++)
            {
                if (lstTesteFinal[i].Angulo == -50)
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico2.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });
                    break;
                }
                else if (lstTesteFinal[i].Angulo <= -49)
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico2.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });
                    break;
                }
                else if (lstTesteFinal[i].Angulo <= -48)
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico2.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });
                    break;
                }
                else
                {
                    lstAngulos2.Add(lstTesteFinal[i].Angulo);
                    lstForcas2.Add(lstTesteFinal[i].Forca);
                    lstVelocidades2.Add(lstTesteFinal[i].Velocidade);

                    lstGrafico2.Add(new TesteFinalInfo()
                    {
                        Angulo = lstTesteFinal[i].Angulo,
                        Forca = lstTesteFinal[i].Forca,
                        Velocidade = lstTesteFinal[i].Velocidade
                    });
                }
            }

            model.ForcaMaxima = 19.8;
            model.ForcaMinima = 26.8;

            model.ListaAngulos1 = lstAngulos1;
            model.ListaForcas1 = lstForcas1;
            model.ListaVelocidades1 = lstVelocidades1;
            model.ListaGrafico1 = lstGrafico1;

            model.ListaAngulos2 = lstAngulos2;
            model.ListaForcas2 = lstForcas2;
            model.ListaVelocidades2 = lstVelocidades2;
            model.ListaGrafico2 = lstGrafico2;

            int countTesteFinalEsquerda = 0;
            int countTesteFinalDireita = 0;

            List<TesteFinalInfo> lstDadosResultadoTesteFinal = new List<TesteFinalInfo>();
            List<TesteFinalInfo> lstTesteEsquerda = bllRastreabilidade.GetEsquerdaTesteFinal(r);
            List<TesteFinalInfo> lstTesteDireita = bllRastreabilidade.GetDireitaTesteFinal(r);

            TesteFinalInfo testeFinalEsquerda = new TesteFinalInfo();
            testeFinalEsquerda.Descricao = "Esquerda para Direita";
            testeFinalEsquerda.Angulo = 4;
            testeFinalEsquerda.Velocidade = 0;
            testeFinalEsquerda.Forca = 0;

            foreach (var item in lstTesteEsquerda)
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
            }
            lstDadosResultadoTesteFinal.Add(testeFinalEsquerda);

            TesteFinalInfo testeFinalDireita = new TesteFinalInfo();
            testeFinalDireita.Descricao = "Direita para Esquerda";
            testeFinalDireita.Angulo = 4;
            testeFinalDireita.Velocidade = 0;
            testeFinalDireita.Forca = 0;

            foreach (var item in lstTesteDireita)
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
            }
            lstDadosResultadoTesteFinal.Add(testeFinalDireita);

            model.ListaDadosZeroGrau = lstDadosResultadoTesteFinal;

            foreach (var item in bllRastreabilidade.GetEsquerdaTesteFinal(r))
            {
                if (item.Forca <= 26.8 && item.Forca >= 19.8) countTesteFinalEsquerda++;
            }

            foreach (var item in bllRastreabilidade.GetDireitaTesteFinal(r))
            {
                if (item.Forca <= 26.8 && item.Forca >= 19.8) countTesteFinalDireita++;
            }

            if (countTesteFinalEsquerda > 0 && countTesteFinalDireita > 0) model.ResultadoTesteFinal = "Peça Aprovada";
            else model.ResultadoTesteFinal = "Peça Reprovada";

            return View("~/Views/DashboardMontagem/Dashboard.cshtml", model);
        }

        [HttpPost]
        public ActionResult pesquisar(string rastreabilidade) 
        {   
           return RedirectToAction("Dashboard", new { r = rastreabilidade });
        }
    }
}
