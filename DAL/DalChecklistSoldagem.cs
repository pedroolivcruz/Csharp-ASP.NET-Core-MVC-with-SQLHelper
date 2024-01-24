using System.Data;
using Conectasys.Portal.Models;
using System.Data.SqlClient;
using System.Transactions;


namespace Conectasys.Portal.DAL
{
    public class DalChecklistSoldagem
    {
        public List<ChecklistSoldagemInfo> GetAllByPosto(int posto)
        {
            List<ChecklistSoldagemInfo> lstCheckListsSoldagem = new List<ChecklistSoldagemInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Checklist_Soldagem WHERE posto = @Posto ORDER BY sequencia ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Posto", SqlDbType.Int);
                parametros[0].Value = posto;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        lstCheckListsSoldagem.Add(AtribuirCheckListsSoldagem(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return lstCheckListsSoldagem;
        }

        public ChecklistSoldagemInfo GetById(int id)
        {
            ChecklistSoldagemInfo checklistsSoldagemInfo = new ChecklistSoldagemInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Checklist_Soldagem WHERE id_checklist = @Id ORDER BY sequencia ASC";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Id", SqlDbType.BigInt);
                parametros[0].Value = id;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        checklistsSoldagemInfo = AtribuirCheckListsSoldagem(dr);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return checklistsSoldagemInfo;
        }

        public bool Insert(ChecklistSoldagemInfo checklistSoldagemInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"INSERT INTO dbo.Cadastro_Checklist_Soldagem
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
                    parametros[0].Value = checklistSoldagemInfo.CodigoMaterial;

                    parametros[1] = new SqlParameter("@data", SqlDbType.DateTime);
                    parametros[1].Value = checklistSoldagemInfo.Data;

                    parametros[2] = new SqlParameter("@posto", SqlDbType.Int);
                    parametros[2].Value = checklistSoldagemInfo.Posto;

                    parametros[3] = new SqlParameter("@sequencia", SqlDbType.Int);
                    parametros[3].Value = checklistSoldagemInfo.Sequencia;

                    parametros[4] = new SqlParameter("@descricao", SqlDbType.VarChar, 200);
                    parametros[4].Value = checklistSoldagemInfo.Descricao;

                    parametros[5] = new SqlParameter("@ler_etiqueta", SqlDbType.Bit);
                    parametros[5].Value = checklistSoldagemInfo.LerEtiqueta;

                    parametros[6] = new SqlParameter("@gerar_rastreabilidade", SqlDbType.Bit);
                    parametros[6].Value = checklistSoldagemInfo.GerarRastreabilidade;

                    parametros[7] = new SqlParameter("@tipo_confirmacao", SqlDbType.Bit);
                    parametros[7].Value = checklistSoldagemInfo.TipoConfirmacao;

                    parametros[8] = new SqlParameter("@fl_ativo", SqlDbType.Bit);
                    parametros[8].Value = checklistSoldagemInfo.Ativo;

                    parametros[9] = new SqlParameter("@valor_confirmacao", SqlDbType.Int);
                    parametros[9].Value = checklistSoldagemInfo.ValorConfirmacao;

                    parametros[10] = new SqlParameter("@foto", SqlDbType.Image);
                    parametros[10].Value = checklistSoldagemInfo.Foto;
                    
                    parametros[11] = new SqlParameter("@num_programa", SqlDbType.Int);
                    parametros[11].Value = checklistSoldagemInfo.NumeroPrograma;

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

        public bool Update(int id ,ChecklistSoldagemInfo checklistSoldagemInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"UPDATE dbo.Cadastro_Checklist_Soldagem SET
                                                                                                               
                                                           posto = @Posto,
                                                           sequencia = @Sequencia,
                                                           descricao = @Descricao,
                                                           ler_etiqueta = @Ler_Etiqueta,
                                                           gerar_rastreabilidade = @Gerar_Rastreabilidade,
                                                           tipo_confirmacao = @Tipo_Confirmacao,
                                                           valor_confirmacao = @Valor_Confirmacao,
                                                           fl_ativo = @Fl_Ativo,
                                                           foto = @Foto
                                                           
                                    WHERE id_checklist = @Id";
                   
                    SqlParameter[] parametros = new SqlParameter[10];

                    parametros[0] = new SqlParameter("@Posto", SqlDbType.Int);
                    parametros[0].Value = checklistSoldagemInfo.Posto;

                    parametros[1] = new SqlParameter("@Sequencia", SqlDbType.Int);
                    parametros[1].Value = checklistSoldagemInfo.Sequencia;

                    parametros[2] = new SqlParameter("@Descricao", SqlDbType.VarChar, 200);
                    parametros[2].Value = checklistSoldagemInfo.Descricao;

                    parametros[3] = new SqlParameter("@Ler_Etiqueta", SqlDbType.Bit);
                    parametros[3].Value = checklistSoldagemInfo.LerEtiqueta;

                    parametros[4] = new SqlParameter("@Gerar_Rastreabilidade", SqlDbType.Bit);
                    parametros[4].Value = checklistSoldagemInfo.GerarRastreabilidade;

                    parametros[5] = new SqlParameter("@Tipo_Confirmacao", SqlDbType.Bit);
                    parametros[5].Value = checklistSoldagemInfo.TipoConfirmacao;

                    parametros[6] = new SqlParameter("@Fl_Ativo", SqlDbType.Bit);
                    parametros[6].Value = checklistSoldagemInfo.Ativo;

                    parametros[7] = new SqlParameter("@Valor_Confirmacao", SqlDbType.Int);
                    parametros[7].Value = checklistSoldagemInfo.ValorConfirmacao;

                    parametros[8] = new SqlParameter("@Foto", SqlDbType.Image);
                    parametros[8].Value = checklistSoldagemInfo.Foto;
                    
                    parametros[9] = new SqlParameter("@Id", SqlDbType.BigInt);
                    parametros[9].Value = id;


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
                    string sSQL = @"DELETE FROM dbo.Cadastro_Checklist_Soldagem        
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

        private ChecklistSoldagemInfo AtribuirCheckListsSoldagem(SqlDataReader dr)
        {
            ChecklistSoldagemInfo checklistsSoldagem = new ChecklistSoldagemInfo();

            try
            {
                checklistsSoldagem.IdChecklist = Convert.ToInt32(dr["id_checklist"]);
                checklistsSoldagem.CodigoMaterial = dr["cod_material"].ToString();
                checklistsSoldagem.Posto = Convert.ToInt32(dr["posto"]);
                checklistsSoldagem.Sequencia = Convert.ToInt32(dr["sequencia"]);
                checklistsSoldagem.Data = Convert.ToDateTime(dr["data"]);
                checklistsSoldagem.Descricao = dr["descricao"].ToString();
                checklistsSoldagem.LerEtiqueta = Convert.ToBoolean(dr["ler_etiqueta"]);
                checklistsSoldagem.GerarRastreabilidade = Convert.ToBoolean(dr["gerar_rastreabilidade"]);
                checklistsSoldagem.TipoConfirmacao = Convert.ToBoolean(dr["tipo_confirmacao"]);
                checklistsSoldagem.ValorConfirmacao = Convert.ToInt32(dr["valor_confirmacao"]);
                checklistsSoldagem.NumeroPrograma = Convert.ToInt32(dr["num_programa"]);
                checklistsSoldagem.Ativo = Convert.ToBoolean(dr["fl_ativo"]);

                if (dr.IsDBNull(dr.GetOrdinal("foto"))) checklistsSoldagem.Foto = null;
                else
                {
                    byte[] foto = (byte[])(dr["foto"]);
                    if (foto == null)
                    {
                        checklistsSoldagem.Foto = null;
                    }
                    else
                    {
                        checklistsSoldagem.Foto = foto;
                        checklistsSoldagem.StringFoto = "data:image/png;base64," + Convert.ToBase64String(foto, 0, foto.Length);
                    }
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return checklistsSoldagem;
        }
    }
}