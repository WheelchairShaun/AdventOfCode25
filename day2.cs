#:project Helpers

using Helpers;

string? puzzle = args.Length > 0 ? args[0] : "";

var data = """
11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
""";

string products = Input.GetInputFromFileAsString(data, puzzle);
string[] ranges = products.Split(',');

var invalids = FindProductIds(ranges).ToList();
			
long invalidsSum = invalids.Select(id => new
			{
				id.i,
				left = id.s[0..(id.s.Length / 2)],
				right = id.s[(id.s.Length /2)..]
			})
			.Where(id => id.left.Equals(id.right))
			.Sum(x => x.i);

System.Console.WriteLine($"The sum of the invalid IDs is {invalidsSum}");





IEnumerable<(long i, string s)> FindProductIds(string[] ranges)
{
	foreach (string range in ranges)
	{
		
		(long lower, long upper) = range.Split('-', 2) switch {var x => (long.Parse(x[0]), long.Parse(x[1]))};

		for (long i = lower; i <= upper; i++)
		{
			yield return (i, i.ToString());
		}
	}
}