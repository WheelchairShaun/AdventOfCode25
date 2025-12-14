#:project Helpers
using Helpers;

var data = """
7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3
""";

string? puzzle = args.Length > 0 ? args[0] : "";
var lines = Input.ReadInputFromFile(data, puzzle);

List<(double X, double Y)> coordinates = lines.Select(line => 
											{
												var l = line.Split(',');
												double x = Double.Parse(l[0]);
												double y = Double.Parse(l[1]);

												return (x, y);
											})
											.ToList();
											
var result = FindLargestRectangle(coordinates);

System.Console.WriteLine(result.maxArea);

(double maxArea, ((double x, double y)? p1, (double x, double y)? p2) pair) FindLargestRectangle(List<(double x, double y)> coordinates)
{
	double maxArea = 0;
	((double X, double Y)? p1, (double X, double Y)? p2) bestPair = (null, null);

	// Brute force all pairs
	foreach (var pair in coordinates.SelectMany((p1, i) => coordinates.Skip(i + 1), (p1, p2) => (p1, p2)))
	{
		double width = Math.Abs(pair.p2.x - pair.p1.x) + 1;
		double height = Math.Abs(pair.p2.y - pair.p1.y) + 1;
		double area = width * height;

		if (area > maxArea)
		{
			maxArea = area;
			bestPair = (pair.p1, pair.p2);
		}
    }

	return (maxArea, bestPair);
}