using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class EpsController : Controller
    {
        BllEps bllEps = new BllEps();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(string cEps, int e)
        {
            ViewBag.Erro = e;

            List<EpsInfo> lstEps = new List<EpsInfo>();

            if (cEps != null && cEps != string.Empty)
            {
                ViewBag.CodigoEps = cEps.Replace(",", ".");
                cEps = cEps.Replace(',', '.');
                lstEps = bllEps.GetAllByCodigo(cEps);
            }
            else
            {
                ViewBag.CodigoEps = cEps;
                lstEps = bllEps.GetAll();
            }

            return View("~/Views/Eps/EpsCadastradas.cshtml",lstEps);
        }

        [HttpPost]
        public ActionResult pesquisar(string epsPesquisa)
        {
            return RedirectToAction("Cadastros", new { cEps = epsPesquisa, e = 0 }); ;
        }

        public ActionResult Nova(string epsPesquisa)
        {
            if (epsPesquisa != string.Empty && epsPesquisa != null) ViewBag.ParametroBusca = epsPesquisa.Replace(",", ".");
            else ViewBag.ParametroBusca = epsPesquisa;

            return View("~/Views/Eps/Nova.cshtml");
        }

        [HttpPost]
        public ActionResult cadastrar(string epsPesquisa,
                                        string codigoEps,
                                        string correnteMinima,
                                        string correnteMaxima,
                                        string tensaoMinima,
                                        string tensaoMaxima)
        {
            int erro = 0;

            EpsInfo cadastroEps = new EpsInfo();

            codigoEps = codigoEps.Replace(".", ",");
            cadastroEps.DoubleCodigoEps = Convert.ToDouble(codigoEps);

            correnteMinima = correnteMinima.Replace(".", ",");
            cadastroEps.CorrenteMinima = Convert.ToDouble(correnteMinima);

            correnteMaxima = correnteMaxima.Replace(".", ",");
            cadastroEps.CorrenteMaxima = Convert.ToDouble(correnteMaxima);

            tensaoMinima = tensaoMinima.Replace(".", ",");
            cadastroEps.TensaoMinima = Convert.ToDouble(tensaoMinima);

            tensaoMaxima = tensaoMaxima.Replace(".", ",");
            cadastroEps.TensaoMaxima = Convert.ToDouble(tensaoMaxima);

            if (bllEps.Insert(cadastroEps) == false) erro = 1;
            return RedirectToAction("Cadastros", new { cEps = epsPesquisa, e = erro });
        }

        public ActionResult Editar(string epsPesquisa, double codigo)
        {   
            ViewBag.CodigoAntigo = codigo;

            if (epsPesquisa != string.Empty && epsPesquisa != null) ViewBag.ParametroBusca = epsPesquisa.Replace(",", ".");
            else ViewBag.ParametroBusca = epsPesquisa;

            return View("~/Views/Eps/Editar.cshtml", bllEps.GetByCodigo(codigo));
        }

        [HttpPost]
        public ActionResult editar(string epsPesquisa,
                                   string codigoAntigo,
                                   string codigoEps,
                                   string correnteMinima,
                                   string correnteMaxima,
                                   string tensaoMinima,
                                   string tensaoMaxima)
        {
            int erro = 0;

            EpsInfo cadastroEps = new EpsInfo();

            codigoEps = codigoEps.Replace(".", ",");
            cadastroEps.DoubleCodigoEps = Convert.ToDouble(codigoEps);

            correnteMinima = correnteMinima.Replace(".", ",");
            cadastroEps.CorrenteMinima = Convert.ToDouble(correnteMinima);

            correnteMaxima = correnteMaxima.Replace(".", ",");
            cadastroEps.CorrenteMaxima = Convert.ToDouble(correnteMaxima);

            tensaoMinima = tensaoMinima.Replace(".", ",");
            cadastroEps.TensaoMinima = Convert.ToDouble(tensaoMinima);

            tensaoMaxima = tensaoMaxima.Replace(".", ",");
            cadastroEps.TensaoMaxima = Convert.ToDouble(tensaoMaxima);

            if(bllEps.Update(Convert.ToDouble(codigoAntigo), cadastroEps) == false) erro = 2;
            return RedirectToAction("Cadastros", new { cEps = epsPesquisa, e = erro });
        }

        [HttpPost]
        public ActionResult deletar(double codigo)
        {
            int erro = 0;

            if(bllEps.Delete(codigo) == false) erro = 3;
            return RedirectToAction("Cadastros", new { cEps = string.Empty , e = erro});
        }
    }
}