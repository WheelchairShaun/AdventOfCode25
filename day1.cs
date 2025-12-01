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
int password2 = 0;

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
	bool passedZero = false;

	if (line[0] == 'L')
		(current, passedZero) = TurnLeft(current, int.Parse(line[1..]));
	else
		(current, passedZero) = TurnRight(current, int.Parse(line[1..]));
	
	System.Console.Write($"The dial is rotated {line} to point at {current.Value}");

	if (passedZero)
	{
		System.Console.Write("; during this rotation, it points at 0 once");
	}

	System.Console.WriteLine(".");

	if (current.Value == 0)
	{
		password++;
	}
		
}

System.Console.WriteLine($"\nThe password is {password}");
System.Console.WriteLine($"\nThe password using method 0x434C49434B is {password2}");

(LinkedListNode<int>, bool) TurnLeft(LinkedListNode<int> c, int steps)
{
	bool passed = false;
	bool first = true;

	for (int i = steps; i > 0; i--)
	{
		if (c.Value == 0)
		{
			password2++;

			if (dial.Last == null)
				throw new InvalidOperationException("dial.Last is null");
			c = dial.Last;
			if (first == false)
				passed = true;

			continue;
		}

		if (c.Previous == null)
				throw new InvalidOperationException("c.Previous is null");
		c = c.Previous;
		first = false;
	}

	return (c, passed);
}

(LinkedListNode<int>, bool) TurnRight(LinkedListNode<int> c, int steps)
{
	bool passed = false;
	bool first = true;

	for (int i = steps; i > 0; i--)
	{
		if (c.Value == 99)
		{
			if (dial.First == null)
				throw new InvalidOperationException("dial.First is null");
			c = dial.First;
			continue;
		}

		if (c.Value == 0)
		{
			password2++;
			if (first == false)
				passed = true;
		}

		if (c.Next == null)
				throw new InvalidOperationException("c.Next is null");
		c = c.Next;
		first = false;
	}

	return (c, passed);
}