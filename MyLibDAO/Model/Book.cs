using System.Text.Json.Serialization;

namespace MyLibDAO.Model;

public partial class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string AuthorSurname { get; set; } = null!;
    public string Publisher { get; set; } = null!;
    public int Quantity { get; set; }
    [JsonIgnore]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>(); // navigation property
}
