#:project Helpers

using Helpers;

string data = """
aaa: you hhh
you: bbb ccc
bbb: ddd eee
ccc: ddd eee fff
ddd: ggg
eee: out
fff: out
ggg: out
hhh: ccc fff iii
iii: out
""";

var puzzle = args.Length > 0 ? args[0] : "";
var lines = Input.ReadInputFromFile(data, puzzle);

Dictionary<string, List<string>> map = lines.Select(line =>
{
	var l = line.Split(':');
	string device = l[0];
	var paths = l[1].Trim().Split(' ').ToList();
	return (device, paths);
}).ToDictionary();

long paths = CountPaths(map, "you", "out");

System.Console.WriteLine(paths);

data = """
svr: aaa bbb
aaa: fft
fft: ccc
bbb: tty
tty: ccc
ccc: ddd eee
ddd: hub
hub: fff
eee: dac
dac: fff
fff: ggg hhh
ggg: out
hhh: out
""";

lines = Input.ReadInputFromFile(data, puzzle);
map = lines.Select(line =>
{
	var l = line.Split(':');
	string device = l[0];
	var paths = l[1].Trim().Split(' ').ToList();
	return (device, paths);
}).ToDictionary();

long total2 = CountPathsWithRequirements(map, "svr", "out");

System.Console.WriteLine(total2);

long CountPaths(Dictionary<string, List<string>> graph, string start, string destination)
{
	var memory = new Dictionary<string, long>();
	var visiting = new HashSet<string>();	

	return Dfs(graph, memory, visiting, start, destination);
}

long Dfs(Dictionary<string, List<string>> graph, Dictionary<string, long> memory, HashSet<string> visiting, 
			string node, string destination)
{
	// If we've reached the destination, that's one complete path
	if (node == destination)
		return 1;

	// If we already computed this node, reuse it
	if (memory.TryGetValue(node, out var cached))
		return cached;

	// Cycle protection: if node is in current recursion stack, ignore this path
	if (!visiting.Add(node))
		return 0;

	long total = 0;

	if (graph.TryGetValue(node, out var neighbors))
	{
		foreach (var next in neighbors)
		{
			total += Dfs(graph, memory, visiting, next, destination);
		}
	}

	visiting.Remove(node);

	memory[node] = total;
	return total;
}

long CountPathsWithRequirements(Dictionary<string, List<string>> graph, string start, string destination)
{
    var memo = new Dictionary<(string node, bool dac, bool fft), long>();
    var visiting = new HashSet<string>();

    long Dfs(string node, bool visitedDac, bool visitedFft)
    {
        // If we reached out, check if both required nodes were visited
        if (node == destination)
            return (visitedDac && visitedFft) ? 1 : 0;

        var key = (node, visitedDac, visitedFft);
        if (memo.TryGetValue(key, out var cached))
            return cached;

        // Cycle protection
        if (!visiting.Add(node))
            return 0;

        long total = 0;

        if (graph.TryGetValue(node, out var neighbors))
        {
            foreach (var next in neighbors)
            {
                bool nextDac = visitedDac || next == "dac";
                bool nextFft = visitedFft || next == "fft";

                total += Dfs(next, nextDac, nextFft);
            }
        }

        visiting.Remove(node);
        memo[key] = total;
        return total;
    }

    return Dfs(start, start == "dac", start == "fft");
}

