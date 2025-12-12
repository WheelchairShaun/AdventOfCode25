#:project Helpers

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

var cols = manifold.GetLength(0);
var rows = manifold.GetLength(1);

//Grid.Print(manifold);

var start = Enumerable.Range(0, cols)
				.SelectMany(row => Enumerable.Range(0, rows)
					.Select(col => (X: col, Y: row, Value: manifold[col, row])))
				.FirstOrDefault(c => c.Value == 'S');

// Dictionary to track how many timelines reach each cell
var paths = new Dictionary<(int x, int y), long>();

// Initialize the first beam
paths[(start.X, start.Y+1)] = 1L;
manifold[start.X, start.Y+1] = '|';

int splits = 0;
var cr = 1;

while (cr < rows)
{
    var beams = Enumerable.Range(0, cols)
        .Where(col => manifold[col, cr] == '|')
        .Select(col => (x: col, y: cr))
        .ToArray();

    splits += TraverseBeams(beams, paths);
    cr++;
}

Console.WriteLine($"Splits: {splits}");
Console.WriteLine($"Timelines: {CountTimelines(paths)}");


// --- Methods ---

int TraverseBeams((int x, int y)[] beams, Dictionary<(int x, int y), long> paths)
{
    int s = 0;

    foreach (var beam in beams)
    {
        if (beam.y + 1 < rows)
        {
            var n = manifold[beam.x, beam.y+1];
            long countHere = paths[(beam.x, beam.y)];

            if (n.Equals('^'))
            {
                var left = (x: beam.x-1, y: beam.y+1);
                var right = (x: beam.x+1, y: beam.y+1);

                manifold[left.x, left.y] = '|';
                manifold[right.x, right.y] = '|';

                paths[left] = paths.GetValueOrDefault(left) + countHere;
                paths[right] = paths.GetValueOrDefault(right) + countHere;

                s++;
            }
            else
            {
                var down = (x: beam.x, y: beam.y+1);
                manifold[down.x, down.y] = '|';

                paths[down] = paths.GetValueOrDefault(down) + countHere;
            }
        }
    }

    return s;
}

long CountTimelines(Dictionary<(int x, int y), long> paths)
{
    int maxY = paths.Keys.Max(p => p.y);
    return paths.Where(kv => kv.Key.y == maxY).Sum(kv => kv.Value);
}