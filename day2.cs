#:project Helpers

using System.Data.Common;
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

long invalidsSum2 = invalids.Select(id => new
			{
				id.i,
				id.s
			})
			.Where(id => IsInvalidPart2(id.i, id.s))
			.Sum(x => x.i);

System.Console.WriteLine($"The sum of the Part 2 invalid IDs is {invalidsSum2}");

bool IsInvalidPart2(long id, string s)
{
    // Iterate through possible repeating pattern lengths
	// The upper bound will be half the string length
    for (int patternLength = 1; patternLength <= s.Length / 2; patternLength++)
    {
        // Extract the potential repeating pattern
        string pattern = s.Substring(0, patternLength);

        // Check if the entire ID string is formed by repeating this pattern at least twice
        if (s.Length % patternLength == 0) // Make sure the string is usually divided by the current length
        {
            int repetitions = s.Length / patternLength;
            if (repetitions >= 2 && Enumerable.Range(0, repetitions)
                                              .All(i => s.Substring(i * patternLength, patternLength) == pattern))
            {
				//System.Console.WriteLine(s);
                return true; // Found a repeating pattern
            }
        }
    }
    return false; // No repeating pattern found
}

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