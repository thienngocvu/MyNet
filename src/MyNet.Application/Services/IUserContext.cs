namespace MyNet.Application.Services
{
    public interface IUserContext
    {
        IUserContext SetId(string id);
        string Id { get; }
        IUserContext SetName(string name);
        string Name { get; }
        IUserContext SetRoleName(string role);
        string RoleName { get; }
        IEnumerable<string> Rights { get; }
        IUserContext SetRights(IEnumerable<string> rights);
    }
}
