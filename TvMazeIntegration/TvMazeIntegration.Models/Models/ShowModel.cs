
using System.ComponentModel.DataAnnotations;

namespace TvMazeIntegration.Models.Models;

public class ShowModel
{
    private List<ActorModel>? _cast;
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }

    [Required]
    public virtual List<ActorModel>? Cast
    {
        get => (_cast ?? new List<ActorModel>()).OrderByDescending(model => model.BirthDate).ToList();
        set => _cast = value;
    }
}