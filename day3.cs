#:project Helpers

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


long FindLargestVoltage(ReadOnlySpan<char> bank)
{
	char v1 = '0', v2 = '0';
	int pos1 = 0;

	// Make 1 pass to find the largest voltage
	// except the final position
	// then capture the number
	for (int i = 0; i < bank.Length - 1; i++)
	{
		char current = bank[i];
		if (current > v1)
		{
			v1 = current;
			pos1 = i;
		}
	}

	// Search the rest of the array from pos1 + 1
	for (int i = pos1 + 1; i < bank.Length; i++)
	{
		char current = bank[i];
		if (current > v2)
		{
			v2 = current;
		}
	}

	// Combine those characters then return a long
	return long.Parse(string.Concat(v1, v2));
}
