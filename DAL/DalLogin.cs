using System.Data;
using System.Data.SqlClient;
using System.Transactions;


namespace Conectasys.Portal.DAL
{
    public class DalLogin
    {
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
    }   
}