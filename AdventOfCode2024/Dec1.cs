namespace AdventOfCode2024;

public static class Dec1
{
    public static int ProcessFirstChallenge(string dataFileName, string dataSeperator, int expectedItemCount)
    {
        if (!File.Exists(dataFileName))
        {
            Console.WriteLine("File not found: " + dataFileName);
            return -1;
        }
        
        var firstList = new List<int>();
        var secondList = new List<int>();

        using (var reader = new StreamReader(dataFileName))
        {
            try
            {
                int lineNumber = 0;
                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = reader.ReadLine();

                    if (line != null && line.Length > 1)
                    {
                        // Parse the file into 2 lists
                        try
                        {
                            var items = line.Split(dataSeperator);
                            if (items.Length == expectedItemCount) 
                            {
                                int int1 = 0;
                                int int2 = 0;

                                if (!int.TryParse(items[0], out int1)) 
                                {
                                    Console.WriteLine($"Error converting {items[0]} to int on line number {lineNumber}");
                                    return -1;
                                }
                                if (!int.TryParse(items[1], out int2)) 
                                {
                                    Console.WriteLine($"Error converting {items[1]} to int on line number {lineNumber}");
                                    return -1;
                                }
                                firstList.Add(int1);
                                secondList.Add(int2);
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing file: " + ex.Message);
                        }
                    }
                }

                // Sort the Lists
                firstList.Sort();
                secondList.Sort();

                // Loop through and find the diff
                int totalDiff = 0;
                for(int i=0;i<firstList.Count;i++)
                {
                    totalDiff += Math.Abs(firstList[i] - secondList[i]);
                    if (firstList[i] < secondList[i])
                    {
                        Console.WriteLine($"First is less than second on line {i}");
                        
                    }
                }
                return totalDiff;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading from file: " + ex.Message);
                return -1;
            }
        }
    }

    public static int ProcessSecondChallenge(string dataFileName, string dataSeperator, int expectedItemCount)
    {
        if (!File.Exists(dataFileName))
        {
            Console.WriteLine("File not found: " + dataFileName);
            return -1;
        }
        
        var firstList = new List<int>();
        var secondList = new List<int>();

        using (var reader = new StreamReader(dataFileName))
        {
            try
            {
                int lineNumber = 0;
                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = reader.ReadLine();

                    if (line != null && line.Length > 1)
                    {
                        // Parse the file into 2 lists
                        try
                        {
                            var items = line.Split(dataSeperator);
                            if (items.Length == expectedItemCount) 
                            {
                                int int1 = 0;
                                int int2 = 0;

                                if (!int.TryParse(items[0], out int1)) 
                                {
                                    Console.WriteLine($"Error converting {items[0]} to int on line number {lineNumber}");
                                    return -1;
                                }
                                if (!int.TryParse(items[1], out int2)) 
                                {
                                    Console.WriteLine($"Error converting {items[1]} to int on line number {lineNumber}");
                                    return -1;
                                }
                                firstList.Add(int1);
                                secondList.Add(int2);
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing file: " + ex.Message);
                        }
                    }
                }

                // Sort the Lists
                firstList.Sort();
                secondList.Sort();

                // Loop through and find the similarity score
                int similarityScore = 0;
                for(int i=0;i<firstList.Count;i++)
                {
                    similarityScore += firstList[i] * secondList.FindAll(s => s == firstList[i]).Count();
                }
                return similarityScore;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading from file: " + ex.Message);
                return -1;
            }
        }
    }

}
