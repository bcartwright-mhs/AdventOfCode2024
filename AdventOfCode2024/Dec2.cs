using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2024;

public static class Dec2
{
    public static int ProcessChallenge(string dataFileName, bool useDampener = false)
    {
        if (!File.Exists(dataFileName))
        {
            Console.WriteLine("File not found: " + dataFileName);
            return -1;
        }

        int safeReportCount = 0;
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
                        // Parse the file
                        try
                        {
                            var reportIsSafe = Dec2.ReportLineIsSafe(line, lineNumber, useDampener);
                            if (reportIsSafe == null)
                            {
                                Console.WriteLine($"Error processing file at line {lineNumber}");
                                throw new Exception($"Error processing file at line {lineNumber}");
                            }
                            else if (reportIsSafe == true)
                            {
                                Console.WriteLine($"Safe report found on line number {lineNumber}");
                                safeReportCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing file: " + ex.Message);
                            throw new Exception($"Error processing file at line {lineNumber}", ex);
                        }
                    }
                }
                return safeReportCount;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading from file: " + ex.Message);
                throw new Exception($"Error reading from file:", ex);
                //return -1;
            }
        }
    }

    public static bool? ReportLineIsSafe(string line, int lineNumber, bool useDampener = false)
    {
        var items = line.Split(' ').ToList();
        if (items.Count > 1) 
        {
            bool? goingUp = null;
            int previousReport = 0;
            bool firstItem = true; 
            int dampeningsRemaining = useDampener ? 1 : 0;

            for (int i=0;i<items.Count;i++)
            {
                var strReport = items[i];
                bool dampenThisReport = false;
                int intReport = 0;
                if (!int.TryParse(strReport, out intReport)) 
                {
                    Console.WriteLine($"Error converting {strReport} to int on line number {lineNumber}");
                    return null;
                }

                if (!firstItem)
                {
                    // Check if the report change is between 1 and 3
                    int reportDiff = Math.Abs(previousReport - intReport);
                    if (reportDiff < 1 || reportDiff > 3)
                    {
                        dampenThisReport = (dampeningsRemaining > 0) && useDampener;
                        if (dampenThisReport)
                        {
                            string temp1 = line.Substring(line.IndexOf(' ') + 1);
                            if (ReportLineIsSafe(temp1, lineNumber) == true)
                            {
                                //Console.WriteLine($"Dampener found safe line at {lineNumber} by removing the first item after a big jump");
                                return true;
                            }

                            var tempList = new List<string>(items);
                            tempList.RemoveAt(i);
                            string temp2 = string.Join(' ', tempList);
                            if (ReportLineIsSafe(temp2, lineNumber) == true)
                            {
                                //Console.WriteLine($"Dampener found safe line at {lineNumber} by removing the i-1 item");
                                return true;
                            }
                            
                            dampeningsRemaining--;
                        }
                        
                        if (dampeningsRemaining == 0)
                        {
                            return false;
                        }
                    }
                    else if (intReport > previousReport && (goingUp == null || goingUp == true)) 
                    {
                        goingUp = true;
                    }
                    else if (intReport < previousReport && (goingUp == null || goingUp == false))
                    {
                        goingUp = false;
                    }
                    else 
                    {
                        // Two numbers changed direction
                        dampenThisReport = (dampeningsRemaining > 0) && useDampener;
                        if (dampenThisReport)
                        {
                            string temp1 = line.Substring(line.IndexOf(' ') + 1);
                            if (ReportLineIsSafe(temp1, lineNumber) == true)
                            {
                                //Console.WriteLine($"Dampener found safe line at {lineNumber} by removing the first item after a direction change.");
                                return true;
                            }

                            // Check if this item was the bad one in the dir change
                            var tempList = new List<string>(items);
                            tempList.RemoveAt(i);
                            string temp2 = string.Join(' ', tempList);
                            if (ReportLineIsSafe(temp2, lineNumber) == true)
                            {
                                //Console.WriteLine($"Dampener found safe line at {lineNumber} by removing the i-1 item");
                                return true;
                            }

                            // Check if the previous item was the bad one in the dir change
                            if (i > 1)
                            {
                                var tempList2 = new List<string>(items);
                                tempList2.RemoveAt(i-1);
                                string temp3 = string.Join(' ', tempList2);
                                if (ReportLineIsSafe(temp3, lineNumber) == true)
                                {
                                    //Console.WriteLine($"Dampener found safe line at {lineNumber} by removing the i-1 item");
                                    return true;
                                }
                            }
                            dampeningsRemaining--;
                        }
                        
                        if (dampeningsRemaining == 0)
                        {       
                            return false;
                        }
                    }
                    
                }
                else 
                {
                    firstItem = false;
                }

                // Skip this item if we want to dampen this report and we have no used up all our dampenings
                if (!dampenThisReport || dampeningsRemaining <= 0) 
                {
                    previousReport = intReport;
                }            
            }
            return true;
        }
        Console.WriteLine($"Report line is empty at {lineNumber}");
        return null;
    }

    public static int ProcessChallengeBruteForce(string dataFileName)
    {
        if (!File.Exists(dataFileName))
        {
            Console.WriteLine("File not found: " + dataFileName);
            return -1;
        }

        int safeReportCount = 0;
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
                        // Parse the file
                        try
                        {
                            var reportIsSafe = Dec2.ReportLineIsSafe(line, lineNumber);
                            if (reportIsSafe == null)
                            {
                                Console.WriteLine($"Error processing file at line {lineNumber}");
                                throw new Exception($"Error processing file at line {lineNumber}");
                            }
                            else if (reportIsSafe == true)
                            {
                                Console.WriteLine($"Safe report found on line number {lineNumber}");
                                safeReportCount++;
                            }
                            else 
                            {
                                // Brute force dampener
                                var items = line.Split(' ').ToList();
                                if (items.Count > 1) 
                                {
                                    for (int i=0;i<items.Count;i++)
                                    {
                                        var tempList = new List<string>(items);
                                        tempList.RemoveAt(i);
                                        string temp2 = string.Join(' ', tempList);
                                        if (ReportLineIsSafe(temp2, lineNumber) == true)
                                        {
                                            Console.WriteLine($"Safe report found on line number {lineNumber}");
                                            safeReportCount++;
                                            i=items.Count;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing file: " + ex.Message);
                            throw new Exception($"Error processing file at line {lineNumber}", ex);
                        }
                    }
                }
                return safeReportCount;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading from file: " + ex.Message);
                throw new Exception($"Error reading from file:", ex);
                //return -1;
            }
        }
    }
    

}
