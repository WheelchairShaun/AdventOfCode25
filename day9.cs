#:project Helpers
#:package NetTopologySuite@2.6.0

using Helpers;

// Set up NetTopology
NetTopologySuite.NtsGeometryServices.Instance = new NetTopologySuite.NtsGeometryServices(
	NetTopologySuite.Geometries.Implementation.CoordinateArraySequenceFactory.Instance,
	new NetTopologySuite.Geometries.PrecisionModel(1000d),
	4326,
	NetTopologySuite.Geometries.GeometryOverlay.NG,
	new NetTopologySuite.Geometries.CoordinateEqualityComparer()
);

// Geometry Factory
var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

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

var poly = BuildPolygon(coordinates);
var rg = FindLargestRedGreenRectangle(coordinates, poly);

System.Console.WriteLine(rg.maxArea);




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

NetTopologySuite.Geometries.Polygon BuildPolygon(List<(double x, double y)> redTiles)
{
	var pc = new NetTopologySuite.Geometries.Coordinate[redTiles.Count + 1];

	for (int i = 0; i < pc.Length - 1; i++)
	{
		pc[i] = new NetTopologySuite.Geometries.Coordinate(redTiles[i].x, redTiles[i].y);
	}

	pc[redTiles.Count] = new NetTopologySuite.Geometries.Coordinate(redTiles[0].x, redTiles[0].y);
	

	return gf.CreatePolygon(pc);
}

(double maxArea, ((double x, double y)? p1, (double x, double y)? p2) pair) 
	FindLargestRedGreenRectangle(List<(double x, double y)> coordinates, 
								 NetTopologySuite.Geometries.Polygon polygon)
{
	double maxArea = 0;
	((double X, double Y)? p1, (double X, double Y)? p2) bestPair = (null, null);

	// Brute force all pairs
	foreach (var pair in coordinates.SelectMany((p1, i) => coordinates.Skip(i + 1), (p1, p2) => (p1, p2)))
	{
		if (IsRectangleValid(pair.p1, pair.p2, polygon))
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
    }

	return (maxArea, bestPair);
}

bool IsRectangleValid((double x, double y) c1, (double x, double y) c2, NetTopologySuite.Geometries.Polygon polygon)
{
	var p1 = new NetTopologySuite.Geometries.Coordinate(c1.x, c1.y);
	var p2 = new NetTopologySuite.Geometries.Coordinate(c2.x, c2.y);
    var p3 = new NetTopologySuite.Geometries.Coordinate(c2.x, c1.y);
	var p4 = new NetTopologySuite.Geometries.Coordinate(c1.x, c2.y);

	var rect = gf.CreatePolygon([p1, p3, p2, p4, p1]);

    return rect.Within(polygon);
} 

