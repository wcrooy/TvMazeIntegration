using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TvMazeIntegration.Models;

namespace TvMazeIntegration.Data;

public class TvMazeDb:DbContext
{
    private readonly ILogger<TvMazeDb> _log;

    public TvMazeDb(DbContextOptions options, ILogger<TvMazeDb> log) : base(options)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }
    public DbSet<Show> Shows { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { }

    public DbSet<Actor> Actors { get; set; } = null!;

    public async Task PutShow(Show show)
    {
        if (show.Cast.Any())
        {
            foreach (var actor in show.Cast)
            {
                if (await Actors.AnyAsync(actor1 => actor1.Id == actor.Id))
                {
                    Actors.Update(actor);
                }
                else
                {
                    await Actors.AddAsync(actor);
                }
            }
        }
        
        if (await Shows.AnyAsync(show1 => show1.Id == show.Id))
        {
            Shows.Update(show);
        }
        else
        {
            await Shows.AddAsync(show);
        }
    
        await SaveChangesAsync();
    }

    public async Task<(int MaxPages, List<Show> Shows)> GetShowsPaginated(int currentPage, int maxItemsPerPage)
    {
        var totalShowsInDatabase = await Shows.CountAsync();
        int maxPages = (int)Math.Floor(totalShowsInDatabase / (decimal)maxItemsPerPage); //We start at page 0

        if (currentPage> maxPages)
        {
            _log.LogDebug($"Totalshows in DB: {totalShowsInDatabase}. MaxPages available: {maxPages} with maxItemsPerPage: {maxItemsPerPage}");
            //Just return nothing
            return (maxPages, new List<Show>());
        }

        var skipNumber = currentPage * maxItemsPerPage;

        var shows=  await Shows.OrderBy(show => show.Id).Skip(skipNumber).Take(maxItemsPerPage).ToListAsync();

        return (maxPages, shows); //One could make a separate DTO for this. 
    }

    public async Task<bool> CheckDatabaseStatus()
    {
        var result = await Actors.FirstOrDefaultAsync();
        return result is { Id: > 0 };
    }
}