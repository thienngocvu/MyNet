namespace MyNet.Domain.Interfaces
{
    /// <summary>
    /// Generic interface for entities that have an identifier
    /// </summary>
    /// <typeparam name="T">The type of the entity's identifier</typeparam>
    public interface IEntity<out T>
    {
        T Id { get; }
    }
}
