using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{  
    public class CordoesController : Controller
    {
        BllCordoes bllCordoes = new BllCordoes();
        BllEps bllEps = new BllEps();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(int p, string c, string e, int er)
        {
            ViewBag.Posto = p;
            ViewBag.Erro = er;
            ViewBag.Cordao = c;
            ViewBag.Eps = e;

            List<CordaoInfo> lstCordoes = new List<CordaoInfo>();

            if (c != null && c != string.Empty) lstCordoes = bllCordoes.GetAllByCordao(Convert.ToInt32(c));
            else if (e != null && e != string.Empty)
            {
                e = e.Replace(".", ",");
                lstCordoes = bllCordoes.GetAllByEps(Convert.ToDouble(e));
            }
            else lstCordoes = bllCordoes.GetAllByPosto(p);

            return View("~/Views/Cordoes/CordoesCadastrados.cshtml", lstCordoes);
        }

        [HttpPost]
        public ActionResult pesquisar(int posto,string cordao,string eps)
        {
            return RedirectToAction("Cadastros", new { p = posto, c = cordao, e = eps, er = 0 });
        }

        public ActionResult Novo(int posto, string cordao, string eps)
        {
            ViewBag.Posto = posto;           
            ViewBag.Cordao = cordao;
            ViewBag.Eps = eps;
            ViewBag.ListaEps = bllEps.GetAll();
            return View("~/Views/Cordoes/Novo.cshtml");
        }

        [HttpPost]
        public ActionResult cadastrar(int postoPesquisa,
                                      string cordaoPesquisa,
                                      string epsPesquisa,
                                      int cordao,
                                      string descricao,
                                      string codigoEps,
                                      int posto,
                                      int tipoSolda,
                                      IFormFile file)
        {

            int erro = 0;

            CordaoInfo cadastroCordaoInfo = new CordaoInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    cadastroCordaoInfo.Foto = ms.ToArray();
                }
            }

            if (cordao == 0) cadastroCordaoInfo.Cordao = bllCordoes.GetLastCordao() + 1;
            else cadastroCordaoInfo.Cordao = cordao;

            if (tipoSolda == 1) cadastroCordaoInfo.CodigoEps = 0;
            else
            {
                codigoEps = codigoEps.Replace(".", ",");
                cadastroCordaoInfo.CodigoEps = Convert.ToDouble(codigoEps);
            }
    
            cadastroCordaoInfo.Descricao = descricao;
            cadastroCordaoInfo.Posto = posto;
           
            if (bllCordoes.Insert(cadastroCordaoInfo) == false) erro = 1;
            return RedirectToAction("Cadastros", new { p = postoPesquisa , c = cordaoPesquisa, e = epsPesquisa, er = erro });
        }

        public ActionResult Editar(int cordao,string cordaoPesquisa, string postoPesquisa, string epsPesquisa)
        {
            ViewBag.Posto = postoPesquisa;
            ViewBag.Cordao = cordaoPesquisa;
            ViewBag.Eps = epsPesquisa;
            ViewBag.ListaEps = bllEps.GetAll();
            return View("~/Views/Cordoes/Editar.cshtml", bllCordoes.GetByCordao(cordao));
        }

        [HttpPost]
        public ActionResult editar(int postoPesquisa,
                                   string cordaoPesquisa,
                                   string epsPesquisa,
                                   int cordao,
                                   string descricao,
                                   string codigoEps,
                                   int posto,
                                   string stringFotoAntiga,
                                   int tipoSolda,
                                   IFormFile file )
        {
            int erro = 0;

            CordaoInfo cadastroCordaoInfo = new CordaoInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    cadastroCordaoInfo.Foto = ms.ToArray();
                }
            }
            else
            {
                if (stringFotoAntiga != null)
                {
                    string foto = stringFotoAntiga.Remove(0, 22);
                    cadastroCordaoInfo.Foto = Convert.FromBase64String(foto);
                }
            }

            cadastroCordaoInfo.Cordao = cordao;

            if (tipoSolda == 1) cadastroCordaoInfo.CodigoEps = 0;
            else
            {
                codigoEps = codigoEps.Replace(".", ",");
                cadastroCordaoInfo.CodigoEps = Convert.ToDouble(codigoEps);
            }

            cadastroCordaoInfo.Descricao = descricao;
            cadastroCordaoInfo.Posto = posto;

            if (bllCordoes.Update(cordao, cadastroCordaoInfo) == false) erro = 2;
            return RedirectToAction("Cadastros", new { p = postoPesquisa, c = cordaoPesquisa, e = epsPesquisa, er = erro });
        }

        [HttpPost]
        public ActionResult deletar(int posto,int cordao, string eps)
        {
            int erro = 0;

            if(bllCordoes.Delete(cordao) == false) erro = 3;
            return RedirectToAction("Cadastros", new { p = posto , c = string.Empty, e = eps, er = erro });
        }
    }
}