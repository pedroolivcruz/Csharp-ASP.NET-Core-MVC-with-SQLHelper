using System.Data;
using Conectasys.Portal.Models;
using System.Transactions;
using System.Data.SqlClient;


namespace Conectasys.Portal.DAL
{
    public class DalEps
    {
        public List<EpsInfo> GetAll()
        {
            List<EpsInfo> lstEps = new List<EpsInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Eps ORDER BY codigo_eps ASC";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstEps.Add(AtribuirEps(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstEps;
        }

        public List<EpsInfo> GetAllByCodigo(string codigo)
        {
            List<EpsInfo> lstEps = new List<EpsInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Eps WHERE codigo_eps LIKE '%" + codigo + "%' ORDER BY codigo_eps ASC";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstEps.Add(AtribuirEps(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstEps;
        }

        public EpsInfo GetByCodigo(double codigo)
        {
            EpsInfo epsInfo = new EpsInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Eps WHERE codigo_eps = @Codigo";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.Float);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        epsInfo = AtribuirEps(dr);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return epsInfo;
        }


        public bool Insert(EpsInfo cadastroEps)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    string sSQL = string.Empty;
                 
                    sSQL = @"INSERT INTO dbo.Cadastro_Eps (codigo_eps
                                                          ,corrente_minima
                                                          ,corrente_maxima
                                                          ,tensao_minima
                                                          ,tensao_maxima 
                                                          )
                                                          VALUES (@Codigo
                                                                 ,@CorrenteMinima
                                                                 ,@CorrenteMaxima
                                                                 ,@TensaoMinima
                                                                 ,@TensaoMaxima)";
                   

                    SqlParameter[] parametros = new SqlParameter[5];

                    parametros[0] = new SqlParameter("@Codigo", SqlDbType.Float);
                    parametros[0].Value = cadastroEps.DoubleCodigoEps;

                    parametros[1] = new SqlParameter("@CorrenteMaxima", SqlDbType.Float);
                    parametros[1].Value = cadastroEps.CorrenteMaxima;

                    parametros[2] = new SqlParameter("@CorrenteMinima", SqlDbType.Float);
                    parametros[2].Value = cadastroEps.CorrenteMinima;

                    parametros[3] = new SqlParameter("@TensaoMaxima", SqlDbType.Float);
                    parametros[3].Value = cadastroEps.TensaoMaxima;

                    parametros[4] = new SqlParameter("@TensaoMinima", SqlDbType.Float);
                    parametros[4].Value = cadastroEps.TensaoMinima;

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

        public bool UpdateEps(double codigoAntigo, EpsInfo updateEps)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = string.Empty;
                  
                    sSQL = @"UPDATE dbo.Cadastro_Eps SET codigo_eps = @Codigo,                                                                                         
                                                         corrente_minima = @CorrenteMinima
                                                         ,corrente_maxima = @CorrenteMaxima
                                                         ,tensao_minima = @TensaoMinima
                                                         ,tensao_maxima = @TensaoMaxima                                                           
                                                      WHERE codigo_eps = @Codigo_Antigo";
                    
                   

                    SqlParameter[] parametros = new SqlParameter[6];

                    parametros[0] = new SqlParameter("@Codigo", SqlDbType.Float);
                    parametros[0].Value = updateEps.DoubleCodigoEps;

                    parametros[1] = new SqlParameter("@Codigo_Antigo", SqlDbType.Float);
                    parametros[1].Value = codigoAntigo;

                    parametros[2] = new SqlParameter("@CorrenteMaxima", SqlDbType.Float);
                    parametros[2].Value = updateEps.CorrenteMaxima;

                    parametros[3] = new SqlParameter("@CorrenteMinima", SqlDbType.Float);
                    parametros[3].Value = updateEps.CorrenteMinima;

                    parametros[4] = new SqlParameter("@TensaoMaxima", SqlDbType.Float);
                    parametros[4].Value = updateEps.TensaoMaxima;

                    parametros[5] = new SqlParameter("@TensaoMinima", SqlDbType.Float);
                    parametros[5].Value = updateEps.TensaoMinima;

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

        public bool Delete(double codigo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"DELETE FROM dbo.Cadastro_Eps        
                                    WHERE codigo_eps = @Codigo";


                    SqlParameter[] parametros = new SqlParameter[1];

                    parametros[0] = new SqlParameter("@Codigo", SqlDbType.Float);
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

        private EpsInfo AtribuirEps(SqlDataReader dr)
        {
            EpsInfo epsCordao = new EpsInfo();

            try
            {
                epsCordao.DoubleCodigoEps = Convert.ToDouble(dr["codigo_eps"]);
                epsCordao.StringCodigoEps = dr["codigo_eps"].ToString().Replace(",", ".");
                epsCordao.TensaoMinima = Convert.ToDouble(dr["tensao_minima"]);
                epsCordao.TensaoMaxima = Convert.ToDouble(dr["tensao_maxima"]);
                epsCordao.CorrenteMinima = Convert.ToDouble(dr["corrente_minima"]);
                epsCordao.CorrenteMaxima = Convert.ToDouble(dr["corrente_maxima"]);
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return epsCordao;
        }
    }
}