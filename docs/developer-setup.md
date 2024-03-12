# Required Tooling

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download)
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
  - Needed for Azure SignalR Emulator
- [Powershell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-macos?view=powershell-7.4)
  - Needed for Playwright Tests
- [Docker](https://www.docker.com/products/docker-desktop/)

# Using Cake 

```bash
# Ensure you are at the root of the solution

dotnet cake --target "StartApp" # Builds & Starts the application using docker compose

dotnet cake # Runs default cake target
```