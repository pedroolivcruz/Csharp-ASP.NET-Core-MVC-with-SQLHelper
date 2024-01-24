using System.Data;
using Conectasys.Portal.Models;
using System.Transactions;
using System.Data.SqlClient;


namespace Conectasys.Portal.DAL
{
    public class DalChecklistMontagem
    {
        public List<ChecklistMontagemInfo> GetAllByPosto(int posto)
        {
            List<ChecklistMontagemInfo> lstCheckListsMontagem = new List<ChecklistMontagemInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Checklist_Montagem WHERE posto = @Posto ORDER BY sequencia ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[0].Value = posto;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstCheckListsMontagem.Add(AtribuirChecklistsMontagem(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return lstCheckListsMontagem;
        }

        public List<ChecklistMontagemInfo> GetAllByPosto2(int posto, int numPrograma)
        {
            List<ChecklistMontagemInfo> lstCheckListsMontagem = new List<ChecklistMontagemInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Checklist_Montagem WHERE posto = @Posto AND num_programa = @NumeroPrograma ORDER BY sequencia ASC";

                SqlParameter[] parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[0].Value = posto;

                parametros[1] = new SqlParameter("@NumeroPrograma", SqlDbType.Int);
                parametros[1].Value = numPrograma;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstCheckListsMontagem.Add(AtribuirChecklistsMontagem(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return lstCheckListsMontagem;
        }

        public ChecklistMontagemInfo GetById(int id)
        {
            ChecklistMontagemInfo checklistsMontagemInfo = new ChecklistMontagemInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Checklist_Montagem WHERE id_checklist = @Id ORDER BY sequencia ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Id", SqlDbType.BigInt);
                parametros[0].Value = id;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        checklistsMontagemInfo = AtribuirChecklistsMontagem(dr);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return checklistsMontagemInfo;
        }

        public bool Insert(ChecklistMontagemInfo checkListMontagemInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    string sSQL = @"INSERT INTO dbo.Cadastro_Checklist_Montagem
                                                                       (cod_material
                                                                       ,data
                                                                       ,posto
                                                                       ,sequencia
                                                                       ,descricao
                                                                       ,ler_etiqueta
                                                                       ,gerar_rastreabilidade
                                                                       ,tipo_confirmacao
                                                                       ,fl_ativo
                                                                       ,valor_confirmacao
                                                                       ,foto
                                                                       ,num_programa)
                                                                 VALUES
                                                                       (@cod_material
                                                                       ,@data
                                                                       ,@posto
                                                                       ,@sequencia
                                                                       ,@descricao
                                                                       ,@ler_etiqueta
                                                                       ,@gerar_rastreabilidade
                                                                       ,@tipo_confirmacao
                                                                       ,@fl_ativo
                                                                       ,@valor_confirmacao
                                                                       ,@foto
                                                                       ,@num_programa)";
                    
                    SqlParameter[] parametros = new SqlParameter[14];

                    parametros[0] = new SqlParameter("@cod_material", SqlDbType.VarChar, 20);
                    parametros[0].Value = checkListMontagemInfo.CodigoMaterial;

                    parametros[1] = new SqlParameter("@data", SqlDbType.DateTime);
                    parametros[1].Value = checkListMontagemInfo.Data;

                    parametros[2] = new SqlParameter("@posto", SqlDbType.Int);
                    parametros[2].Value = checkListMontagemInfo.Posto;

                    parametros[3] = new SqlParameter("@sequencia", SqlDbType.Int);
                    parametros[3].Value = checkListMontagemInfo.Sequencia;

                    parametros[4] = new SqlParameter("@descricao", SqlDbType.VarChar, 200);
                    parametros[4].Value = checkListMontagemInfo.Descricao;

                    parametros[5] = new SqlParameter("@ler_etiqueta", SqlDbType.Bit);
                    parametros[5].Value = checkListMontagemInfo.LerEtiqueta;

                    parametros[6] = new SqlParameter("@gerar_rastreabilidade", SqlDbType.Bit);
                    parametros[6].Value = checkListMontagemInfo.GerarRastreabilidade;

                    parametros[7] = new SqlParameter("@tipo_confirmacao", SqlDbType.Bit);
                    parametros[7].Value = checkListMontagemInfo.TipoConfirmacao;

                    parametros[8] = new SqlParameter("@fl_ativo", SqlDbType.Bit);
                    parametros[8].Value = checkListMontagemInfo.Ativo;

                    parametros[9] = new SqlParameter("@valor_confirmacao", SqlDbType.Int);
                    parametros[9].Value = checkListMontagemInfo.ValorConfirmacao;

                    parametros[10] = new SqlParameter("@foto", SqlDbType.Image);
                    parametros[10].Value = checkListMontagemInfo.Foto;
                    
                    parametros[11] = new SqlParameter("@num_programa", SqlDbType.Int);
                    parametros[11].Value = checkListMontagemInfo.NumeroPrograma;

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

        public bool Update(int id, ChecklistMontagemInfo checkListMontagemInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL =  @"UPDATE dbo.Cadastro_Checklist_Montagem SET
                                                                                                                  
                                                           posto = @Posto,
                                                           sequencia = @Sequencia,
                                                           descricao = @Descricao,
                                                           ler_etiqueta = @Ler_Etiqueta,
                                                           gerar_rastreabilidade = @Gerar_Rastreabilidade,
                                                           tipo_confirmacao = @Tipo_Confirmacao,
                                                           valor_confirmacao = @Valor_Confirmacao,
                                                           num_programa = @Num_Programa,
                                                           fl_ativo = @Fl_Ativo,
                                                           foto = @Foto
                                                           
                                    WHERE id_checklist = @Id";
                    
                    SqlParameter[] parametros = new SqlParameter[13];

                    parametros[0] = new SqlParameter("@Posto", SqlDbType.Int);
                    parametros[0].Value = checkListMontagemInfo.Posto;

                    parametros[1] = new SqlParameter("@Sequencia", SqlDbType.Int);
                    parametros[1].Value = checkListMontagemInfo.Sequencia;

                    parametros[2] = new SqlParameter("@Descricao", SqlDbType.VarChar, 200);
                    parametros[2].Value = checkListMontagemInfo.Descricao;

                    parametros[3] = new SqlParameter("@Ler_Etiqueta", SqlDbType.Bit);
                    parametros[3].Value = checkListMontagemInfo.LerEtiqueta;

                    parametros[4] = new SqlParameter("@Gerar_Rastreabilidade", SqlDbType.Bit);
                    parametros[4].Value = checkListMontagemInfo.GerarRastreabilidade;

                    parametros[5] = new SqlParameter("@Tipo_Confirmacao", SqlDbType.Bit);
                    parametros[5].Value = checkListMontagemInfo.TipoConfirmacao;

                    parametros[6] = new SqlParameter("@Fl_Ativo", SqlDbType.Bit);
                    parametros[6].Value = checkListMontagemInfo.Ativo;

                    parametros[7] = new SqlParameter("@Valor_Confirmacao", SqlDbType.Int);
                    parametros[7].Value = checkListMontagemInfo.ValorConfirmacao;

                    parametros[8] = new SqlParameter("@Foto", SqlDbType.Image);
                    parametros[8].Value = checkListMontagemInfo.Foto;
                    
                    parametros[9] = new SqlParameter("@Num_Programa", SqlDbType.Int);
                    parametros[9].Value = checkListMontagemInfo.NumeroPrograma;

                    parametros[10] = new SqlParameter("@Id", SqlDbType.BigInt);
                    parametros[10].Value = id;

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
                    string sSQL = @"DELETE FROM dbo.Cadastro_Checklist_Montagem        
                                    WHERE id_checklist = @Id";


                    SqlParameter[] parametros = new SqlParameter[1];

                    parametros[0] = new SqlParameter("@Id", SqlDbType.BigInt);
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

        private ChecklistMontagemInfo AtribuirChecklistsMontagem(SqlDataReader dr)
        {
            ChecklistMontagemInfo checkListsMontagem = new ChecklistMontagemInfo();

            try
            {
                checkListsMontagem.IdChecklist = Convert.ToInt32(dr["id_checklist"]);
                checkListsMontagem.CodigoMaterial = dr["cod_material"].ToString();
                checkListsMontagem.Posto = Convert.ToInt32(dr["posto"]);
                checkListsMontagem.Sequencia = Convert.ToInt32(dr["sequencia"]);
                checkListsMontagem.Data = Convert.ToDateTime(dr["data"]);
                checkListsMontagem.Descricao = dr["descricao"].ToString();
                checkListsMontagem.LerEtiqueta = Convert.ToBoolean(dr["ler_etiqueta"]);
                checkListsMontagem.GerarRastreabilidade = Convert.ToBoolean(dr["gerar_rastreabilidade"]);
                checkListsMontagem.TipoConfirmacao = Convert.ToBoolean(dr["tipo_confirmacao"]);
                checkListsMontagem.ValorConfirmacao = Convert.ToInt32(dr["valor_confirmacao"]);
                checkListsMontagem.NumeroPrograma = Convert.ToInt32(dr["num_programa"]);
                checkListsMontagem.Ativo = Convert.ToBoolean(dr["fl_ativo"]);

                if (dr.IsDBNull(dr.GetOrdinal("foto"))) checkListsMontagem.Foto = null;
                else
                {
                    byte[] foto = (byte[])(dr["foto"]);
                    if (foto == null)
                    {
                        checkListsMontagem.Foto = null;
                    }
                    else
                    {
                        checkListsMontagem.Foto = foto;
                        checkListsMontagem.StringFoto = "data:image/png;base64," + Convert.ToBase64String(foto, 0, foto.Length);
                    }
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return checkListsMontagem;
        }
    }
}        

