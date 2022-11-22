using System.ComponentModel.DataAnnotations;

namespace TvMazeIntegration.Models;

public class Show
{
    private List<Actor>? _actors = new List<Actor>();
    
    [Required]
    public int Id { get; set; }
    public string? Name { get; set; }

    public List<Actor>? Cast
    {
        get => _actors.OrderByDescending(actor => actor.BirthDate).ToList();
        set => _actors = value;
    }
}