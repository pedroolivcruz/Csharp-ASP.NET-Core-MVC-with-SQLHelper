using Conectasys.Portal.DAL;
using Conectasys.Portal.Models;
using Newtonsoft.Json;


namespace Conectasys.Portal.BLL
{
    public class BllFotoTorques
    {
        string fileName = Config.RootFolder + "\\Repositories\\GetFotoTorquesById.json";


        public List<FotoTorquesInfo> GetAll()
        {   
            List<FotoTorquesInfo> lstFotosTorques = new List<FotoTorquesInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstFotosTorques = JsonConvert.DeserializeObject<List<FotoTorquesInfo>>(fileText).OrderBy(x => x.IdFoto).ToList();

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstFotosTorques));
            }
            else
            {
                DalFotoTorques dalFotoTorques = new DalFotoTorques();
                lstFotosTorques = dalFotoTorques.GetAll();
            }

            return lstFotosTorques;
        }

        public List<FotoTorquesInfo> GetAllByDescricao(string descricao)
        {
            List<FotoTorquesInfo> lstFotosTorques = new List<FotoTorquesInfo>();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                lstFotosTorques = JsonConvert.DeserializeObject<List<FotoTorquesInfo>>(fileText).Where(x => x.Descricao.Contains(descricao)).ToList();
            }
            else
            {
                DalFotoTorques dalFotoTorques = new DalFotoTorques();
                lstFotosTorques = dalFotoTorques.GetAllByDescricao(descricao);
            }
            return lstFotosTorques;
        }

        public FotoTorquesInfo GetById(int id)
        {
            FotoTorquesInfo fotoTorques = new FotoTorquesInfo();

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                fotoTorques = JsonConvert.DeserializeObject<List<FotoTorquesInfo>>(fileText).Find(x => x.IdFoto == id);
            }
            else
            {
                DalFotoTorques dalFotoTorques = new DalFotoTorques();
                fotoTorques = dalFotoTorques.GetById(id);
            }

            return fotoTorques;
        }

        public bool Insert(FotoTorquesInfo fotoTorque)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<FotoTorquesInfo> lstFotoTorques = JsonConvert.DeserializeObject<List<FotoTorquesInfo>>(fileText);

                int lastId = lstFotoTorques.OrderBy(x => x.IdFoto).Last().IdFoto;

                lstFotoTorques.Add(new FotoTorquesInfo
                {     
                    IdFoto = lastId + 1,
                    Descricao = fotoTorque.Descricao,
                    TipoTorque = fotoTorque.TipoTorque,
                    StringFoto = "data:image/png;base64," + Convert.ToBase64String(fotoTorque.Foto, 0, fotoTorque.Foto.Length)
                });
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstFotoTorques));
            }
            else
            {
                DalFotoTorques dalFotoTorques = new DalFotoTorques();
                retorno = dalFotoTorques.Insert(fotoTorque);
            }

            return retorno;
        }

        public bool Update(int id ,FotoTorquesInfo fotoTorque)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<FotoTorquesInfo> lstFotoTorques = JsonConvert.DeserializeObject<List<FotoTorquesInfo>>(fileText);

                lstFotoTorques.Find(x => x.IdFoto == id).Descricao = fotoTorque.Descricao;
                lstFotoTorques.Find(x => x.IdFoto == id).Foto = fotoTorque.Foto;
                lstFotoTorques.Find(x => x.IdFoto == id).StringFoto = "data:image/png;base64," + Convert.ToBase64String(fotoTorque.Foto, 0, fotoTorque.Foto.Length);

                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstFotoTorques));
            }
            else
            {
                DalFotoTorques dalFotoTorques = new DalFotoTorques();
                retorno = dalFotoTorques.Update(id,fotoTorque); 
            }

            return retorno;
        }

        public bool Delete(int id)
        {
            bool retorno = true;

            if (Config.IsDemostration)
            {
                string fileText = File.ReadAllText(fileName);
                List<FotoTorquesInfo> lstFotoTorques = JsonConvert.DeserializeObject<List<FotoTorquesInfo>>(fileText);

                lstFotoTorques.RemoveAll(x => x.IdFoto == id);
                File.WriteAllText(fileName, JsonConvert.SerializeObject(lstFotoTorques));
            }
            else
            {
                DalFotoTorques dalFotoTorques = new DalFotoTorques();
                retorno = dalFotoTorques.Delete(id);
            }

            return retorno;
        }
    }
}