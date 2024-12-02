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
        case "puzzle_1a":
        case "p1a":
            if (input_parts.Length == 2) 
            {
                var output = Dec1.ProcessFirstChallenge(input_parts[1], "   ", 2);
                if (output > 0)
                {
                    Console.WriteLine(output);
                }
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
            Console.WriteLine("  puzzle_1a (p1a) <filename> - Processes file for the first puzzle on Dec 1.");
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

