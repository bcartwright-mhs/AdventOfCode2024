using System.Diagnostics;

namespace AdventOfCode2024;

public static class Dec3
{
    public static int ProcessChallenge(string dataFileName)
    {
        if (!File.Exists(dataFileName))
        {
            Console.WriteLine("File not found: " + dataFileName);
            return -1;
        }

        using (var reader = new StreamReader(dataFileName))
        {
            try
            {
                int runningTotal = 0;
                int intNextChar;
                var stateMachine = new StateMachine();
                string[] validCommands = ["mul(", "do(", "don't("];
                string currentCommand = "";
                string firstOperand = "";
                string secondOperand = "";
                int position = 0;
                bool enabledMul = true;
                while ((intNextChar = reader.Read()) != -1)
                {
                    position++;

                    // Check if we invalidated and reset if so
                    if (stateMachine.CurrentState == State.Searching) 
                    {
                        currentCommand = "";
                        firstOperand = "";
                        secondOperand = "";
                    }
                    else if (stateMachine.CurrentState == State.Complete_Command_Found)
                    {
                        switch (currentCommand)
                        {
                            case "mul(":
                                int firstOp;
                                int secondOp;

                                if (!int.TryParse(firstOperand, out firstOp))
                                {
                                    throw new Exception($"Error trying to parse first operand {firstOperand}.  Current position is {position}");
                                }
                                if (!int.TryParse(secondOperand, out secondOp))
                                {
                                    throw new Exception($"Error trying to parse first operand {secondOperand}.  Current position is {position}");
                                }

                                if (enabledMul)
                                {
                                    runningTotal += (firstOp * secondOp);
                                    Console.WriteLine($"Found operation, firstOp={firstOperand}, secondOp={secondOperand}, runningTotal={runningTotal}");
                                }
                                else 
                                {
                                    Console.WriteLine($"Mul Disabled *** Found operation, firstOp={firstOperand}, secondOp={secondOperand}, runningTotal={runningTotal}");
                                }
                                currentCommand = "";
                                firstOperand = "";
                                secondOperand = "";
                                break;
                            case "do(":
                                enabledMul = true;
                                break;
                            case "don't(":
                                //enabledMul = false;
                                break;
                            default:
                                throw new Exception($"Invalid command idenfied {currentCommand}");
                        }
                        stateMachine.MoveNext(Trigger.Processed_Complete_Command);
                    }
                    
                    char nextChar = (char)intNextChar;
                    
                    switch (stateMachine.CurrentState)
                    {
                        case State.Searching:
                            {
                                currentCommand += nextChar;
                                string validatedCommand = ValidateCommandFragment(validCommands, currentCommand);
                                if (!string.IsNullOrEmpty(validatedCommand))
                                {
                                    stateMachine.MoveNext(Trigger.Found_Valid_Command_Start);
                                }
                                else 
                                {
                                    stateMachine.MoveNext(Trigger.Invalid_Value);
                                }
                            }
                            break;
                        case State.Command_Building:
                            {
                                currentCommand += nextChar;
                                string validatedCommand = ValidateCommandFragment(validCommands, currentCommand);
                                if (!string.IsNullOrEmpty(validatedCommand))
                                {
                                    // If we have the whole command, trigger statemachine and move on
                                    if (currentCommand == validatedCommand)
                                    {
                                        if (currentCommand == "mul(")
                                        {
                                            stateMachine.MoveNext(Trigger.Found_Command_With_Operands);
                                        }
                                        else 
                                        {
                                            stateMachine.MoveNext(Trigger.Found_Command_No_Operands);
                                        }
                                    }
                                }
                                else 
                                {
                                    stateMachine.MoveNext(Trigger.Invalid_Value);
                                }
                            }
                            break;
                        case State.First_Operand:
                            if (nextChar == ',')
                            {
                                if (firstOperand.Length > 0)
                                {
                                    stateMachine.MoveNext(Trigger.Found_Comma);
                                }
                                else 
                                {
                                    stateMachine.MoveNext(Trigger.Invalid_Value);
                                }
                            }
                            else 
                            {
                                if (char.IsNumber(nextChar))
                                {
                                    firstOperand += nextChar;
                                }
                                else 
                                {
                                    stateMachine.MoveNext(Trigger.Invalid_Value);
                                }
                            }
                            break;
                        case State.Second_Operand:
                            if (nextChar == ')')
                            {
                                if (secondOperand.Length > 0)
                                {
                                    stateMachine.MoveNext(Trigger.Found_Close_Paren);
                                }
                                else 
                                {
                                    stateMachine.MoveNext(Trigger.Invalid_Value);
                                }
                            }
                            else 
                            {
                                if (char.IsNumber(nextChar))
                                {
                                    secondOperand += nextChar;
                                }
                                else 
                                {
                                    stateMachine.MoveNext(Trigger.Invalid_Value);
                                }
                            }
                            break;
                        case State.No_Operands:
                            if (nextChar == ')')
                            {
                                stateMachine.MoveNext(Trigger.Found_Close_Paren);
                            }
                            else 
                            {
                                stateMachine.MoveNext(Trigger.Invalid_Value);
                            }
                            break;
                    }
                }

                
                return runningTotal;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                throw new Exception("", ex);
            }
        }
    }

    // Check for valid command
    // return empty if it is not
    // otherwise return the first complete command this matches
    public static string ValidateCommandFragment(string[] validCommands, string currentCommand)
    {
        foreach(string command in validCommands)
        {
            if (command.StartsWith(currentCommand))
            {
                return command;
            }
        }
        return "";
    }

    public enum State
    {
        Searching,
        Command_Building,
        First_Operand,
        Second_Operand,
        No_Operands,
        Complete_Command_Found
    }

    public enum Trigger
    {
        Found_Valid_Command_Start,
        Found_Command_No_Operands,
        Found_Command_With_Operands,
        Found_Comma,
        Found_Close_Paren,
        Processed_Complete_Command,
        Invalid_Value
    }

    public class StateMachine
    {
        private State _currentState;
        private Dictionary<(State, Trigger), State> _transitions;

        public StateMachine()
        {
            _currentState = State.Searching;
            _transitions = new Dictionary<(State, Trigger), State>
            {
                { (State.Searching, Trigger.Found_Valid_Command_Start), State.Command_Building },
                { (State.Command_Building, Trigger.Found_Command_With_Operands), State.First_Operand },
                { (State.Command_Building, Trigger.Found_Command_No_Operands), State.No_Operands },
                { (State.First_Operand, Trigger.Found_Comma), State.Second_Operand },
                { (State.Second_Operand, Trigger.Found_Close_Paren), State.Complete_Command_Found },
                { (State.No_Operands, Trigger.Found_Close_Paren), State.Complete_Command_Found },
                { (State.Complete_Command_Found, Trigger.Processed_Complete_Command), State.Searching },
            };
        }

        public void MoveNext(Trigger trigger)
        {
            if (_transitions.TryGetValue((_currentState, trigger), out State nextState))
            {
                _currentState = nextState;
                //Console.WriteLine($"State changed to {_currentState}");
            }
            else if (trigger == Trigger.Invalid_Value)
            {
                _currentState = State.Searching;
            }
            else
            {
                throw new Exception($"StateMachine Error: Invalid transition from {_currentState} using {trigger}");
            }
        }

        public State CurrentState => _currentState;
    }

}
