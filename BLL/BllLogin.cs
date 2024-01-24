using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using Newtonsoft.Json;


namespace Conectasys.Portal.BLL
{
    public class BllLogin
    {
        string fileName = Config.RootFolder + "\\Repositories\\Usuarios.json";
        

        public bool HasUsuario(string matricula)
        {
            bool existeUsuario = false;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                existeUsuario = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText).Where(x => x.Matricula == matricula).Any();
            }
            else
            {
                DalLogin dalLogin = new DalLogin();
                existeUsuario = dalLogin.HasUsuario(matricula);
            }

            return existeUsuario;
        }
    }
}