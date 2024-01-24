using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class ComponentesController : Controller
    {
        BllComponentes bllComponentes = new BllComponentes();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(int p, int m, int e)
        {
            ViewBag.Posto = p;
            ViewBag.Modelo = m;
            ViewBag.Erro = e;
            return View("~/Views/Componentes/ComponentesCadastrados.cshtml", bllComponentes.SearchComponentes(p,m));

        }

        [HttpPost]
        public ActionResult pesquisar(int posto, int modelo)
        {
            return RedirectToAction("Cadastros", new { p = posto, m = modelo, e = 0 });
        }

        public ActionResult Novo(int posto, int modelo)
        {
            ViewBag.Posto = posto;
            ViewBag.Modelo = modelo;
            return View("~/Views/Componentes/Novo.cshtml");
        }

        [HttpPost]
        public ActionResult cadastrar(int postoPesquisa,
                                      int modeloPesquisa,
                                      string tipoMaterial,
                                      string codigoSAP,
                                      bool ativo,
                                      string descricao,
                                      int sequencia,
                                      int posto,
                                      int modelo)
        {
            int erro = 0;

            ComponenteInfo componenteInfo = new ComponenteInfo();

            componenteInfo.Sequencia = sequencia;

            componenteInfo.Descricao = descricao;
            componenteInfo.TipoMaterial = tipoMaterial;
            componenteInfo.CodigoSAP = codigoSAP;
            componenteInfo.Posto = posto;
            componenteInfo.Modelo = modelo;
            componenteInfo.Ativo = ativo;

            if(bllComponentes.Insert(componenteInfo) == false) erro = 1;
            return RedirectToAction("Cadastros", new { p = postoPesquisa, m = modeloPesquisa, e = erro });
        }

        public ActionResult Editar(int id,int posto, int modelo)
        {
            ViewBag.IdComponente = id;
            ViewBag.Posto = posto;
            ViewBag.Modelo = modelo;
            return View("~/Views/Componentes/Editar.cshtml", bllComponentes.GetById(id));
        }

        [HttpPost]
        public ActionResult editar(int postoPesquisa,
                                   int modeloPesquisa,
                                   string codigoSAP,
                                   string tipoMaterial,
                                   string descricao,
                                   int sequencia,
                                   bool ativo,
                                   int posto,
                                   int modelo,
                                   int id)
        {
            int erro = 0;

            ComponenteInfo componenteInfo = new ComponenteInfo();

            componenteInfo.Sequencia = sequencia;
            componenteInfo.CodigoSAP = codigoSAP;
            componenteInfo.Descricao = descricao;
            componenteInfo.TipoMaterial = tipoMaterial;
            componenteInfo.Posto = posto;
            componenteInfo.Modelo = modelo;
            componenteInfo.IdComponente = id;
            componenteInfo.Ativo = ativo;

            if(bllComponentes.Update(componenteInfo) == false) erro = 2;
            return RedirectToAction("Cadastros", new { p = postoPesquisa, m = modeloPesquisa, e = erro });
        }

        [HttpPost]
        public ActionResult deletar(int id, int posto, int modelo)
        {
            int erro = 0;

            if(bllComponentes.Delete(id) == false) erro = 3;
            return RedirectToAction("Cadastros", new { p = posto, m = modelo, e = erro });
        }
    }
}