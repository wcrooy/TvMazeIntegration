using System.ComponentModel.DataAnnotations;

namespace TvMazeIntegration.Models;

public class ActorShow
{

    public int ActorId;
    [Required] public int ShowId;

    public Actor Actor { get; set; }
    public Show Show { get; set; }

}