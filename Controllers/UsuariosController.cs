using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace JOST.Buslink.WEB.Controllers
{
    public class UsuariosController : Controller
    {
        BllUsuarios bllUsuarios = new BllUsuarios();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(string m, int p , string n, int e)
        {
            ViewBag.Erro = e;
            ViewBag.Matricula = m;
            ViewBag.Permissao = p;
            ViewBag.Nome = n;

            if (m != string.Empty && m != null) return View("~/Views/Usuarios/UsuariosCadastrados.cshtml", bllUsuarios.GetAllByMatricula(m));
            else if (n != string.Empty && n != null) return View("~/Views/Usuarios/UsuariosCadastrados.cshtml", bllUsuarios.GetAllByNome(n));
            else if(p == 2) return View("~/Views/Usuarios/UsuariosCadastrados.cshtml", bllUsuarios.GetAll());
            else return View("~/Views/Usuarios/UsuariosCadastrados.cshtml", bllUsuarios.GetAllByPermissao(p));
        }

        [HttpPost]
        public ActionResult pesquisar(string matricula, int permissao,string nome)
        {
            return RedirectToAction("Cadastros", new { m = matricula, p = permissao, n = nome, e = 0});
        }

        public ActionResult Novo(string matricula, int permissao, string nome)
        {
            ViewBag.Matricula = matricula;
            ViewBag.Permissao = permissao;
            ViewBag.Nome = nome;
            return View("~/Views/Usuarios/Novo.cshtml");
        }

        [HttpPost]
        public ActionResult cadastrar(string matricula, string matriculaPesquisada, int permissaoPesquisada,string nomePesquisado, string nome,string logon,int permissao)
        {
            int erro = 0;

            UsuarioInfo usuario = new UsuarioInfo();

            usuario.Matricula = matricula;
            usuario.Nome = nome;
            usuario.Logon = logon;
            usuario.Permissao = permissao;

            if (bllUsuarios.Insert(usuario) == false) erro = 1;
            return RedirectToAction("Cadastros", new { m = matriculaPesquisada, p = permissaoPesquisada, n = nomePesquisado, e = erro });
        }

        public ActionResult Editar(string matricula, string matriculaPesquisada, int permissaoPesquisada, string nomePesquisado)
        {
            ViewBag.Matricula = matricula;
            ViewBag.MatriculaPesquisada = matriculaPesquisada;
            ViewBag.PermissaoPesquisada = permissaoPesquisada;
            ViewBag.NomePesquisado = nomePesquisado;
            return View("~/Views/Usuarios/Editar.cshtml", bllUsuarios.ObterUsuarioPorMatricula(matricula));
        }

        [HttpPost]
        public ActionResult editar(string matriculaAntiga,
                                   string matriculaPesquisada,
                                   string nomePesquisado,
                                   int permissaoPesquisada,  
                                   string matricula,
                                   string nome,
                                   string logon, 
                                   int permissao)
        {
            int erro = 0;

            UsuarioInfo usuario = new UsuarioInfo();

            usuario.Matricula = matricula;
            usuario.Nome = nome;
            usuario.Logon = logon;
            usuario.Permissao = permissao;

            if (bllUsuarios.Update(matriculaAntiga, usuario) == false) erro = 2;
            return RedirectToAction("Cadastros", new { m = matriculaPesquisada, p = permissaoPesquisada, n = nomePesquisado, e = erro });
        }

        [HttpPost]
        public ActionResult deletar(string matricula, string matriculaPesquisada,int permissaoPesquisada, string nomePesquisado)
        {
            int erro = 0;

            if (bllUsuarios.Delete(matricula) == false) erro = 3;
            return RedirectToAction("Cadastros", new { m = matriculaPesquisada, p = permissaoPesquisada, n = nomePesquisado, e = erro });
        }
    }
}