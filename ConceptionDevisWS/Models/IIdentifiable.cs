namespace ConceptionDevisWS.Models
{
    /// <summary>
    /// Interface for all <see cref="ConceptionDevisWS.Models"/> to implement. Only states there must be an identity property called Id.
    /// </summary>
    public interface IIdentifiable
    {
        int Id { get; set; }
    }
}