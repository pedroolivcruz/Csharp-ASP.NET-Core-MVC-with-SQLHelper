using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class FotoTorquesController : Controller
    {
        BllFotoTorques bllFotoTorques = new BllFotoTorques();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(string d, int e)
        {
            ViewBag.Erro = e;

            if (d != string.Empty && d != null)
            {
                ViewBag.Descricao = d;
                return View("~/Views/GetFotoTorquesById/FotosCadastradas.cshtml", bllFotoTorques.GetAllByDescricao(d));
            }
            else
            {
                ViewBag.Descricao = string.Empty;
                return View("~/Views/GetFotoTorquesById/FotosCadastradas.cshtml", bllFotoTorques.GetAll());
            }
        }

        [HttpPost]
        public ActionResult pesquisar(string descricao)
        {
            return RedirectToAction("Cadastros", new { d = descricao,  e = 0});
        }

        public ActionResult Novo(string descricao)
        {
            ViewBag.Descricao = descricao;
            return View("~/Views/GetFotoTorquesById/Novo.cshtml");
        }

        [HttpPost]
        public ActionResult cadastrar(string descricao, string descricaoAntiga, string tipoTorque, IFormFile file)
        {
            int erro = 0;

            FotoTorquesInfo fotoTorque = new FotoTorquesInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fotoTorque.Foto = ms.ToArray();
                }
            }

            fotoTorque.Descricao = descricao;
            fotoTorque.TipoTorque = tipoTorque;

            if(bllFotoTorques.Insert(fotoTorque) == false) erro = 1;
            return RedirectToAction("Cadastros", new { d = descricaoAntiga, e = erro });
        }

        public ActionResult Editar(int id,string descricao)
        {
            ViewBag.Descricao = descricao;
            return View("~/Views/GetFotoTorquesById/Editar.cshtml", bllFotoTorques.GetById(id));
        }

        [HttpPost]
        public ActionResult editar(int id,
                                   string descricao,
                                   string descricaoAntiga,
                                   string tipoTorque,
                                   string stringFotoAntiga,
                                   IFormFile file)
        {
            int erro = 0;

            FotoTorquesInfo fotoTorqueInfo = new FotoTorquesInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fotoTorqueInfo.Foto = ms.ToArray();
                }
            }
            else
            {
                if (stringFotoAntiga != null)
                {
                    string foto = stringFotoAntiga.Remove(0, 22);
                    fotoTorqueInfo.Foto = Convert.FromBase64String(foto);
                }
            }

            fotoTorqueInfo.Descricao = descricao;
            fotoTorqueInfo.TipoTorque = tipoTorque;
        
            if(bllFotoTorques.Update(id, fotoTorqueInfo) == false) erro = 2;
            return RedirectToAction("Cadastros", new { d = descricaoAntiga, e = erro });
        }

        [HttpPost]
        public ActionResult deletar(int id, string descricao)
        {
            int erro = 0;

            if(bllFotoTorques.Delete(id) == false) erro = 3;
            return RedirectToAction("Cadastros", new { d = descricao, e = erro });
        }
    }
}