# Stockr - Live stocks
This is an example project, which shows live updates of simulated stock prices. It consists consiting of:
- An simple Angular frontend with a simple overview of the stocks, which is updated live through a SignalR hub.
- An ASP.Net Core web API, which exposes a few endpoints for retrieving the current list of stocks (and an unused endpoint for historical values, which was intended for graphing a stock's price over time).
- A Microsoft Orleans Silo, which is used as the primary way of processing and persisting data. For anyone unfamiliar, see: https://learn.microsoft.com/en-us/dotnet/orleans/
- A Stock Simulator, which is a .NET console application which hooks into and provides data for the Orleans Silo.

## Running locally
Prerequisites:
- docker compose

You can build, run and explore the project with the following command from the root of the project repository:
`docker compose up`

This should start up all 4 components, as well as Consul, which is the technology used for the Orleans cluster.

The Angular frontend can then be visisted at http://localhost:4200