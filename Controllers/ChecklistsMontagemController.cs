using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class ChecklistsMontagemController : Controller
    {
        BllChecklistMontagem bllChecklistsMontagem = new BllChecklistMontagem();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(int p, int e)
        {
            ViewBag.Posto = p;
            ViewBag.Erro = e;
            return View("~/Views/ChecklistsMontagem/Checklists.cshtml", bllChecklistsMontagem.GetAllByPosto(p));
        }

        [HttpPost]
        public ActionResult pesquisar(int posto)
        {
            return RedirectToAction("Cadastros", new { p = posto, e = 0 });            
        }

        public ActionResult Novo(int posto)
        {
            ViewBag.ParametroBusca = posto;
            return View("~/Views/ChecklistsMontagem/Novo.cshtml");
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
                                              int numPrograma,
                                              IFormFile file)
        {
            int erro = 0;

            ChecklistMontagemInfo checklistMontagemInfo = new ChecklistMontagemInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    checklistMontagemInfo.Foto = ms.ToArray();
                }
            }

            checklistMontagemInfo.CodigoMaterial = "1";
            checklistMontagemInfo.Sequencia = sequencia;
            checklistMontagemInfo.ValorConfirmacao = numConfirmacao;
            checklistMontagemInfo.NumeroPrograma = numPrograma;
            checklistMontagemInfo.LerEtiqueta = lerEtiqueta;
            checklistMontagemInfo.GerarRastreabilidade = gerarRastreabilidade;
            checklistMontagemInfo.TipoConfirmacao = tipoConfirmacao;
            checklistMontagemInfo.Ativo = ativo;
            checklistMontagemInfo.Descricao = descricao;
            checklistMontagemInfo.Data = DateTime.Now;
            checklistMontagemInfo.Posto = posto;

            if(bllChecklistsMontagem.Insert(checklistMontagemInfo) == false) erro = 1;
            return RedirectToAction("Cadastros", new { p = postoPesquisa , e = erro});
        }

        public ActionResult Editar(int postoPesquisa,int idChecklist)
        {
            ViewBag.ParametroBusca = postoPesquisa;
            return View("~/Views/ChecklistsMontagem/Editar.cshtml", bllChecklistsMontagem.GetById(idChecklist));
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
                                    int numPrograma,
                                    string stringFotoAntiga,
                                    IFormFile file)
        {
            int erro = 0;

            ChecklistMontagemInfo checklistMontagemInfo = new ChecklistMontagemInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    checklistMontagemInfo.Foto = ms.ToArray();
                }
            }
            else
            {   
                if(stringFotoAntiga != null)
                {
                    string foto = stringFotoAntiga.Remove(0, 22);
                    checklistMontagemInfo.Foto = Convert.FromBase64String(foto);
                }   
            }

            checklistMontagemInfo.Sequencia = sequencia;
            checklistMontagemInfo.ValorConfirmacao = numConfirmacao;
            checklistMontagemInfo.NumeroPrograma = numPrograma;
            checklistMontagemInfo.LerEtiqueta = lerEtiqueta;
            checklistMontagemInfo.GerarRastreabilidade = gerarRastreabilidade;
            checklistMontagemInfo.TipoConfirmacao = tipoConfirmacao;
            checklistMontagemInfo.Ativo = ativo;
            checklistMontagemInfo.Descricao = descricao;
            checklistMontagemInfo.Posto = posto;

            if(bllChecklistsMontagem.Update(idChecklist, checklistMontagemInfo) == false) erro = 2;
            return RedirectToAction("Cadastros", new { p = postoPesquisa , e = erro });
        }

        [HttpPost]
        public ActionResult deletar(int idChecklist,int postoPesquisa)
        {
            int erro = 0;

            if(bllChecklistsMontagem.Delete(idChecklist) == false) erro = 3;
            return RedirectToAction("Cadastros", new { p = postoPesquisa , e = erro });
        }
    }
}