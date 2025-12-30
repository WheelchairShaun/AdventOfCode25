#:project Helpers

using Helpers;

var data = """
162,817,812
57,618,57
906,360,560
592,479,940
352,342,300
466,668,158
542,29,236
431,825,988
739,650,466
52,470,668
216,146,977
819,987,18
117,168,530
805,96,715
346,949,466
970,615,88
941,993,340
862,61,35
984,92,344
425,690,689
""";

string? puzzle = args.Length > 0 ? args[0] : "";
var lines = Input.ReadInputFromFile(data, puzzle);

var junctions = lines
    .Select((p, i) =>
    {
        var a = p.Split(',');
        return new Junction(i, (double.Parse(a[0]), double.Parse(a[1]), double.Parse(a[2])));
    })
    .ToList();

// Compute distances and sort
var edges = new List<Edge>();
for (int i = 0; i < junctions.Count; i++)
{
    for (int j = i + 1; j < junctions.Count; j++)
    {
        var dist = Geometry.Distance3D(junctions[i].p, junctions[j].p);
        edges.Add(new Edge(junctions[i], junctions[j], dist));
    }
}

edges.Sort((a, b) => a.distance.CompareTo(b.distance));

var uf = new UnionFind(junctions.Count);
int processed = 0;

foreach (var edge in edges)
{
    // Always process the edge
    uf.Union(edge.a.id, edge.b.id);

    processed++;
    if (processed == 1000)
        break;
}

var circuits = junctions
    .Select(j => uf.ComponentSize(j.id)) 
    .Distinct()
    .OrderByDescending(size => size)
    .ToList();

int result = circuits.Take(3).Aggregate(1, (acc, val) => acc * val);
Console.WriteLine(result);

int lastA = -1, lastB = -1;

foreach (var edge in edges)
{
    if (uf.Union2(edge.a.id, edge.b.id))
    {
        lastA = edge.a.id;
        lastB = edge.b.id;

        if (uf.AllConnected())
            break;
    }
}

var result2 = (long)junctions[lastA].p.x * (long)junctions[lastB].p.x;
Console.WriteLine(result2);


record Junction(int id, (double x, double y, double z) p);

record Edge(Junction a, Junction b, double distance);

class UnionFind
{
    private readonly int[] parent;
    private readonly int[] size;

    public int Components { get; private set; }

    public UnionFind(int n)
    {
        parent = Enumerable.Range(0, n).ToArray();
        size = Enumerable.Repeat(1, n).ToArray();
        Components = n;
    }

    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]); // path compression
        return parent[x];
    }

    public bool Union(int a, int b)
    {
        int rootA = Find(a);
        int rootB = Find(b);
        if (rootA == rootB) return false;

        if (size[rootA] < size[rootB])
            (rootA, rootB) = (rootB, rootA);

        parent[rootB] = rootA;
        size[rootA] += size[rootB];
        return true;
    }

    public bool Union2(int a, int b)
    {
        int rootA = Find(a);
        int rootB = Find(b);
        if (rootA == rootB) return false;

        if (size[rootA] < size[rootB])
            (rootA, rootB) = (rootB, rootA);

        parent[rootB] = rootA;
        size[rootA] += size[rootB];

        Components--;   // NEW: one fewer component after a successful merge
        return true;
    }


    public int ComponentSize(int x) => size[Find(x)];
    public bool AllConnected() => Components == 1;
}
