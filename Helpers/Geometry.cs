namespace Helpers;

public static class Geometry
{
    /// <summary>
    /// Calculates the Euclidean distance between two 3D points.
    /// </summary>
    public static double Distance3D((double X, double Y, double Z) p1, (double X, double Y, double Z) p2)
    {
        double dx = p2.X - p1.X;
        double dy = p2.Y - p1.Y;
        double dz = p2.Z - p1.Z;

        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}
