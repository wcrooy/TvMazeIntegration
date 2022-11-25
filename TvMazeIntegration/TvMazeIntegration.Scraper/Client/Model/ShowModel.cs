
using System.ComponentModel.DataAnnotations;

namespace TvMazeIntegration.Scraper.Client.Model;

public class ShowModel
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public List<ActorModel>? Cast
    {
        get;
        set;
    }
}