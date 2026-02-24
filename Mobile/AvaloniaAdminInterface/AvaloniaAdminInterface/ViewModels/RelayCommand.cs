using System;

public class RelayCommand
{
    private Action updateFunction;

    public RelayCommand(Action execute)
    {
        updateFunction = execute;
    }

    public void Execute()
    {
        updateFunction?.Invoke();
    }
}