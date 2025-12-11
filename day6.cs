#:project Helpers

using System.Text;
using Helpers;

string data ="""
123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  
""";

string? puzzle = args.Length > 0 ? args[0] : "";

var lines = Input.ReadInputFromFile(data, puzzle);

//  Split the input into a grid
var grid = lines
			.Select(line => line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
			.ToArray();
	
// Parse every column into an array of problems
int cols = grid.Max(l => l.Length);
int rows = grid.Length - 1;

var homework = Enumerable.Range(0, cols)
            .Select(c => string.Join(" ", grid.Select(l => l[c])))
            .ToArray();

long total = 0;

foreach(var problem in homework)
{
	total += Calculate(problem);
}

System.Console.WriteLine(total);

long total2 = 0;

List<string> homework2 = new();

// Get columns and rows
cols = lines.Max(line => line.Length);
rows = lines.Length - 1;

StringBuilder sb = new();
var last = cols - 1;

// Parse input right to left and top to bottom for each column
for (int col = cols - 1; col > -1; col--)
{
	// Read current column of characters
	var c = Enumerable.Range(0, rows)
				.Select(r => lines[r][col])
				.ToArray();

	if (c.All(s => char.IsWhiteSpace(s)) || col == 0)
	{
		// Grab final column
		if (col == 0)
		{
			sb.Append(new string(c).Trim());
			sb.Append(' ');
		}

		// Grab the operator
		var op = lines[rows].Substring(col, last - col).Trim();
		sb.Append(op);

		// Add to List
		homework2.Add(sb.ToString().Trim());
		sb.Clear();
		last = col;
	}

	sb.Append(new string(c).Trim());
	sb.Append(' ');
}

foreach(var problem in homework2)
{
	total2 += Calculate(problem);
}

System.Console.WriteLine(total2);

long Calculate(string problem)
{
	var p = problem.Split(' ');

	long r = p[^1] switch
	{
		"*" => p.Take(p.Length - 1).Select(long.Parse).Aggregate(1L, (acc, n) => acc * n),
		_ 	=> p.Take(p.Length - 1).Select(long.Parse).Aggregate(0L, (acc, n) => acc + n)

	};

	return r;
}