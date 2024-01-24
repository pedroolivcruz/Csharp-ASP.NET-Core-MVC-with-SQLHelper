using Conectasys.Portal.Models;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;


namespace Conectasys.Portal.DAL
{
    public class DalFotoTorques
    {
        public List<FotoTorquesInfo> GetAll()
        {
            List<FotoTorquesInfo> lstFotos = new List<FotoTorquesInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Fotos_Torques";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstFotos.Add(Atribuir(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstFotos;
        }

        public List<FotoTorquesInfo> GetAllByDescricao(string descricao)
        {
            List<FotoTorquesInfo> lstFotos = new List<FotoTorquesInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Fotos_Torques WHERE descricao LIKE '%" + descricao + "%'";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstFotos.Add(Atribuir(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstFotos;
        }

        public FotoTorquesInfo GetById(int id)
        {
            FotoTorquesInfo fotoTorque = new FotoTorquesInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Cadastro_Fotos_Torques WHERE id_foto = @id";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@id", SqlDbType.Int);
                parametros[0].Value = id;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL,parametros))
                {
                    while (dr.Read())
                    {
                        fotoTorque = Atribuir(dr);
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


        public bool Insert(FotoTorquesInfo fotoTorques)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    string sSQL = @"INSERT INTO dbo.Cadastro_Fotos_Torques
                                                                       (descricao
                                                                       ,tipo_torque
                                                                       ,foto)
                                                                 VALUES
                                                                       (@descricao
                                                                       ,@tipoTorque
                                                                       ,@foto)";

                    SqlParameter[] parametros = new SqlParameter[3];

                    parametros[0] = new SqlParameter("@descricao", SqlDbType.VarChar, 200);
                    parametros[0].Value = fotoTorques.Descricao;

                    parametros[1] = new SqlParameter("@tipoTorque", SqlDbType.VarChar, 50);
                    parametros[1].Value = fotoTorques.TipoTorque;

                    parametros[2] = new SqlParameter("@foto", SqlDbType.Image);
                    parametros[2].Value = fotoTorques.Foto;
                    
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

        public bool Update(int id, FotoTorquesInfo fotoTorquesInfo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"UPDATE dbo.Cadastro_Fotos_Torques SET
                                                           
                                                           descricao = @Descricao,
                                                           tipo_torque = @TipoTorque,
                                                           foto = @Foto
                                                           
                                    WHERE id_foto = @Id";
                    
                    SqlParameter[] parametros = new SqlParameter[4];

                    parametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                    parametros[0].Value = id;

                    parametros[1] = new SqlParameter("@Descricao", SqlDbType.VarChar, 200);
                    parametros[1].Value = fotoTorquesInfo.Descricao;

                    parametros[2] = new SqlParameter("@TipoTorque", SqlDbType.VarChar, 50);
                    parametros[2].Value = fotoTorquesInfo.TipoTorque;

                    parametros[3] = new SqlParameter("@Foto", SqlDbType.Image);
                    parametros[3].Value = fotoTorquesInfo.Foto;
                    
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
                    string sSQL = @"DELETE FROM dbo.Cadastro_Fotos_Torques        
                                    WHERE id_foto = @Id";


                    SqlParameter[] parametros = new SqlParameter[1];

                    parametros[0] = new SqlParameter("@Id", SqlDbType.Int);
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

        private FotoTorquesInfo Atribuir(SqlDataReader dr)
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
    }
}