using System.Security.Cryptography.X509Certificates;

namespace Helpers
{
	public static class Grid
	{
		public static char[,] CreateGrid(string[] input)
		{
			int rows = input.Length;
			int cols = input.FirstOrDefault()?.Length ?? 0;

			var grid = new char[cols, rows];

			for (int row = 0; row < rows; row++)
			{
				for (int col = 0; col < cols; col++)
				{
					grid[col, row] = input[row][col];
				}
			}

			return grid;
		}
		
		public static void Print(char[,] grid)
		{
			int cols = grid.GetLength(0);
			int rows = grid.GetLength(1);

			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					Console.Write(grid[x, y]);
				}
				Console.WriteLine(); // end of row
			}

		}
	}
}