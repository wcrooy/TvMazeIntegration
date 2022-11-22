namespace TvMazeIntegration.Clients.InternalModels;

public class CastEntry
{
    public Person Person { get; set; }

    public CastEntry(Person person)
    {
        Person = person;
    }
}