using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Newtonsoft.Json;


namespace Conectasys.Portal.BLL
{
    public class BllProdutos
    {
        string fileName = Config.RootFolder + "\\Repositories\\Produtos.json";
        

        public List<ProdutoInfo> GetAll()
        {   
            List<ProdutoInfo> lstProdutos = new List<ProdutoInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstProdutos = JsonConvert.DeserializeObject<List<ProdutoInfo>>(fileText);
            }
            else
            {
                DalProdutos dalProdutos = new DalProdutos();
                lstProdutos = dalProdutos.GetAll();            
            }

            return lstProdutos;
        }

        public List<ProdutoInfo> GetAllByCodigo(string codigo)
        {
            List<ProdutoInfo> lstProdutos = new List<ProdutoInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                var data = JsonConvert.DeserializeObject<List<ProdutoInfo>>(fileText);
                lstProdutos = data.Where(x => x.Material.Contains(codigo)).ToList();
            }
            else
            {
                DalProdutos dalProdutos = new DalProdutos();
                lstProdutos = dalProdutos.GetAllByCodigo(codigo);
            }

            return lstProdutos;
        }

        public bool Insert(ProdutoInfo produtoInfo)
        {
            bool retorno = true;
         
            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<ProdutoInfo> lstProdutos = JsonConvert.DeserializeObject<List<ProdutoInfo>>(fileText);

                if (!lstProdutos.Where(x => x.Material == produtoInfo.Material).Any())
                {
                    lstProdutos.Add(new ProdutoInfo
                    {
                        Material = produtoInfo.Material,
                        Descricao = produtoInfo.Descricao,
                        StringFoto = "data:image/png;base64," + Convert.ToBase64String(produtoInfo.Foto, 0, produtoInfo.Foto.Length)
                });
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstProdutos));
                }
                else retorno = false;
            }
            else
            {
                DalProdutos dalProdutos = new DalProdutos();
                retorno =  dalProdutos.Insert(produtoInfo);
            }

            return retorno;
        }

        public bool Update(string codigoAntigo, ProdutoInfo produtoInfo)
        {
            bool retorno = true;
         
            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<ProdutoInfo> lstProdutos = JsonConvert.DeserializeObject<List<ProdutoInfo>>(fileText);

                lstProdutos.Find(x => x.Material == codigoAntigo).Descricao = produtoInfo.Descricao;
                lstProdutos.Find(x => x.Material == codigoAntigo).Foto = produtoInfo.Foto;
                lstProdutos.Find(x => x.Material == codigoAntigo).StringFoto = "data:image/png;base64," + Convert.ToBase64String(produtoInfo.Foto, 0, produtoInfo.Foto.Length);
                lstProdutos.Find(x => x.Material == codigoAntigo).Material = produtoInfo.Material;
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstProdutos));
            }
            else
            {
                DalProdutos dalProdutos = new DalProdutos();
                retorno = dalProdutos.Update(codigoAntigo, produtoInfo);
            }

            return retorno;
        }

        public bool Delete(string codigo)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<ProdutoInfo> lstProdutos = JsonConvert.DeserializeObject<List<ProdutoInfo>>(fileText);

                lstProdutos.RemoveAll(x => x.Material == codigo);
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstProdutos));
            }
            else
            {
                DalProdutos dalProdutos = new DalProdutos();
                retorno = dalProdutos.Delete(codigo);
            }

            return retorno;
        }
    }
}