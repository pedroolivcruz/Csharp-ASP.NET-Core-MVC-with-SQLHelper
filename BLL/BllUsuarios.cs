using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Newtonsoft.Json;


namespace Conectasys.Portal.BLL
{
    public class BllUsuarios
    {
        string fileName = Config.RootFolder + "\\Repositories\\Usuarios.json";

        public List<UsuarioInfo> GetAll()
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText);
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                lstUsuarios = dalUsuarios.GetAll();
            }

            return lstUsuarios;  
        }

        public List<UsuarioInfo> GetAllByMatricula(string matricula)
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText).Where(x => x.Matricula.Contains(matricula)).ToList();
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                lstUsuarios = dalUsuarios.GetAllByMatricula(matricula);
            }

            return lstUsuarios;
        }

        public List<UsuarioInfo> GetAllByNome(string nome)
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText).Where(x => x.Nome.Contains(nome)).ToList();
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                lstUsuarios = dalUsuarios.GetAllByNome(nome);
            }

            return lstUsuarios;
        }

        public List<UsuarioInfo> GetAllByPermissao(int permissao)
        {
            List<UsuarioInfo> lstUsuarios = new List<UsuarioInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText).Where(x => x.Permissao == permissao).ToList();
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                lstUsuarios = dalUsuarios.GetAllByPermissao(permissao);
            }

            return lstUsuarios;
        }


        public UsuarioInfo ObterUsuarioPorMatricula(string matricula)
        {
            UsuarioInfo usuario = new UsuarioInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                usuario = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText).Find(x => x.Matricula == matricula);
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                usuario = dalUsuarios.GetByMatricula(matricula);
            }

            return usuario;
        }

        public bool HasUsuario(string matricula)
        {
            bool existeUsuario  = false;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                existeUsuario = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText).Where(x => x.Matricula == matricula).Any();
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                existeUsuario = dalUsuarios.HasUsuario(matricula);
            }

            return existeUsuario;

        }

        public bool Insert(UsuarioInfo usuario)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<UsuarioInfo> lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText);

                if (!lstUsuarios.Where(x => x.Matricula == usuario.Matricula).Any())
                {
                    lstUsuarios.Add(new UsuarioInfo
                    {
                        Matricula = usuario.Matricula,
                        Logon = usuario.Logon,
                        Nome = usuario.Nome,
                        Permissao = usuario.Permissao
                    });
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstUsuarios));
                }
                else retorno = false;
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                retorno = dalUsuarios.Insert(usuario);
            }

            return retorno;
        }

        public bool Update(string matricula,UsuarioInfo usuario)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<UsuarioInfo> lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText);

                if (matricula == usuario.Matricula)
                {
                    lstUsuarios.Find(x => x.Matricula == matricula).Matricula = usuario.Matricula;
                    lstUsuarios.Find(x => x.Matricula == matricula).Logon = usuario.Logon;
                    lstUsuarios.Find(x => x.Matricula == matricula).Nome = usuario.Nome;
                    lstUsuarios.Find(x => x.Matricula == matricula).Permissao = usuario.Permissao;

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstUsuarios));
                }
                else if(!lstUsuarios.Where(x => x.Matricula == usuario.Matricula).Any())
                {
                    lstUsuarios.Find(x => x.Matricula == matricula).Matricula = usuario.Matricula;
                    lstUsuarios.Find(x => x.Matricula == matricula).Logon = usuario.Logon;
                    lstUsuarios.Find(x => x.Matricula == matricula).Nome = usuario.Nome;
                    lstUsuarios.Find(x => x.Matricula == matricula).Permissao = usuario.Permissao;

                    File.WriteAllText(fileName, JsonConvert.SerializeObject(lstUsuarios));
                }
                else retorno = false;
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                retorno = dalUsuarios.Update(matricula, usuario);
            }

            return retorno;
        }

        public bool Delete(string matricula)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<UsuarioInfo> lstUsuarios = JsonConvert.DeserializeObject<List<UsuarioInfo>>(fileText);

                lstUsuarios.RemoveAll(x => x.Matricula == matricula);
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstUsuarios));
            }
            else
            {
                DalUsuarios dalUsuarios = new DalUsuarios();
                retorno = dalUsuarios.Delete(matricula);
            }

            return retorno;
        }
    }
}