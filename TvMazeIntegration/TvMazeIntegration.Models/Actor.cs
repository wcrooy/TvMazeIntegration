using System.ComponentModel.DataAnnotations;

namespace TvMazeIntegration.Models;

public class Actor
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? BirthDate { get; set; }
    public virtual List<Show> Shows { get; set; }
    
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}