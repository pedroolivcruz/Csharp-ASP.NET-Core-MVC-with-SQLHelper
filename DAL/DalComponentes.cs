using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Conectasys.Portal.Models;


namespace Conectasys.Portal.DAL
{
    public class DalComponentes
    {
        public List<ComponenteInfo> SearchComponentes(int posto, int modelo)
        {
            List<ComponenteInfo> lstComponentes = new List<ComponenteInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Componentes_Montagem WHERE posto = @Posto AND modelo = @Modelo ORDER BY id_componente ASC";

                SqlParameter[] parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[0].Value = posto;

                parametros[1] = new SqlParameter("@Modelo", SqlDbType.Int);
                parametros[1].Value = modelo;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstComponentes.Add(AtribuirComponente(dr));
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

        public ComponenteInfo GetById(int id)
        {
            ComponenteInfo componente = new ComponenteInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Componentes_Montagem WHERE id_componente = @IdComponente";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@IdComponente", SqlDbType.Int);
                parametros[0].Value = id;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL,parametros))
                {
                    while (dr.Read())
                    {
                        componente = AtribuirComponente(dr);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return componente;
        }

        public bool Insert(ComponenteInfo componenteInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"INSERT INTO dbo.Cadastro_Componentes_Montagem
                                                                       (cod_material 
                                                                       ,posto
                                                                       ,descricao
                                                                       ,sequencia
                                                                       ,data
                                                                       ,fl_ativo
                                                                       ,modelo
                                                                       ,cod_sap
                                                                       )
                                                                 VALUES
                                                                       (@TipoMaterial
                                                                       ,@Posto
                                                                       ,@Descricao
                                                                       ,@Sequencia
                                                                       ,@Data
                                                                       ,@Ativo
                                                                       ,@Modelo
                                                                       ,@CodigoSAP)";

                  
                    SqlParameter[] parametros = new SqlParameter[8];

                    parametros[0] = new SqlParameter("@TipoMaterial", SqlDbType.VarChar, 20);
                    parametros[0].Value = componenteInfo.TipoMaterial;

                    parametros[1] = new SqlParameter("@Posto", SqlDbType.Int);
                    parametros[1].Value = componenteInfo.Posto;

                    parametros[2] = new SqlParameter("@Descricao", SqlDbType.VarChar, 200);
                    parametros[2].Value = componenteInfo.Descricao;

                    parametros[3] = new SqlParameter("@Sequencia", SqlDbType.Int);
                    parametros[3].Value = componenteInfo.Sequencia;

                    parametros[4] = new SqlParameter("@Data", SqlDbType.DateTime);
                    parametros[4].Value = DateTime.Now;

                    parametros[5] = new SqlParameter("@Ativo", SqlDbType.Bit);
                    parametros[5].Value = componenteInfo.Ativo;

                    parametros[6] = new SqlParameter("@CodigoSAP", SqlDbType.VarChar, 20);
                    parametros[6].Value = componenteInfo.CodigoSAP;

                    parametros[7] = new SqlParameter("@Modelo", SqlDbType.Int);
                    parametros[7].Value = componenteInfo.Modelo;

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

        public bool Update(ComponenteInfo componente)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"UPDATE dbo.Cadastro_Componentes_Montagem SET
                                                           
                                                           cod_material = @TipoMaterial,                                                         
                                                           posto = @Posto,
                                                           descricao = @Descricao,
                                                           sequencia = @Sequencia,
                                                           data = @Data,
                                                           fl_ativo = @Ativo,
                                                           cod_sap = @CodigoSAP
                                                                                                                      
                                    WHERE id_componente = @IdComponente";
                    

                    SqlParameter[] parametros = new SqlParameter[8];

                    parametros[0] = new SqlParameter("@CodigoSAP", SqlDbType.VarChar,20);
                    parametros[0].Value = componente.CodigoSAP;

                    parametros[1] = new SqlParameter("@Posto", SqlDbType.Int);
                    parametros[1].Value = componente.Posto;

                    parametros[2] = new SqlParameter("@Descricao", SqlDbType.VarChar, 200);
                    parametros[2].Value = componente.Descricao;

                    parametros[3] = new SqlParameter("@Sequencia", SqlDbType.Int);
                    parametros[3].Value = componente.Sequencia;

                    parametros[4] = new SqlParameter("@Data", SqlDbType.DateTime);
                    parametros[4].Value = DateTime.Now;

                    parametros[5] = new SqlParameter("@Ativo", SqlDbType.Bit);
                    parametros[5].Value = componente.Ativo;

                    parametros[6] = new SqlParameter("@TipoMaterial", SqlDbType.VarChar,20);
                    parametros[6].Value = componente.TipoMaterial;

                    parametros[7] = new SqlParameter("@IdComponente", SqlDbType.Int);
                    parametros[7].Value = componente.IdComponente;

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

        public bool Delete(int id)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"DELETE FROM dbo.Cadastro_Componentes_Montagem        
                                    WHERE id_componente = @IdComponente";


                    SqlParameter[] parametros = new SqlParameter[1];

                    parametros[0] = new SqlParameter("IdComponente", SqlDbType.Int);
                    parametros[0].Value = id;

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

        private ComponenteInfo AtribuirComponente(SqlDataReader dr)
        {
            ComponenteInfo componente = new ComponenteInfo();

            try
            {
                componente.Posto = Convert.ToInt32(dr["posto"]);
                componente.Sequencia = Convert.ToInt32(dr["sequencia"]);
                componente.Descricao = dr["descricao"].ToString();
                componente.TipoMaterial = dr["cod_material"].ToString();
                componente.Modelo = Convert.ToInt32(dr["modelo"]);
                componente.Ativo = Convert.ToBoolean(dr["fl_ativo"]);
                componente.CodigoSAP = dr["cod_sap"].ToString();
                componente.IdComponente = Convert.ToInt32(dr["id_componente"]);
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return componente;
        }
    }
}