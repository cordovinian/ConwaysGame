# Conways Game

## Getting started

1. Install dotnet core
1. Run `dotnet build` in solution workspace to get all packages
1. Run `dotnet test` in solution workspace to run all tests
1. Run `dotnet run --project ConwaysGame` in solution workspace to build and run program
1. Run `dotnet run --project ConwaysGame -- -h` to see argument usage and help text

## Rules

The next state of each space on the grid is determined by the current state of it's surrounding grid.

- Any live cell at time T with < 2 live neighbors dies (by underpopulation)
- Any live cell at time T  with exactly 2 or 3 live neighbors survives
- Any live cell at time T with > 3 live neighbors dies (by overpopulation)
- Any dead cell with exactly 3 live neighbors becomes alive (by reproduction)

## Interface

Implement the following interface:

```
# Transition exactly one timestep
# Grid is a 2D array
def trasition(old_state: Grid) -> Grid
```

## Example

Note: Known by the common oscillator pattern name "Blinker" this pattern will remain stable until acted upon by external influence.
See [Conway Life Wiki: Common Objects](https://conwaylife.com/wiki/Common)

Starting Grid

```
0, 0, 0
1, 1, 1
0, 0, 0
```

Post Transition Grid

```
0, 1, 0
0, 1, 0
0, 1, 0
```

## Notes

- We can assume the shape of the grid will remain retangular. Width of each row will be the same.
