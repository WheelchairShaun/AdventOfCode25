namespace Helpers
{
	public static class Input
	{
		public static string[] ReadInputFromFile(string testInput, string path = null)
		{
			if (File.Exists(path))
			{
				return File.ReadAllLines(path);
			}

			var lines = testInput.Split(System.Environment.NewLine);
			return lines;
		}

		public static string GetInputFromFileAsString(string testInput, string path = null)
		{
			if (File.Exists(path))
			{
				return File.ReadAllText(path);
			}

			return testInput;
		}
	}
}
