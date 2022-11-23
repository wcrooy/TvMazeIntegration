
namespace TvMazeIntegration.Models.Models;

public class ShowModel
{
    private List<ActorModel>? _cast;
    
    public int Id { get; set; }
    public string? Name { get; set; }

    public virtual List<ActorModel>? Cast
    {
        get => (_cast ?? new List<ActorModel>()).OrderByDescending(model => model.BirthDate).ToList();
        set => _cast = value;
    }
}