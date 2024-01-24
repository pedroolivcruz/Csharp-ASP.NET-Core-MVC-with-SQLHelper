using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class AcessoNegadoController : Controller
    {
        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult AcessoNegado()
        {
            return View("~/Views/AcessoNegado/AcessoNegado.cshtml");
        }

        public ActionResult voltar()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}