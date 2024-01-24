using System.Data;
using Conectasys.Portal.Models;
using System.Data.SqlClient;


namespace Conectasys.Portal.DAL
{
    public class DalRastreabilidadeMontagem
    {
        public List<RastreabilidadeMontagemInfo> GetAllRastreabilidadesByCodigo(string codigo)
        {
            List<RastreabilidadeMontagemInfo> lstRastreabilidade = new List<RastreabilidadeMontagemInfo>();

            try
            {
                string sSQL = @"SELECT (codigo),(material),(posto),(data_inicio),(data_fim),(Operador),(status) FROM dbo.Rastreabilidade_Montagem WHERE codigo = @Codigo ORDER BY data_inicio ASC";

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

        public bool HasSequenciaChecklist(string codigo, int posto, int sequencia)
        {
            bool existeSequencia = false;

            try
            {
                string sSQL = @"SELECT (sequencia) FROM dbo.Rastreabilidade_Checklist_Montagem WHERE codigo = @Codigo AND posto = @Posto AND sequencia = @Sequencia";

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

        public int CountPostosFinished(string codigo)
        {
            int num = 0;

            try
            {
                string sSQL = @"SELECT DISTINCT(COUNT(posto)) AS quantidade FROM dbo.Rastreabilidade_Montagem WHERE codigo = @Codigo AND Status = 2 ";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        num = Convert.ToInt32(dr["quantidade"]);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return num;
        }

        public RastreabilidadeMontagemInfo GetDatesByCodigo(string codigo)
        {
            RastreabilidadeMontagemInfo rastreabilidadeMontagemInfo = new RastreabilidadeMontagemInfo();

            try
            {
                string sSQL = @"SELECT DISTINCT(codigo),(SELECT TOP(1) (data_inicio) FROM dbo.Rastreabilidade_Montagem WHERE codigo = @Codigo ORDER BY data_inicio ASC) AS data_inicio,(SELECT TOP(1) (data_fim) FROM dbo.Rastreabilidade_Montagem WHERE codigo = @Codigo ORDER BY data_fim DESC) AS data_fim FROM dbo.Rastreabilidade_Montagem WHERE codigo = @Codigo";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;


                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        rastreabilidadeMontagemInfo.Rastreabilidade = dr["codigo"].ToString();
                        rastreabilidadeMontagemInfo.DataInicio = Convert.ToDateTime(dr["data_inicio"]);
                        rastreabilidadeMontagemInfo.DataFim = Convert.ToDateTime(dr["data_fim"]);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return rastreabilidadeMontagemInfo;
        }

        public List<string> GetAllRastreabilidadesByData(DateTime dataInicio, DateTime dataFim)
        {
            List<string> lstRastreabilidade = new List<string>();

            try
            {
                string sSQL = @"SELECT DISTINCT(codigo) FROM dbo.Rastreabilidade_Montagem WHERE data_inicio >= @DataInicio AND data_fim <= @DataFim";

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
                string sSQL = @"SELECT DISTINCT(codigo) FROM dbo.Rastreabilidade_Montagem WHERE codigo LIKE '%" + codigo + "%'";

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

        public string GetLastRastreabilidade()
        {
            string rastreabilidade = string.Empty;

            try
            {
                string sSQL = @"SELECT TOP(1) codigo FROM dbo.Rastreabilidade_TesteFinal_Montagem ORDER BY data DESC ";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        rastreabilidade = dr["codigo"].ToString();
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return rastreabilidade;
        }

        public List<RastreabilidadeComponentesMontagemInfo> GetAllComponentesByCodigo(string codigo)
        {
            List<RastreabilidadeComponentesMontagemInfo> lstComponentes = new List<RastreabilidadeComponentesMontagemInfo>();

            try
            {
                string sSQL = @"SELECT cod_componente, descricao FROM dbo.Rastreabilidade_Componentes_Montagem WHERE codigo = @Codigo ORDER BY posto ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstComponentes.Add(AtribuirRastreabilidadeComponente(dr));
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

        public List<TorqueInfo> GetTorquesRala(string codigo)
        {
            List<TorqueInfo> lstTorques = new List<TorqueInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Rastreabilidade_Torques_Montagem WHERE codigo = @Codigo AND descricao = 'Rala' ORDER BY data ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstTorques.Add(AtribuirTorque(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstTorques;
        }

        public List<TorqueInfo> GetTorquesComponentes(string codigo)
        {
            List<TorqueInfo> lstTorques = new List<TorqueInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Rastreabilidade_Torques_Montagem WHERE codigo = @Codigo AND descricao = 'Componentes' ORDER BY data ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstTorques.Add(AtribuirTorque(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstTorques;
        }


        public List<TesteFinalInfo> GetTesteFinal(string codigo)
        {
            List<TesteFinalInfo> lstTesteFinal = new List<TesteFinalInfo>();

            try
            {
                string sSQL = @"SELECT angulo,forca,velocidade FROM dbo.Rastreabilidade_TesteFinal_Montagem WHERE codigo = @Codigo ORDER BY data ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstTesteFinal.Add(AtribuirTesteFinal(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstTesteFinal;
        }

        public List<TesteFinalInfo> GetEsquerdaTesteFinal(string codigo)
        {
            List<TesteFinalInfo> lstTesteFinal = new List<TesteFinalInfo>();

            try
            {
                string sSQL = @"SELECT TOP 50 PERCENT angulo,forca,velocidade FROM dbo.Rastreabilidade_TesteFinal_Montagem WHERE codigo = @Codigo AND angulo >= -3 AND angulo <= 3 ORDER BY data ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstTesteFinal.Add(AtribuirTesteFinal(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstTesteFinal;
        }

        public List<TesteFinalInfo> GetDireitaTesteFinal(string codigo)
        {
            List<TesteFinalInfo> lstTesteFinal = new List<TesteFinalInfo>();

            try
            {
                string sSQL = @"SELECT TOP 50 PERCENT angulo,forca,velocidade FROM dbo.Rastreabilidade_TesteFinal_Montagem WHERE codigo = @Codigo AND angulo <= 3 AND angulo >= -3 ORDER BY data DESC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar, 20);
                parametros[0].Value = codigo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstTesteFinal.Add(AtribuirTesteFinal(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstTesteFinal;
        }

        public List<string> SearchComponente(string codigo)
        {
            List<string> lstCodigoRastreabilidade = new List<string>();

            try
            {
                string sSQL = @"SELECT DISTINCT(codigo) FROM dbo.Rastreabilidade_Componentes_Montagem WHERE cod_componente LIKE '%" + codigo + "%'"; ;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstCodigoRastreabilidade.Add(dr["codigo"].ToString());
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstCodigoRastreabilidade;
        }

        public int GetIdFromFotoTorques(string rastreabilidade, string descricao)
        {
            int idFoto = 0;

            try
            {
                string sSQL = @"SELECT id_foto FROM dbo.Relaciona_Foto_Torques WHERE descricao = @Descricao AND codigo = @Rastreabilidade";

                SqlParameter[] parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@Rastreabilidade", SqlDbType.VarChar, 20);
                parametros[0].Value = rastreabilidade;

                parametros[1] = new SqlParameter("@Descricao", SqlDbType.VarChar, 20);
                parametros[1].Value = descricao;


                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        idFoto = Convert.ToInt32(dr["id_foto"]);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return idFoto;
        }

        public FotoTorquesInfo GetFotoTorquesById(int idFoto)
        {
            FotoTorquesInfo fotoTorque = new FotoTorquesInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Fotos_Torques WHERE id_foto = @Id";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                parametros[0].Value = idFoto;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        fotoTorque = AtribuirFotos(dr);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return fotoTorque;
        }



        private FotoTorquesInfo AtribuirFotos(SqlDataReader dr)
        {
            FotoTorquesInfo fotoTorque = new FotoTorquesInfo();

            try
            {
                fotoTorque.Descricao = dr["descricao"].ToString();
                fotoTorque.IdFoto = Convert.ToInt32(dr["id_foto"]);
                fotoTorque.TipoTorque = dr["tipo_torque"].ToString();

                if (dr.IsDBNull(dr.GetOrdinal("foto"))) fotoTorque.Foto = null;
                else
                {
                    byte[] foto = (byte[])(dr["foto"]);
                    if (foto == null)
                    {
                        fotoTorque.Foto = null;
                    }
                    else
                    {
                        fotoTorque.Foto = foto;
                        fotoTorque.StringFoto = "data:image/png;base64," + Convert.ToBase64String(foto, 0, foto.Length);

                    }
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return fotoTorque;
        }


        private TesteFinalInfo AtribuirTesteFinal(SqlDataReader dr)
        {
            TesteFinalInfo testeFinalInfo = new TesteFinalInfo();

            try
            {
                testeFinalInfo.Angulo = Convert.ToDouble(dr["angulo"]);
                testeFinalInfo.Forca = Convert.ToDouble(dr["forca"]);
                testeFinalInfo.Velocidade = Convert.ToDouble(dr["velocidade"]);

            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return testeFinalInfo;
        }


        private TorqueInfo AtribuirTorque(SqlDataReader dr)
        {
            TorqueInfo torqueInfo = new TorqueInfo();

            try
            {
                torqueInfo.Codigo = dr["codigo"].ToString();
                torqueInfo.Descricao = dr["descricao"].ToString();
                torqueInfo.Data = Convert.ToDateTime(dr["data"]);
                torqueInfo.Angulo = Convert.ToInt32(dr["angulo"]);
                torqueInfo.Torque = Convert.ToInt32(dr["torque"]);
                torqueInfo.Operador = dr["operador"].ToString();
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return torqueInfo;
        }

        private RastreabilidadeComponentesMontagemInfo AtribuirRastreabilidadeComponente(SqlDataReader dr)
        {
            RastreabilidadeComponentesMontagemInfo rastreabilidadeComponentesInfo = new RastreabilidadeComponentesMontagemInfo();

            try
            {
                rastreabilidadeComponentesInfo.CodigoComponente = dr["cod_componente"].ToString();
                rastreabilidadeComponentesInfo.Descricao = dr["descricao"].ToString();
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return rastreabilidadeComponentesInfo;
        }

        private RastreabilidadeMontagemInfo AtribuirRastreabilidade(SqlDataReader dr)
        {
            RastreabilidadeMontagemInfo rastreabilidadeInfo = new RastreabilidadeMontagemInfo();

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
    }
}