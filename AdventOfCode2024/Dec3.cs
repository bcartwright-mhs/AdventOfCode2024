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
                string fullCommand = "mul(";
                string currentCommand = "";
                string firstOperand = "";
                string secondOperand = "";
                int position = 0;
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
                    else if (stateMachine.CurrentState == State.CompleteCommandFound)
                    {
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
                        runningTotal += (firstOp * secondOp);
                        Console.WriteLine($"Found operation, firstOp={firstOperand}, secondOp={secondOperand}, runningTotal={runningTotal}");

                        currentCommand = "";
                        firstOperand = "";
                        secondOperand = "";

                        stateMachine.MoveNext(Trigger.Processed_Complete_Command);
                    }
                    
                    char nextChar = (char)intNextChar;
                    
                    if (stateMachine.CurrentState == State.Searching && nextChar == 'm') 
                    {
                        stateMachine.MoveNext(Trigger.Found_m);
                        currentCommand += nextChar;
                    }
                    else if (stateMachine.CurrentState != State.Searching) 
                    {
                        switch (stateMachine.CurrentState)
                        {
                            case State.Command:
                                if (nextChar == fullCommand.ToCharArray()[currentCommand.Length])
                                {
                                    currentCommand += nextChar;
                                    // If we have the whole command, trigger statemachine and move on
                                    if (currentCommand == fullCommand)
                                    {
                                        stateMachine.MoveNext(Trigger.Found_Open_Paren);
                                    }
                                }
                                else 
                                {
                                    stateMachine.MoveNext(Trigger.Invalid_Value);
                                }
                                break;
                            case State.FirstOperand:
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
                            case State.SecondOperand:
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
                        }
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

    public enum State
    {
        Searching,
        Command,
        FirstOperand,
        SecondOperand,
        CompleteCommandFound,
    }

    public enum Trigger
    {
        Found_m,
        Found_Open_Paren,
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
                { (State.Searching, Trigger.Found_m), State.Command },
                { (State.Command, Trigger.Found_Open_Paren), State.FirstOperand },
                { (State.FirstOperand, Trigger.Found_Comma), State.SecondOperand },
                { (State.SecondOperand, Trigger.Found_Close_Paren), State.CompleteCommandFound },
                { (State.CompleteCommandFound, Trigger.Processed_Complete_Command), State.Searching },
                { (State.Command, Trigger.Invalid_Value), State.Searching },
                { (State.FirstOperand, Trigger.Invalid_Value), State.Searching },
                { (State.SecondOperand, Trigger.Invalid_Value), State.Searching },
            };
        }

        public void MoveNext(Trigger trigger)
        {
            if (_transitions.TryGetValue((_currentState, trigger), out State nextState))
            {
                _currentState = nextState;
                //Console.WriteLine($"State changed to {_currentState}");
            }
            else
            {
                throw new Exception($"StateMachine Error: Invalid transition from {_currentState} using {trigger}");
            }
        }

        public State CurrentState => _currentState;
    }

}
