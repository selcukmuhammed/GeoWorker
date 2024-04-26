# GeoWorker

CoordsProcessor is a background service that processes geographic coordinate data. In the project, it retrieves the geographical coordinates stored in the database, retrieves the exact addresses using these coordinates, and finally saves the processed data in the database.

##Requirements

- .NET 7.0 SDK
- SQL Server
- OpenCage Geocoding API key
- EntityFrameworkCore
- Newtonsoft Json
