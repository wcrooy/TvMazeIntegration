# TvMazeIntegration
This service offers the scraped shows from: https://www.tvmaze.com/

See for Api details:
https://localhost:7055/swagger/index.html

Additionaly there's a background service which does the actual scraping. 
See: 

## Environment
* .NET 6
* EF Core for ORM
* SQLite as storage backend. (This is included within the source)

## Usefull links:
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
https://www.tvmaze.com/api
