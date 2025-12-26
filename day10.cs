#:project Helpers
#:package Google.OrTools@9.14.6206

using Helpers;
using System.Text;
using System.Text.RegularExpressions;
using Google.OrTools.LinearSolver;


string data ="""
[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
""";

string? puzzle = args.Length > 0 ? args[0] : "";
var lines = Input.ReadInputFromFile(data, puzzle);

string pattern = @"(?<lights>\[[.#]+\])\s*(?<buttons>(?:\(\d+(?:,\d+)*\)\s*)+)(?<voltage>\{(?:\d+|,)+\})";

List<Machine> machines = new();

foreach(string line in lines)
{
	var match = Regex.Match(line, pattern);
	
	if (match.Success)
	{
		string l = match.Groups["lights"].Value.Trim();
		string b = match.Groups["buttons"].Value.Trim();
		string v = match.Groups["voltage"].Value.Trim();

		machines.Add(new Machine(l, b, v));
	}
}

int total = 0;
int vlt = 0;

foreach(Machine m in machines)
{
	total += m.GetMinimumPresses();
	vlt += m.GetMinimumJoltagePresses();
}

Console.WriteLine(total);
Console.WriteLine(vlt);

enum Light
{
	Off,
	On
}

class Machine
{
	readonly string? _target;
	readonly List<int[]>? _buttons;
	readonly int[]? _voltage;

	public Machine(string lights, string buttons, string voltage)
	{
		// Initialize target
		_target = lights[1..^1];

		// Initialize buttons
		_buttons = buttons.Split(' ')
						.Select(b => b[1..^1].Split(',')
							.Select(x => int.Parse(x))
							.ToArray()
						).ToList();

		// Initialize voltage
		_voltage = voltage[1..^1].Split(',')
							.Select(v => int.Parse(v))
							.ToArray();
	}

	public static char FlipLight(char light) =>
		light.ToLight() == Light.On ? '.' : '#';

	public static string PressButton(string state, int[] button)
	{
		StringBuilder result = new StringBuilder(state);

		foreach (var index in button)
		{
			result[index] = FlipLight(result[index]);
		}

		return result.ToString();
	}

	public int GetMinimumPresses()
	{
		// All lights start OFF
		string startState = new string('.', _target!.Length);

		// BFS queue: (state, presses)
		var queue = new Queue<(string state, int presses)>();
		queue.Enqueue((startState, 0));

		// Track visited states
		var visited = new HashSet<string>();
		visited.Add(startState);

		while (queue.Count > 0)
		{
			var (state, presses) = queue.Dequeue();

			foreach (var btn in _buttons!)
			{
				string nextState = PressButton(state, btn);

				if (nextState == _target)
					return presses + 1;

				if (!visited.Contains(nextState))
				{
					visited.Add(nextState);
					queue.Enqueue((nextState, presses + 1));
				}
			}
		}

		return -1; // No solution found
	}

	public int GetMinimumJoltagePresses()
	{
		int n = _voltage!.Length;     // number of counters
		int m = _buttons!.Count;      // number of buttons

		// Create solver (CBC is the integer solver)
		Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
		if (solver == null)
			throw new Exception("Could not create solver.");

		// Variables: x_j >= 0 integer
		Variable[] x = new Variable[m];
		for (int j = 0; j < m; j++)
		{
			x[j] = solver.MakeIntVar(0.0, double.PositiveInfinity, $"x_{j}");
		}

		// Constraints: sum_j A[i,j] * x_j == voltage[i]
		for (int i = 0; i < n; i++)
		{
			Constraint ct = solver.MakeConstraint(_voltage[i], _voltage[i], $"counter_{i}");

			for (int j = 0; j < m; j++)
			{
				if (_buttons[j].Contains(i))
					ct.SetCoefficient(x[j], 1);
			}
		}

		// Objective: minimize total presses
		Objective objective = solver.Objective();
		for (int j = 0; j < m; j++)
			objective.SetCoefficient(x[j], 1);

		objective.SetMinimization();

		// Solve
		Solver.ResultStatus resultStatus = solver.Solve();

		if (resultStatus != Solver.ResultStatus.OPTIMAL)
			throw new Exception("No optimal solution found.");

		// Sum presses
		int totalPresses = 0;
		for (int j = 0; j < m; j++)
			totalPresses += (int)x[j].SolutionValue();

		return totalPresses;
	}
}

static class LightExtensions
{
	public static Light ToLight(this char c) =>
		c switch
		{
			'#' => Light.On,
			'.' => Light.Off,
			_   => throw new ArgumentException($"Invalid light character: {c}")
		};
}