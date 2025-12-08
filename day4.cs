#:project Helpers

string data = """
..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.
""";

string? puzzle = args.Length > 0 ? args[0] : "";

string[] rolls = Helpers.Input.ReadInputFromFile(data, puzzle);
var grid = Helpers.Grid.CreateGrid(rolls);

Helpers.Grid.Print(grid);
System.Console.WriteLine();

(int maxX, int maxY) coords = (rolls[0].Length, rolls.Length);

(char[,] part1, int total) = RemoveRolls(grid);


Helpers.Grid.Print(part1);
System.Console.WriteLine();

System.Console.WriteLine(total);
System.Console.WriteLine();

// Part 2
int removed = 0;
char[,] g = part1;

do
{
	(g, removed) = RemoveRolls(g);
	total+=removed;
	System.Console.WriteLine(total);
}
while(removed > 0);

(char[,], int) RemoveRolls(char[,] grid)
{
	var result = new char[coords.maxX, coords.maxY];
	int count = 0;

	for (int x = 0; x < coords.maxX; x++)
	{
		for (int y = 0; y < coords.maxY; y++)
		{
			char c = grid[x,y];
			if (c == '@')
			{
				// Lookup all 8 spaces around current roll
				// Add up adjacent rolls
				int a =
					Lookup(x-1, y-1, grid) +
					Lookup(x-1, y, grid) +
					Lookup(x-1, y+1, grid) +
					Lookup(x, y+1, grid) +
					Lookup(x+1, y+1, grid) +
					Lookup(x+1, y, grid) +
					Lookup(x+1, y-1, grid) +
					Lookup(x, y-1, grid);

				// Less than 4
				if (a < 4)
				{
					count++;
					result[x,y] = 'x';
				}
				else
				{
					result[x,y] = c;
				}
			}
			else
			{
				result[x,y] = c;
			}
		}
	}

	return (result, count);
}

int Lookup(int x, int y, char[,] grid)
{
	if (x >= 0 && y >= 0 && 
		x <= coords.maxX - 1 && y <= coords.maxY - 1)
	{
		return grid[x,y] == '@' ? 1 : 0;
	}

	return 0;
}
