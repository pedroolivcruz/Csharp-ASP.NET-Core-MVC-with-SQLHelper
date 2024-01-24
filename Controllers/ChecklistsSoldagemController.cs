using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class ChecklistsSoldagemController : Controller
    {
        BllChecklistSoldagem bllChecklistsSoldagem = new BllChecklistSoldagem();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(int p , int e)
        {
            ViewBag.Posto = p;
            ViewBag.Erro = e;
            return View("~/Views/ChecklistsSoldagem/Checklists.cshtml", bllChecklistsSoldagem.GetAllByPosto(p));
        }

        [HttpPost]
        public ActionResult pesquisar(int posto)
        {
            return RedirectToAction("Cadastros", new { p = posto , e = 0 });
        }

        public ActionResult Novo(int posto)
        {
            ViewBag.ParametroBusca = posto;
            return View("~/Views/ChecklistsSoldagem/Novo.cshtml");
        }

        [HttpPost]
        public ActionResult cadastrar(int postoPesquisa,
                                        string descricao,
                                        int posto,
                                        int sequencia,
                                        bool lerEtiqueta,
                                        bool gerarRastreabilidade,
                                        bool tipoConfirmacao,
                                        int numConfirmacao,
                                        bool ativo,
                                        IFormFile file)
        {
            int erro = 0;

            ChecklistSoldagemInfo checklistSoldagemInfo = new ChecklistSoldagemInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    checklistSoldagemInfo.Foto = ms.ToArray();
                }
            }
            
            checklistSoldagemInfo.CodigoMaterial = "1";
            checklistSoldagemInfo.Sequencia = sequencia;
            checklistSoldagemInfo.ValorConfirmacao = numConfirmacao;
            checklistSoldagemInfo.NumeroPrograma = 0;
            checklistSoldagemInfo.LerEtiqueta = lerEtiqueta;
            checklistSoldagemInfo.GerarRastreabilidade = gerarRastreabilidade;
            checklistSoldagemInfo.TipoConfirmacao = tipoConfirmacao;
            checklistSoldagemInfo.Ativo = ativo;
            checklistSoldagemInfo.Descricao = descricao;
            checklistSoldagemInfo.Data = DateTime.Now;
            checklistSoldagemInfo.Posto = posto;
       
            if(bllChecklistsSoldagem.Insert(checklistSoldagemInfo) == false) erro = 1;
            return RedirectToAction("Cadastros", new { p = postoPesquisa , e = erro });
        }

        public ActionResult Editar(int postoPesquisa,int idChecklist)
        {
            ViewBag.ParametroBusca = postoPesquisa;
            return View("~/Views/ChecklistsSoldagem/Editar.cshtml", bllChecklistsSoldagem.GetById(idChecklist));
        }

        [HttpPost]
        public ActionResult editar(int postoPesquisa,
                                           int idChecklist,
                                           string descricao,
                                           int posto,
                                           int sequencia,
                                           bool lerEtiqueta,
                                           bool gerarRastreabilidade,
                                           bool tipoConfirmacao,
                                           int numConfirmacao,
                                           bool ativo,
                                           string stringFotoAntiga,
                                           IFormFile file)
        {
            int erro = 0;

            ChecklistSoldagemInfo checklistSoldagemInfo = new ChecklistSoldagemInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    checklistSoldagemInfo.Foto = ms.ToArray();
                }
            }
            else
            {
                if (stringFotoAntiga != null)
                {
                    string foto = stringFotoAntiga.Remove(0, 22);
                    checklistSoldagemInfo.Foto = Convert.FromBase64String(foto);
                }
            }

            checklistSoldagemInfo.Sequencia = sequencia;
            checklistSoldagemInfo.ValorConfirmacao = numConfirmacao;
            checklistSoldagemInfo.LerEtiqueta = lerEtiqueta;
            checklistSoldagemInfo.GerarRastreabilidade = gerarRastreabilidade;
            checklistSoldagemInfo.TipoConfirmacao = tipoConfirmacao;
            checklistSoldagemInfo.Ativo = ativo;
            checklistSoldagemInfo.Descricao = descricao;
            checklistSoldagemInfo.Posto = posto;

            if(bllChecklistsSoldagem.Update(idChecklist, checklistSoldagemInfo) == false) erro = 2;
            return RedirectToAction("Cadastros", new { p = postoPesquisa , e = erro });
        }

        [HttpPost]
        public ActionResult deletar(int postoPesquisa,int idChecklist)
        {
            int erro = 0;

            if(bllChecklistsSoldagem.Delete(idChecklist) == false) erro = 3;
            return RedirectToAction("Cadastros", new { p = postoPesquisa , e = erro });
        }
    }
}