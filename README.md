# Stockr - Live stocks
This is an example project, which shows live updates of simulated stock prices. It consists of:
- An simple Angular frontend with an overview of the stocks on the server, which is updated live through a SignalR hub.
- An ASP.Net Core web API, which exposes a few endpoints for retrieving the current list of stocks (and an unused endpoint for historical values, which was intended for graphing a stock's price over time).
- A Microsoft Orleans Silo, which is used as the primary way of processing and persisting data. For anyone unfamiliar, see: https://learn.microsoft.com/en-us/dotnet/orleans/
- A Stock Simulator, which is a .NET console application which hooks into and provides data for the Orleans Silo.

## Running locally
Prerequisites:
- docker compose

You can build, run and explore the project with the following command from the root of the project repository:
`docker compose up`

This should start up all 4 components, as well as Consul, which is the technology used for orchestrating the Orleans cluster.

The Angular frontend can then be visisted at http://localhost:4200

The generated data is sent from the console application in `/dotnet/Stockr.StockSimulator`. It can be controlled by modifying the following environment variables in the corresponding service in `/docker-compose.yaml`:
- `SIMULATOR__STOCKUPDATEINTERVAL: TimeSpan` - Controls how often the simulator emits a new StockUpdate, e.g.: '00:00:00.100' for 100ms between emits.
- `SIMULATOR__RANDOMIZERSEED: int` - The seed that the simulator uses to generate data. This allows repetition of exactly the same sequence of events between runs (not according for timestamps)
- `SIMULATOR__NUMBEROFSTOCKS: int` - How many unique stocks should be generated.
- `SIMULATOR__UPDATESTOCKSSEQUANTIALLY: bool` - Whether or not the simulator should emit for each stock in order (true) or pick a random one each time (false). 
- `SIMULATOR__MAXIMUMSTOCKCHANGEPERCENT: double` - How much the value of the stocks are able to change between updates, e.g 0.05 for a maximum of 5% change in either direction - Be careful not to set this too high, might overflow if you're (un)lucky.

## Limitations and corners cut
The primary focus has been on getting Orleans, SignalR and Angular to play nicely together.

There are no tests, authentication or authorization and everything very much assumes sunshine scenarios.

Most of the work has been in the backend, which shows in the frontend.

There are a few issues with the lifetime management between the SignalR Hubs and the Orleans Observers, which turned out to be a non-trivial issue to fix. Very much a fixable issue, but a time-consuming one to crack.

No pesistence is implemented.