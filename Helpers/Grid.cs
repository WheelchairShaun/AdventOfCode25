using System.Security.Cryptography.X509Certificates;

namespace Helpers
{
	public static class Grid
	{
		public static char[,] CreateGrid(string[] input)
		{
			int height = input.Length;                  // number of rows
			int width  = input.FirstOrDefault()?.Length ?? 0; // number of columns

			var grid = new char[width, height];         // [x,y] convention

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					grid[x, y] = input[y][x];
				}
			}

			return grid;
		}

		public static void Print(char[,] grid)
		{
			int width  = grid.GetLength(0); // x dimension
			int height = grid.GetLength(1); // y dimension

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Console.Write(grid[x, y]);
				}
				Console.WriteLine(); // end of row
			}
		}
	}
}