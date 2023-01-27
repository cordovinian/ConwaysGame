# ConwaysGame

## What is this template?

Contains:

- Main project
- Test Project with FluentAssertions

Steps:

```
dotnet new sln
dotnet new console -n ConwaysGame
dotnet new xunit -n ConwaysGameTests
dotnet add ConwaysGameTests package FluentAssertions
dotnet add ConwaysGameTests reference ConwaysGame
dotnet sln add ConwaysGame
dotnet sln add ConwaysGameTests
dotnet build
```

## Conways

Any live cell at time T with < 2 live neighbors dies (by underpopulation)
Any live cell at time T  with exactly 2 or 3 live neighbors survives
Any live cell at time T with > 3 live neighbors dies (by overpopulation)
Any dead cell with exactly 3 live neighbors becomes alive (by reproduction)


```
0, 0, 0
1, 1, 1
0, 0, 0
```

Transition

```
0, 1, 0 (done)
0, 1, 0
0, 1, 0
```
