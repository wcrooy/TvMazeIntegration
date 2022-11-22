namespace TvMazeIntegration.Models;

public class Actor
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? BirthDate { get; set; }
    public List<Show> Shows { get; set; }
}