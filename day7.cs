#:project Helpers

using System.Runtime.CompilerServices;

using Helpers;

var data = """
.......S.......
...............
.......^.......
...............
......^.^......
...............
.....^.^.^.....
...............
....^.^...^....
...............
...^.^...^.^...
...............
..^...^.....^..
...............
.^.^.^.^.^...^.
...............
""";

string? puzzle = args.Length > 0 ? args[0] : "";
var lines = Input.ReadInputFromFile(data, puzzle);

var manifold = Grid.CreateGrid(lines);
var quantum = Grid.CreateGrid(lines);
var cols = manifold.GetLength(0);
var rows = manifold.GetLength(1);

//Grid.Print(manifold);

var start = Enumerable.Range(0, cols)
				.SelectMany(row => Enumerable.Range(0, rows)
					.Select(col => (X: col, Y: row, Value: manifold[col, row])))
				.FirstOrDefault(c => c.Value == 'S');

// Create first beam
manifold[start.X, start.Y+1] = '|';

int splits = 0;

var cr = 1;

while(cr < rows)
{
	var beams = Enumerable.Range(0, cols)
					.Where(col => manifold[col, cr] == '|')
					.Select(col => (x: col, y: cr))
					.ToArray();

	splits += TraverseBeams(beams);
	cr++;
}


System.Console.WriteLine(splits);

Grid.Print(quantum);

int TraverseBeams((int x, int y)[] beams)
{
	int s = 0;

	foreach(var beam in beams)
	{
		if (beam.y + 1 < rows)
		{
			var n = manifold[beam.x, beam.y+1]; // Get the next space the beam goes to

			if (n.Equals('^'))
			{
				// [7, 2]
				var left = (x: beam.x-1, y: beam.y+1);
				var right = (x: beam.x+1, y: beam.y+1);

				manifold[left.x, left.y] = '|';
				manifold[right.x, right.y] = '|';

				s++;
			}
			else
			{
				manifold[beam.x, beam.y+1] = '|';
			}
		}
	}

	return s;
}