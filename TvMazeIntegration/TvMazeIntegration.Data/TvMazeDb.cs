using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TvMazeIntegration.Models;

namespace TvMazeIntegration.Data;

public class TvMazeDb:DbContext
{
    private readonly ILogger<TvMazeDb> _log;

    public TvMazeDb()
    {
        
    }
    public TvMazeDb(DbContextOptions options, ILogger<TvMazeDb> log) : base(options)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
          
    }
    public DbSet<Show> Shows { get; set; }
    public DbSet<Actor> Actors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Show>()
            .HasMany(x => x.Cast)
            .WithMany(x => x.Shows)
            .UsingEntity<ActorShow>(
                x => x.HasOne(x => x.Actor).WithMany().HasForeignKey(x => x.ActorId),
                x => x.HasOne(x => x.Show).WithMany().HasForeignKey(x => x.ShowId)
            );
    }



    public virtual async Task<List<Show>> PutShows(List<Show> showsToAddOrUpdate)
    {
        foreach (var show in showsToAddOrUpdate)
        {
            await UpdateShow(show);
        }

        return showsToAddOrUpdate;
    }

    private async Task UpdateShow(Show show)
    {
        var showFromDb = await Shows.Include(show1 => show1.Cast).SingleOrDefaultAsync(show1 => show1.Id == show.Id);
        if (showFromDb != null && !string.IsNullOrWhiteSpace(showFromDb.Name))
        {
            var actors = await GetFromDbOrAddActorToDb(show);
            show.Cast = actors;
            if (show.Name != showFromDb.Name)
            {
                showFromDb.Name = show.Name;
            }
            Shows.Update(showFromDb);
        }
        else
        {
            var actors = await GetFromDbOrAddActorToDb(show);
            show.Cast = actors;
            await Shows.AddAsync(show);
        }
        
        
        await SaveChangesAsync();
    }

    private async Task<List<Actor>> GetFromDbOrAddActorToDb(Show show)
    {
        List<Actor> actors = new List<Actor>();
        if (show.Cast != null)
        {
            foreach (var actor in show.Cast)
            {
                var actorFromDb = await Actors.Include(actor1 => actor1.Shows)
                    .SingleOrDefaultAsync(x => x.Id == actor.Id);
                if (actorFromDb != null)
                { //Update
                    actorFromDb.Name = actor.Name;
                    actorFromDb.BirthDate = actor.BirthDate;
                    actors.Add(actorFromDb);
                }
                else
                {
                    await Actors.AddAsync(actor);
                    actors.Add(actor);
                }
            }
        }

        return actors;
    }

    public virtual async Task<(int MaxPages, List<Show> Shows)> GetShowsPaginated(int currentPage, int maxItemsPerPage)
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

        var shows=  await Shows
            .AsNoTracking()
            .Include(show => show.Cast)
            .OrderBy(show => show.Id).Skip(skipNumber).Take(maxItemsPerPage).ToListAsync();

        return (maxPages, shows); //One could make a separate DTO for this. 
    }

    /// <summary>
    /// Status check which checks if DB is available.
    /// </summary>
    /// <returns><see cref="bool"/>True if it can retrieve the first actor in the table</returns>
    public async Task<bool> CheckDatabaseStatus()
    {
        var result = await Actors.FirstOrDefaultAsync();
        return result is { Id: > 0 };
    }
}