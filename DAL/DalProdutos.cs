using System.Data;
using System.Transactions;
using Conectasys.Portal.Models;
using System.Data.SqlClient;


namespace Conectasys.Portal.DAL
{
    public class DalProdutos
    {
        public List<ProdutoInfo> GetAll()
        {
            List<ProdutoInfo> lstComponentes = new List<ProdutoInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Produto";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstComponentes.Add(AtribuirProduto(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstComponentes;
        }

        public List<ProdutoInfo> GetAllByCodigo(string codigo)
        {
            List<ProdutoInfo> lstComponentes = new List<ProdutoInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Produto WHERE material LIKE '%" + codigo + "%'";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstComponentes.Add(AtribuirProduto(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstComponentes;
        }


        public bool Insert(ProdutoInfo produtoInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    string sSQL = @"INSERT INTO dbo.Cadastro_Produto
                                                                       (material
                                                                       ,descricao
                                                                       ,foto)
                                                                 VALUES
                                                                       (@material
                                                                       ,@descricao
                                                                       ,@foto)";

                    SqlParameter[] parametros = new SqlParameter[3];

                    parametros[0] = new SqlParameter("@material", SqlDbType.VarChar,20);
                    parametros[0].Value = produtoInfo.Material;

                    parametros[1] = new SqlParameter("@descricao", SqlDbType.VarChar, 200);
                    parametros[1].Value = produtoInfo.Descricao;

                    parametros[2] = new SqlParameter("@foto", SqlDbType.Image);
                    parametros[2].Value = produtoInfo.Foto;
                    
                    SqlHelper.ExecuteNonQuery(Config.ConexaoDB, CommandType.Text, sSQL, parametros);

                    scope.Complete();

                    return true;
                }
                catch (Exception erro)
                {

                }
                return false;
            }
        }

        public bool Update(string codigoAntigo, ProdutoInfo produtoInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"UPDATE dbo.Cadastro_Produto SET
                                                           
                                                           material = @Material,                                                         
                                                           descricao = @Descricao,
                                                           foto = @Foto
                                                           
                                    WHERE material = @Material_Antigo";

                    SqlParameter[] parametros = new SqlParameter[4];

                    parametros[0] = new SqlParameter("@Material", SqlDbType.VarChar,20);
                    parametros[0].Value = produtoInfo.Material;

                    parametros[1] = new SqlParameter("@Descricao", SqlDbType.VarChar, 200);
                    parametros[1].Value = produtoInfo.Descricao;

                    parametros[2] = new SqlParameter("@Material_Antigo", SqlDbType.VarChar,20);
                    parametros[2].Value = codigoAntigo;

                    parametros[3] = new SqlParameter("@Foto", SqlDbType.Image);
                    parametros[3].Value = produtoInfo.Foto;
                    
                    SqlHelper.ExecuteNonQuery(Config.ConexaoDB, CommandType.Text, sSQL, parametros);

                    scope.Complete();

                    return true;
                }
                catch (Exception erro)
                {

                }
                return false;
            }
        }

        public bool Delete(string codigo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"DELETE FROM dbo.Cadastro_Produto        
                                    WHERE material = @Material";


                    SqlParameter[] parametros = new SqlParameter[1];

                    parametros[0] = new SqlParameter("@Material", SqlDbType.VarChar, 20);
                    parametros[0].Value = codigo;

                    SqlHelper.ExecuteNonQuery(Config.ConexaoDB, CommandType.Text, sSQL, parametros);

                    scope.Complete();

                    return true;
                }
                catch (Exception erro)
                {
                    // Log.Gravar(erro);
                }
                return false;
            }
        }

        private ProdutoInfo AtribuirProduto(SqlDataReader dr)
        {
            ProdutoInfo produto = new ProdutoInfo();

            try
            {                
                produto.Descricao = dr["descricao"].ToString();
                produto.Material = dr["material"].ToString();

                if (dr.IsDBNull(dr.GetOrdinal("foto"))) produto.Foto = null;
                else
                {
                    byte[] foto = (byte[])(dr["foto"]);
                    if (foto == null)
                    {
                        produto.Foto = null;
                    }
                    else
                    {
                        produto.Foto = foto;
                        produto.StringFoto = "data:image/png;base64," + Convert.ToBase64String(foto, 0, foto.Length);

                    }
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return produto;
        }

    }
}