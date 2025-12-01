#:project Helpers

string? puzzle = args.Length > 0 ? args[0] : null;

var data = """
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
""";

int password = 0;

LinkedList<int> dial = new();

// Populate dial
for (int i = 0; i < 100; i++)
{
	dial.AddLast(i);
}

// Start at 50
var current = dial.Find(50);
System.Console.WriteLine($"The dial starts by pointing at {current?.Value}.");

if (current == null)
	throw new InvalidOperationException("Starting position 50 not found in dial");

// Step through input
var lines = Helpers.Input.ReadInputFromFile(data, puzzle);

foreach (var line in lines)
{
	if (line[0] == 'L')
		current = TurnLeft(current, int.Parse(line[1..]));
	else
		current = TurnRight(current, int.Parse(line[1..]));
	
	System.Console.WriteLine($"The dial is rotated {line} to point at {current.Value}.");

	if (current.Value == 0)
		password++;
}

System.Console.WriteLine($"\nThe password is {password}");

LinkedListNode<int> TurnLeft(LinkedListNode<int> c, int steps)
{
	for (int i = steps; i > 0; i--)
	{
		if (c.Value == 0)
		{
			if (dial.Last == null)
				throw new InvalidOperationException("dial.Last is null");
			c = dial.Last;
			continue;
		}

		if (c.Previous == null)
				throw new InvalidOperationException("c.Previous is null");
		c = c.Previous;
	}

	return c;
}

LinkedListNode<int> TurnRight(LinkedListNode<int> c, int steps)
{
	for (int i = steps; i > 0; i--)
	{
		if (c.Value == 99)
		{
			if (dial.First == null)
				throw new InvalidOperationException("dial.First is null");
			c = dial.First;
			continue;
		}

		if (c.Next == null)
				throw new InvalidOperationException("c.Next is null");
		c = c.Next;
	}

	return c;
}