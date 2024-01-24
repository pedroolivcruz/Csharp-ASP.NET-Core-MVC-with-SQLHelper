namespace Conectasys.Portal
{
    public class Session
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public Session(IHttpContextAccessor httpContextAccessor)
        {
            _session.SetString("MatriculaUsuario", string.Empty);
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
