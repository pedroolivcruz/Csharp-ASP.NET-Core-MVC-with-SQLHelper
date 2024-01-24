using Conectasys.Portal.BLL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Mvc;


namespace Conectasys.Portal.Controllers
{
    public class ProdutosController : Controller
    {
        BllProdutos bllProdutos = new BllProdutos();

        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Cadastros(string c, int e)
        {
            ViewBag.Erro = e;
            ViewBag.CodigoMaterial = c;

            List<ProdutoInfo> lstProdutos = new List<ProdutoInfo>();

            if (c != null && c != string.Empty) lstProdutos = bllProdutos.GetAllByCodigo(c);
            else lstProdutos = bllProdutos.GetAll();

            return View("~/Views/Produtos/ProdutosCadastrados.cshtml", lstProdutos);
        }

        [HttpPost]
        public ActionResult pesquisar(string codigo)
        {
            return RedirectToAction("Cadastros", new { c = codigo, e = 0 });           
        }

        public ActionResult Novo(string codigo)
        {
            ViewBag.CodigoMaterial = codigo;
            return View("~/Views/Produtos/Novo.cshtml");
        }

        [HttpPost]
        public ActionResult cadastrar(string codigo,
                                      string material,
                                      string descricao,
                                      IFormFile file)
        {
            int erro = 0;

            ProdutoInfo produtoInfo = new ProdutoInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    produtoInfo.Foto = ms.ToArray();
                }
            }

            produtoInfo.Material = material;
            produtoInfo.Descricao = descricao;

            if(bllProdutos.Insert(produtoInfo) == false) erro = 1;
            return RedirectToAction("Cadastros", new { c = codigo, e = erro });
        }

        public ActionResult Editar(string material, string codigo)
        {
            ViewBag.CodigoMaterial = codigo;
            return View("~/Views/Produtos/Editar.cshtml", bllProdutos.GetAllByCodigo(material)[0]);
        }

        [HttpPost]
        public ActionResult editar(string codigo,
                                  string material,
                                  string materialAntigo,
                                  string descricao,
                                  string stringFotoAntiga,
                                  IFormFile file)
        {
            int erro = 0;

            ProdutoInfo produtoInfo = new ProdutoInfo();

            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    produtoInfo.Foto = ms.ToArray();
                }
            }
            else
            {
                if (stringFotoAntiga != null)
                {
                    string foto = stringFotoAntiga.Remove(0, 22);
                    produtoInfo.Foto = Convert.FromBase64String(foto);
                }
            }

            produtoInfo.Material = material;
            produtoInfo.Descricao = descricao;

            if(bllProdutos.Update(materialAntigo, produtoInfo) == false) erro = 2;
            return RedirectToAction("Cadastros", new { c = codigo,  e = erro });
        }

        [HttpPost]
        public ActionResult deletar(string material, string codigo)
        {
            int erro = 0;

            if(bllProdutos.Delete(material) == false) erro = 3;
            return RedirectToAction("Cadastros", new { c = codigo, e = erro });
        }
    }
}