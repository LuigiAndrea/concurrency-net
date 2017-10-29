Concurrency (Examples in ASP.NET Core MVC)
===

List of recipes on concurrency and asyncronous programming in c#. 

- Task Completion Source
- Task FromResult
- Simple retry mechanism simulated with Task.Delay and CancellationToken
- CreateLinkedTokenSource
- ThrowIfCancellationRequested
- Aggregate Exceptions

### Prerequisitest

You need to have installed .NET Core on your machine.
Verify your version running dotnet --version in a terminal/console window.

### Build and Run

To __build and run__ the samples, navigate to src/concurrency and run the following commands:
```
dotnet restore

dotnet run

The app is listening on http://localhost:5000
```
