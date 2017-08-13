using System;
using System.Collections.Generic;


namespace State
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            Console.WriteLine("Current State:" + p.CurrentState);
            Console.WriteLine("Command.Begin:Current State=" + p.MoveToNextState(Command.Begin));
        }

        
    }

    public enum ProcessState
    {
        Inactive,
        Active,
        Paused,
        Terminated
    }

    public enum Command
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }

    public class Process
    {
        //StateTransition记录着当前状态以及一个命令，ProcessState表示新的状态
        //也就是，命令和下一个状态是预先设置好的
        Dictionary<StateTransition, ProcessState> transitions;
        public ProcessState CurrentState { get; private set; }

        public Process()
        {
            CurrentState = ProcessState.Inactive;
            transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(ProcessState.Inactive, Command.Exit), ProcessState.Terminated},
                { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.Active},
                { new StateTransition(ProcessState.Active, Command.End), ProcessState.Inactive},
                { new StateTransition(ProcessState.Active, Command.Pause), ProcessState.Paused},
                { new StateTransition(ProcessState.Paused, Command.End), ProcessState.Inactive},
                { new StateTransition(ProcessState.Paused, Command.Resume), ProcessState.Active}
            };
        }

        //根据输入的命令得到下一个状态
        public ProcessState GetNextState(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            ProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("invalid transition: " + CurrentState + "->" + command);
            return nextState;
        }

        public ProcessState MoveToNextState(Command command)
        {
            CurrentState = GetNextState(command);
            return CurrentState;
        }

        //记录当前的状态，以及给当前状态的一个命令
        class StateTransition
        {
            readonly ProcessState CurrentState;
            readonly Command command;

            public StateTransition(ProcessState currentState, Command command)
            {
                CurrentState = currentState;
                this.command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.command == other.command;
            }
        }
    }
}