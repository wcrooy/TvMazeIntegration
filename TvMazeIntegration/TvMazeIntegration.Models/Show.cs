using System.ComponentModel.DataAnnotations;

namespace TvMazeIntegration.Models;

public class Show
{
    [Required]
    public int Id { get; set; }
    public string? Name { get; set; }

    public virtual List<Actor>? Cast
    {
        get;
        set;
    }
}