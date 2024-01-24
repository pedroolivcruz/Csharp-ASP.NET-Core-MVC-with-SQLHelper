using System.Data;
using Conectasys.Portal.Models;
using System.Data.SqlClient;


namespace Conectasys.Portal.DAL
{
    public class DalRastreabilidadeSoldagem
    {

        public RastreabilidadeSoldagemInfo GetDatesByCodigo(string codigo)
        {
            RastreabilidadeSoldagemInfo rastreabilidadeSoldagemInfo = new RastreabilidadeSoldagemInfo();

            try
            {
                string sSQL = @"SELECT DISTINCT(codigo),(SELECT TOP(1) (data_inicio) FROM dbo.Rastreabilidade_Soldagem WHERE codigo = @Codigo ORDER BY data_inicio ASC) AS data_inicio,(SELECT TOP(1) (data_fim) FROM dbo.Rastreabilidade_Soldagem WHERE codigo = @Codigo ORDER BY data_fim DESC) AS data_fim FROM dbo.Rastreabilidade_Soldagem WHERE codigo = @Codigo";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;


                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        rastreabilidadeSoldagemInfo.Rastreabilidade = dr["codigo"].ToString();
                        rastreabilidadeSoldagemInfo.DataInicio = Convert.ToDateTime(dr["data_inicio"]);
                        rastreabilidadeSoldagemInfo.DataFim = Convert.ToDateTime(dr["data_fim"]);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return rastreabilidadeSoldagemInfo;
        }

        public List<RastreabilidadeSoldagemInfo> GetAllRastreabilidadesByCodigo(string codigo)
        {
            List<RastreabilidadeSoldagemInfo> lstRastreabilidade = new List<RastreabilidadeSoldagemInfo>();

            try
            {
                string sSQL = @"SELECT (codigo),(material),(posto),(data_inicio),(data_fim),(Operador),(status) FROM dbo.Rastreabilidade_Soldagem WHERE codigo = @Codigo";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstRastreabilidade.Add(AtribuirRastreabilidade(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstRastreabilidade;
        }

        public List<string> GetAllRastreabilidadesByData(DateTime dataInicio, DateTime dataFim)
        {
            List<string> lstRastreabilidade = new List<string>();

            try
            {
                string sSQL = @"SELECT DISTINCT(codigo) FROM dbo.Rastreabilidade_Soldagem WHERE data_inicio >= @DataInicio AND data_fim <= @DataFim";

                SqlParameter[] parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@DataInicio", SqlDbType.DateTime);
                parametros[0].Value = dataInicio;

                parametros[1] = new SqlParameter("@DataFim", SqlDbType.DateTime);
                parametros[1].Value = dataFim;


                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstRastreabilidade.Add(dr["codigo"].ToString());
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstRastreabilidade;
        }

        public List<string> GetSimilarRastreabilidadesByCodigo(string codigo)
        {
            List<string> lstRastreabilidade = new List<string>();

            try
            {
                string sSQL = @"SELECT DISTINCT(codigo) FROM dbo.Rastreabilidade_Soldagem WHERE codigo LIKE '%" + codigo + "%'";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstRastreabilidade.Add(dr["codigo"].ToString());
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstRastreabilidade;
        }

        public List<int> GetCordoesByPosto(string codigo, int posto)
        {
            List<int> lstCordoesPosto = new List<int>();

            try
            {
                string sSQL = @"SELECT DISTINCT(Cordao) AS cordoes FROM dbo.vwRastreabilidade_Graficos_Soldagem WHERE codigo = @Codigo AND posto = @Posto AND Cordao > 0 AND codigo_eps > 0 ORDER BY Cordao ASC";


                SqlParameter[] parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 10);
                parametros[0].Value = codigo;

                parametros[1] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[1].Value = posto;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstCordoesPosto.Add(Convert.ToInt32(dr["cordoes"]));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstCordoesPosto;
        }

        public bool ExisteSequenciaChecklist(string codigo, int posto, int sequencia)
        {
            bool existeSequencia = false;

            try
            {
                string sSQL = @"SELECT (sequencia) FROM dbo.Rastreabilidade_Checklist_Soldagem WHERE codigo = @Codigo AND posto = @Posto AND sequencia = @Sequencia ";

                SqlParameter[] parametros = new SqlParameter[3];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                parametros[1] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[1].Value = posto;

                parametros[2] = new SqlParameter("@Sequencia", SqlDbType.Int);
                parametros[2].Value = sequencia;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    existeSequencia = dr.HasRows;
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return existeSequencia;
        }

        public bool ObterInfoGraficos(string codigo,int posto, ref List<RelatorioGraficoInfo> lstGraficos)
        {
            bool flag = false;

            try
            {
                string sSQL = @"SELECT id_cordao,Cordao,corrente,tensao FROM vwRastreabilidade_Graficos_Soldagem WHERE codigo = @Codigo AND posto = @Posto AND codigo_eps > 0 ORDER BY id_cordao ASC";

                SqlParameter[] parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 10);
                parametros[0].Value = codigo;

                parametros[1] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[1].Value = posto;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstGraficos.Add(AtribuirGrafico(dr));
                    }

                    dr.Close();
                }
                return true;
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return false;
        }

        public bool ObterInfoCordoes(int posto, ref List<RelatorioCordaoInfo> lstCordoes)
        {
            bool flag = false;

            try
            {
                string sSQL = @"SELECT posto,Cordao,codigo_eps,corrente_minima,corrente_maxima,tensao_minima,tensao_maxima FROM vwRastreabilidade_Cordoes_Soldagem_V2 WHERE posto = @Posto ORDER BY Cordao ASC";

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
                return true;
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return false;
        }

        public string ObterFotoCordao(int cordao)
        {
            string stringFoto = string.Empty;

            try
            {
                string sSQL = @"SELECT foto FROM Cadastro_Cordoes_Soldagem WHERE Cordao = @Cordao";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Cordao", SqlDbType.Int);
                parametros[0].Value = cordao;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        if (dr.IsDBNull(dr.GetOrdinal("foto"))) stringFoto = string.Empty;
                        else
                        {
                            byte[] foto = (byte[])(dr["foto"]);
                            if (foto == null)
                            {
                                stringFoto = string.Empty;
                            }
                            else
                            {
                                stringFoto = "data:image/png;base64," + Convert.ToBase64String(foto, 0, foto.Length);
                            }
                        }
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return stringFoto;
        }


        private RastreabilidadeSoldagemInfo AtribuirRastreabilidade(SqlDataReader dr)
        {
            RastreabilidadeSoldagemInfo rastreabilidadeInfo = new RastreabilidadeSoldagemInfo();

            try
            {
                rastreabilidadeInfo.Rastreabilidade = dr["codigo"].ToString();
                rastreabilidadeInfo.Posto = Convert.ToInt32(dr["posto"]);
                rastreabilidadeInfo.DataInicio = Convert.ToDateTime(dr["data_inicio"]);
                rastreabilidadeInfo.DataFim = Convert.ToDateTime(dr["data_fim"]);
                rastreabilidadeInfo.Operador = dr["operador"].ToString();
                rastreabilidadeInfo.Status = Convert.ToInt32(dr["status"]);
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return rastreabilidadeInfo;
        }

        private RelatorioGraficoInfo AtribuirGrafico(SqlDataReader dr)
        {
            RelatorioGraficoInfo grafico = new RelatorioGraficoInfo();

            try
            {
                grafico.cordao = Convert.ToInt32(dr["Cordao"]);
                grafico.tensao = Convert.ToDouble(dr["tensao"]);
                grafico.corrente = Convert.ToDouble(dr["corrente"]);
                grafico.sinal = Convert.ToInt32(dr["id_cordao"]);               
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return grafico;
        }

        private RelatorioCordaoInfo AtribuirCordao(SqlDataReader dr)
        {
            RelatorioCordaoInfo cordao = new RelatorioCordaoInfo();

            try
            {   
                cordao.Cordao = Convert.ToInt32(dr["Cordao"]);
                cordao.CodigoEps = dr["codigo_eps"].ToString().Replace(",",".");
                cordao.CorrenteMinima = Convert.ToDouble(dr["corrente_minima"]);
                cordao.CorrenteMaxima = Convert.ToDouble(dr["corrente_maxima"]);
                cordao.TensaoMinima = Convert.ToDouble(dr["tensao_minima"]);
                cordao.TensaoMaxima = Convert.ToDouble(dr["tensao_maxima"]);
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return cordao;
        }
    }
}