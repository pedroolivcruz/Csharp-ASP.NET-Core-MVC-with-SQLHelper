using Conectasys.Portal.Models;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;


namespace Conectasys.Portal.DAL
{
    public class DalUsuarios
    {
        public List<UsuarioInfo> GetAll()
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Usuarios WHERE permission = 0 ORDER BY id_pernr_sap ASC";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstUsuarios.Add(AtribuirUsuario(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstUsuarios;
        }

        public List<UsuarioInfo> GetAllByMatricula(string matricula)
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Usuarios WHERE id_pernr_sap LIKE '%" + matricula + "%' AND permission = 0";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstUsuarios.Add(AtribuirUsuario(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstUsuarios;
        }

        public List<UsuarioInfo> GetAllByPermissao(int permissao)
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Usuarios WHERE permission =" + permissao;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstUsuarios.Add(AtribuirUsuario(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstUsuarios;
        }

        public List<UsuarioInfo> GetAllByNome(string nome)
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Usuarios WHERE mn_user LIKE '%" + nome + "%'";

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL))
                {
                    while (dr.Read())
                    {
                        lstUsuarios.Add(AtribuirUsuario(dr));
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return lstUsuarios;
        }

        public UsuarioInfo GetByMatricula(string matricula)
        {
            UsuarioInfo usuario = new UsuarioInfo();

            try
            {
                string sSQL = @"SELECT * FROM dbo.Usuarios WHERE id_pernr_sap = @matricula";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@matricula", SqlDbType.VarChar,20);
                parametros[0].Value = matricula;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        usuario = AtribuirUsuario(dr);
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return usuario;
        }

        public bool HasUsuario(string usuario)
        {
            bool flag = false;

            try
            {
                string sSQL = @"SELECT * FROM dbo.Usuarios WHERE id_pernr_sap = @Usuario AND permission = 0";

                SqlParameter[] parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@Usuario", SqlDbType.VarChar, 20);
                parametros[0].Value = usuario;

                using (SqlDataReader dr = SqlHelper.ExecuteReader(Config.ConexaoDB, CommandType.Text, sSQL, parametros))
                {
                    while (dr.Read())
                    {
                        flag = dr.HasRows;
                    }

                    dr.Close();
                }
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }

            return flag;
        }


        public bool Insert(UsuarioInfo usuario)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                try
                {
                    string sSQL = @"INSERT INTO dbo.Usuarios
                                                                       (id_pernr_sap,
                                                                        cd_logon,
                                                                        nm_user,
                                                                        permission)
                                                                 VALUES
                                                                       (@matricula
                                                                       ,@logon
                                                                       ,@nome
                                                                       ,@permissao)";
                   
                    SqlParameter[] parametros = new SqlParameter[4];

                    parametros[0] = new SqlParameter("@matricula", SqlDbType.VarChar,20);
                    parametros[0].Value = usuario.Matricula;

                    parametros[1] = new SqlParameter("@nome", SqlDbType.VarChar, 200);
                    parametros[1].Value = usuario.Nome;

                    parametros[2] = new SqlParameter("@logon", SqlDbType.VarChar, 20);
                    parametros[2].Value = usuario.Logon;

                    parametros[3] = new SqlParameter("@permissao", SqlDbType.Int);
                    parametros[3].Value = usuario.Permissao;

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

        public bool Update(string matricula, UsuarioInfo usuario)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    string sSQL = @"UPDATE dbo.Usuarios SET
                                                           
                                                           id_pernr_sap = @matricula,
                                                           cd_logon = @logon,
                                                           nm_user = @nome,
                                                           permission = @permissao
                                                                                                                      
                                    WHERE id_pernr_sap = @MatriculaAntiga";
                    

                    SqlParameter[] parametros = new SqlParameter[5];

                    parametros[0] = new SqlParameter("@MatriculaAntiga", SqlDbType.VarChar,20);
                    parametros[0].Value = matricula;

                    parametros[1] = new SqlParameter("@matricula", SqlDbType.VarChar,20);
                    parametros[1].Value = usuario.Matricula;

                    parametros[2] = new SqlParameter("@nome", SqlDbType.VarChar, 200);
                    parametros[2].Value = usuario.Nome;

                    parametros[3] = new SqlParameter("@logon", SqlDbType.VarChar, 20);
                    parametros[3].Value = usuario.Logon;

                    parametros[4] = new SqlParameter("@permissao", SqlDbType.Int);
                    parametros[4].Value = usuario.Permissao;

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

        public bool Delete(string matricula)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sSQL = @"DELETE FROM dbo.Usuarios        
                                    WHERE id_pernr_sap = @Matricula";


                    SqlParameter[] parametros = new SqlParameter[1];

                    parametros[0] = new SqlParameter("@Matricula", SqlDbType.VarChar,20);
                    parametros[0].Value = matricula;

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



        private UsuarioInfo AtribuirUsuario(SqlDataReader dr)
        {
            UsuarioInfo usuario = new UsuarioInfo();

            try
            {
                usuario.Nome = dr["nm_user"].ToString();
                usuario.Matricula = dr["id_pernr_sap"].ToString();
                usuario.Logon = dr["cd_logon"].ToString();
                usuario.Permissao = Convert.ToInt32(dr["permission"]);
            }
            catch (Exception Erro)
            {
                // Log.Gravar(Erro);
            }
            return usuario;
        }

    }
}