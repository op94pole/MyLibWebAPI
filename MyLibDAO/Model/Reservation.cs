namespace MyLibDAO.Model;

public partial class Reservation
{
    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public virtual Book Book { get; set; } = null!; // navigation property
    public virtual User User { get; set; } = null!; // navigation property
}
