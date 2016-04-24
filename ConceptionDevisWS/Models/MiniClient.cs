using System;

namespace ConceptionDevisWS.Models
{
    /// <summary>
    /// Model used in web responses to prevent a recursive structure with <see cref="ConceptionDevisWS.Models.Project"/>.<see cref="ConceptionDevisWS.Models.Client"/>.<see cref="ConceptionDevisWS.Models.Project"/>s.
    /// </summary>
    public class MiniClient
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Birdthdate { get; set; }
        
        public MiniClient() { }
        
        public MiniClient(Client client)
        {
            Id = client.Id;
            Reference = client.Referrence;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Address = client.Address;
            City = client.City;
            ZipCode = client.ZipCode;
            Phone = client.Phone;
            Email = client.Email;
            Birdthdate = client.Birdthdate;
        } 

    }
}