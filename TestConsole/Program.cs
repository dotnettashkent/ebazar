using System;
using System.Collections.Generic;

class Program
{
	static void Main()
	{
		// Example input string
		string inputString = "10|20|30|40|50";

		// Split the string based on the '|' delimiter
		string[] substrings = inputString.Split('|');

		// Convert the array to a List<string>
		List<string> stringList = new List<string>(substrings);

		// Print the elements in the list
		foreach (string item in stringList)
		{
			Console.WriteLine(item);
		}
	}
}
