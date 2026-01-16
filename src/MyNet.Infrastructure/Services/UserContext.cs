using MyNet.Application.Services;

namespace MyNet.Infrastructure.Services
{
    public class UserContext : IUserContext
    {
        private string _id = string.Empty;
        public string Id => _id;

        private string _name = string.Empty;
        public string Name => _name;

        private string _roleName = string.Empty;
        public string RoleName => _roleName;

        private readonly List<string> _rights = new List<string>();
        public IEnumerable<string> Rights => _rights;

        public IUserContext SetId(string id)
        {
            _id = id;
            return this;
        }

        public IUserContext SetName(string name)
        {
            _name = name;
            return this;
        }

        public IUserContext SetRoleName(string role)
        {
            _roleName = role;
            return this;
        }

        public IUserContext SetRights(IEnumerable<string> rights)
        {
            if (rights != null && rights.Any())
            {
                _rights.Clear();
                _rights.AddRange(rights);
            }
            return this;
        }
    }
}
