// See https://aka.ms/new-console-template for more information
using System;
using System.CommandLine;
using System.Reflection;
using AdventOfCode2024;

Console.Clear();
Console.WriteLine("Welcome to the Advent of Code 2024 Tool.");
Console.WriteLine("Type 'h' for a list of available commands.");
string prompt = "AOC2024>";
//DateTime utcDateTime;

Console.Write(string.Format(prompt, 0));
string? input = Console.ReadLine();
while (input != null && input.Length > 0 && input.ToLower() != "quit" && input.ToLower() != "q")
{
    string[] input_parts = input.Split(' ');
    string command = input_parts[0].ToLower();

    switch (command)
    {
        case "puzzle_1":
        case "p1":
            if (input_parts.Length == 2) 
            {
                var output = -1;

                // Do the first part
                output = Dec1.ProcessFirstChallenge(input_parts[1], "   ", 2);
            
                if (output > 0)
                {
                    Console.WriteLine($"Difference Score: {output}");
                }

                // Do the second part
                output = Dec1.ProcessSecondChallenge(input_parts[1], "   ", 2);


                if (output > 0)
                {
                    Console.WriteLine($"Similarity Score: {output}");
                }
            }
            else 
            {
                Console.WriteLine("Missing input file name");
            }
            break;
        case "puzzle_2":
        case "p2":
            bool useDampener = input_parts.Length == 3 && input_parts[2] == "--useDampener";
            bool bruteForce = input_parts.Length == 3 && input_parts[2] == "--bruteForce";
            if (input_parts.Length > 1) 
            {
                var output = -1;

                // Do the first part
                if (!bruteForce)
                {
                    output = Dec2.ProcessChallenge(input_parts[1], useDampener);
                }
                else
                {
                    output = Dec2.ProcessChallengeBruteForce(input_parts[1]);
                }
            
                if (output > 0)
                {
                    Console.WriteLine($"Safe report count: {output}");
                }

                // Do the second part
                /*output = Dec1.ProcessSecondChallenge(input_parts[1], "   ", 2);

                if (output > 0)
                {
                    Console.WriteLine($"Similarity Score: {output}");
                }*/
            }
            else 
            {
                Console.WriteLine("Missing input file name");
            }
            break;
        case "clear":
        case "cls":
            Console.Clear();
            break;
        case "version":
        case "v":
            Console.WriteLine("Version info not implemented yet!");
            break;
        case "help":
        case "h":
            Console.WriteLine("Commands:");
            Console.WriteLine("  puzzle_2 (p2) <filename> [--useDampener] [--bruteForce] - Processes file for the puzzles on Dec 2.");
            Console.WriteLine("  puzzle_1 (p1) <filename> - Processes file for the puzzles on Dec 1.");
            Console.WriteLine("  clear (cls)");
            Console.WriteLine("  version (v)");
            Console.WriteLine("  quit (q)");
            break;
        default:
            Console.WriteLine("Unknown command: " + input);
            break;
    }

    Console.WriteLine("");
    Console.Write(prompt);
    input = Console.ReadLine();
}

