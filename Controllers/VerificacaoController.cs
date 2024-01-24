using Conectasys.Portal.BLL;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class VerificacaoController : Controller
    {
        BllLogin bllLogin = new BllLogin();
        BllComponentes bllComponentes = new BllComponentes();
        BllCordoes bllCordoes = new BllCordoes();
        BllEps bllEps = new BllEps();
        BllProdutos bllProdutos = new BllProdutos();
        BllChecklistMontagem bllChecklistsMontagem = new BllChecklistMontagem();
        BllChecklistSoldagem bllChecklistsSoldagem = new BllChecklistSoldagem();
        BllFotoTorques bllFotoTorques = new BllFotoTorques();
        BllUsuarios bllUsuarios = new BllUsuarios();


        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public VerificacaoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Login(int i)
        {
            if (_session.GetString("MatriculaUsuario") != null)
            {
                switch (i)
                {
                    case 1:
                        return RedirectToAction("Cadastros", "Componentes", new { p = 1, m = 1, e = 0 });

                    case 2:
                        return RedirectToAction("Cadastros", "Cordoes", new { p = 5, c = string.Empty ,e = string.Empty, er = 0 });

                    case 3:
                        return RedirectToAction("Cadastros", "Eps", new { cEPS = string.Empty, e = 0 });

                    case 4:
                        return RedirectToAction("Cadastros", "GetFotoTorquesById", new { e = 0 });

                    case 5:
                        return RedirectToAction("Cadastros", "Produtos", new { e = 0 });

                    case 6:
                        return RedirectToAction("Cadastros", "Usuarios", new { p = 2, e = 0 });

                    case 7:
                        return RedirectToAction("Cadastros", "ChecklistsMontagem", new { p = 1, e = 0 });

                    case 8:
                        return RedirectToAction("Cadastros", "ChecklistsSoldagem", new { p = 1, e = 0 });

                    default:
                        return RedirectToAction("AcessoNegado", "AcessoNegado");
                }
            }
            else
            {
                ViewBag.IndexPagina = i;
                return View("~/Views/Verificacao/Login.cshtml");
            }        
        }

        [HttpPost]
        public ActionResult login(int i, string matricula)
        {
            if (bllLogin.HasUsuario(matricula))
            {
                _session.SetString("MatriculaUsuario", matricula);

                switch (i)
                {
                    case 1:
                        return RedirectToAction("Cadastros", "Componentes", new { p = 1, m = 1, e = 0 });

                    case 2:
                        return RedirectToAction("Cadastros", "Cordoes", new { p = 5, c = string.Empty, e = string.Empty, er = 0 });
                        
                    case 3:
                        return RedirectToAction("Cadastros", "Eps", new { cEPS = string.Empty, e = 0 });

                    case 4:
                        return RedirectToAction("Cadastros", "GetFotoTorquesById" , new { e = 0 });

                    case 5:
                        return RedirectToAction("Cadastros", "Produtos", new { e = 0 });

                    case 6:
                        return RedirectToAction("Cadastros", "Usuarios", new { p = 2, e = 0 });

                    case 7:
                        return RedirectToAction("Cadastros", "ChecklistsMontagem", new { p = 1, e = 0 });

                    case 8:
                        return RedirectToAction("Cadastros", "ChecklistsSoldagem", new { p = 1, e = 0 });

                    default:
                        return RedirectToAction("AcessoNegado", "AcessoNegado");
                }
            }
            else
            {
                return RedirectToAction("AcessoNegado", "AcessoNegado");
            }
        }
    }
}