#:project Helpers

using System.Text;
using Helpers;

var data = """
987654321111111
811111111111119
234234234234278
818181911112111
""";

string? puzzle = args.Length > 0 ? args[0] : "";

var banks = Input.ReadInputFromFile(data, puzzle);

var voltage = banks.Select(b => new
{
	v = FindLargestVoltage(b.AsSpan())
}).Sum(b => b.v);

System.Console.WriteLine(voltage);

voltage = banks.Select(b => new
{
	v = FindLargestVoltage(b.AsSpan(), 12)
}).Sum(b => b.v);

System.Console.WriteLine(voltage);

long FindLargestVoltage(ReadOnlySpan<char> bank, int keep = 2)
{
	Stack<char> digits = new();
	int drop = bank.Length - keep;

	foreach (char d in bank)
	{
		// While I still have drops
		// and added the first digit to the stack
		// peek at the last digit and see if it's less than the current digit
		while (drop > 0 && digits.Count > 0 && digits.Peek() < d)
		{
			// remove the last digit
			digits.Pop();
			drop--;
		}
		digits.Push(d);
	}

	// If we still have drops left, remove from the end
	while (drop > 0)
	{
		digits.Pop();
		drop--;
	}

	// Build the result
	var result = new char[digits.Count];
	for (int i = digits.Count - 1; i >= 0; i--)
	{
		result[i] = digits.Pop();
	}

	return long.Parse(new string(result));
}
