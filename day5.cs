#:project Helpers
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using Helpers;

string data = """
3-5
10-14
16-20
12-18

1
5
8
11
17
32
""";

string? puzzle = args.Length > 0 ? args[0] : @"";
var database = Input.ReadInputFromFile(data, puzzle);

int index = Array.IndexOf(database, "");

List<(long start, long end)> ranges = database.Take(index)
				.Select(r => 
				{
					var start = long.Parse(r.Split('-')[0]);
					var end = long.Parse(r.Split('-')[1]);
					return (start, end);
				})
				.OrderBy(r => r.start)
				.ThenBy(r => r.end)
				.ToList();

ranges = MergeRanges(ranges);

var stock = new Stack<long>(database.Skip(index + 1)
				.Select(s => long.Parse(s)));

int fresh = 0;

while(stock.Count > 0)
{
	long c = stock.Pop();
	foreach((long start, long end) in ranges)
	{
		if(c >= start && c <= end)
		{
			fresh++;
			break;
		}
	}
}

System.Console.WriteLine(fresh);

long total = 0;

foreach ((long s, long e) in ranges)
{
	total += (e + 1) - s;
}

System.Console.WriteLine(total);

List<(long start, long end)> MergeRanges(List<(long start, long end)> ranges)
{
	var merged = new List<(long start, long end)>();
	foreach (var range in ranges)
	{
		if (merged.Count == 0)
		{
			merged.Add(range);
		}
		else
		{
			var last = merged[^1]; // last element
			if (range.start <= last.end + 1) // overlap or adjacent
			{
				merged[^1] = (last.start, Math.Max(last.end, range.end));
			}
			else
			{
				merged.Add(range);
			}
		}
	}

	return merged;
}