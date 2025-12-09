#:project Helpers

using Helpers;

string data ="""
123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  
""";

string? puzzle = args.Length > 0 ? args[0] : "";

//  Split the input into a grid
var grid = Input.ReadInputFromFile(data, puzzle)
	.Select(line => line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
	.ToArray();
	
// Parse every column into an array of problems
int cols = grid.Max(l => l.Length);

var homework = Enumerable.Range(0, cols)
            .Select(c => string.Join(" ", grid.Select(l => l[c])))
            .ToArray();

long total = 0;

foreach(var problem in homework)
{
	var p = problem.Split(' ');

	long r = p[^1] switch
	{
		"*" => p.Take(p.Length - 1).Select(long.Parse).Aggregate(1L, (acc, n) => acc * n),
		_ 	=> p.Take(p.Length - 1).Select(long.Parse).Aggregate(0L, (acc, n) => acc + n)

	};

	total += r;
}

System.Console.WriteLine(total);