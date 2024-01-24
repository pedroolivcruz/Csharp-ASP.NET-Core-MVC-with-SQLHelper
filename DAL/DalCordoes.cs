using System.Data;
using Conectasys.Portal.Models;
using System.Data.SqlClient;
using System.Transactions;


namespace Conectasys.Portal.DAL
{
    public class DalCordoes
    {
        public List<CordaoInfo> GetAllByPosto(int posto)
        {
            List<CordaoInfo> lstCordoes = new List<CordaoInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Cordoes_Soldagem WHERE posto = @Posto";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[0].Value = posto;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstCordoes.Add(AtribuirCordao(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return lstCordoes;
        }

        public List<CordaoInfo> GetAllByCordao(int cordao)
        {
            List<CordaoInfo> lstCordoes = new List<CordaoInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Cordoes_Soldagem WHERE Cordao LIKE '%" + cordao + "%'";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstCordoes.Add(AtribuirCordao(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return lstCordoes;
        }

        public List<CordaoInfo> GetAllByEps(double eps)
        {
            List<CordaoInfo> lstCordoes = new List<CordaoInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Cordoes_Soldagem WHERE codigo_eps = @Eps";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Eps", SqlDbType.Float);
                parametros[0].Value = eps;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL,parametros))
                {
                    while (dr.Read())
                    {
                        lstCordoes.Add(AtribuirCordao(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return lstCordoes;
        }

        public int GetLastCordao()
        {
            int ultimoCordao = 0;

            try
            {
                string sSQL = @"SELECT TOP(1) Cordao FROM dbo.Cadastro_Cordoes_Soldagem ORDER BY Cordao DESC";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        ultimoCordao = Convert.ToInt32(dr["Cordao"]);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return ultimoCordao;
        }

        public CordaoInfo GetByCordao(int cordao)
        {
            CordaoInfo cordaoInfo = new CordaoInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Cordoes_Soldagem WHERE Cordao = @Cordao";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Cordao", SqlDbType.Int);
                parametros[0].Value = cordao;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        cordaoInfo = AtribuirCordao(dr);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return cordaoInfo;
        }

        public bool Insert(CordaoInfo cadastroCordaoInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    string sSQL = @"INSERT INTO dbo.Cadastro_Cordoes_Soldagem
                                                                       (Cordao
                                                                       ,posto
                                                                       ,descricao
                                                                       ,codigo_eps
                                                                       ,foto)
                                                                 VALUES
                                                                       (@Cordao
                                                                       ,@posto
                                                                       ,@descricao
                                                                       ,@DescricaoEps
                                                                       ,@foto)";
                   
                    SqlParameter[] parametros = new SqlParameter[5];

                    parametros[0] = new SqlParameter("@Cordao", SqlDbType.Int);
                    parametros[0].Value = cadastroCordaoInfo.Cordao;

                    parametros[1] = new SqlParameter("@posto", SqlDbType.Int);
                    parametros[1].Value = cadastroCordaoInfo.Posto;

                    parametros[2] = new SqlParameter("@descricao", SqlDbType.VarChar,200);
                    parametros[2].Value = cadastroCordaoInfo.Descricao;

                    parametros[3] = new SqlParameter("@DescricaoEps", SqlDbType.Float);
                    parametros[3].Value = cadastroCordaoInfo.CodigoEps;

                    parametros[4] = new SqlParameter("@foto", SqlDbType.Image);
                    parametros[4].Value = cadastroCordaoInfo.Foto;

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

        public bool Update(int cordaoAntigo, CordaoInfo cadastroCordaoInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"UPDATE dbo.Cadastro_Cordoes_Soldagem SET
                                                           
                                                           Cordao = @Cordao,                                                         
                                                           posto = @Posto,
                                                           descricao = @Descricao,
                                                           codigo_eps = @DescricaoEps,
                                                           foto = @Foto
                                                           
                                    WHERE Cordao = @Cordao_Antigo";
                    
                    SqlParameter[] parametros = new SqlParameter[6];

                    parametros[0] = new SqlParameter("@Cordao", SqlDbType.Int);
                    parametros[0].Value = cadastroCordaoInfo.Cordao;

                    parametros[1] = new SqlParameter("@Posto", SqlDbType.Int);
                    parametros[1].Value = cadastroCordaoInfo.Posto;

                    parametros[2] = new SqlParameter("@Descricao", SqlDbType.VarChar, 200);
                    parametros[2].Value = cadastroCordaoInfo.Descricao;

                    parametros[3] = new SqlParameter("@Cordao_Antigo", SqlDbType.Int);
                    parametros[3].Value = cordaoAntigo;

                    parametros[4] = new SqlParameter("@DescricaoEps", SqlDbType.Float);
                    parametros[4].Value = cadastroCordaoInfo.CodigoEps;

                    parametros[5] = new SqlParameter("@Foto", SqlDbType.Image);
                    parametros[5].Value = cadastroCordaoInfo.Foto;

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

        public bool Delete(int cordao)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"DELETE FROM dbo.Cadastro_Cordoes_Soldagem        
                                    WHERE Cordao = @Cordao";


                    SqlParameter[] parametros = new SqlParameter[1];

                    parametros[0] = new SqlParameter("@Cordao", SqlDbType.Int);
                    parametros[0].Value = cordao;

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

        private CordaoInfo AtribuirCordao(SqlDataReader dr)
        {
            CordaoInfo cadastroCordao = new CordaoInfo();

            try
            {
                cadastroCordao.Cordao = Convert.ToInt32(dr["Cordao"]);
                cadastroCordao.Posto = Convert.ToInt32(dr["posto"]);
                cadastroCordao.CodigoEps = Convert.ToDouble(dr["codigo_eps"]);
                cadastroCordao.Descricao = dr["descricao"].ToString();

                if (dr.IsDBNull(dr.GetOrdinal("foto"))) cadastroCordao.Foto = null;
                else
                {
                    byte[] foto = (byte[])(dr["foto"]);

                    if (foto == null)
                    {
                        cadastroCordao.Foto = null;
                    }
                    else
                    {
                        cadastroCordao.Foto = foto;
                        cadastroCordao.StringFoto = "data:image/png;base64," + Convert.ToBase64String(foto, 0, foto.Length);

                    }
                }                   
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return cadastroCordao;
        }
    }
}