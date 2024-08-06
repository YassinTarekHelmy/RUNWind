using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredicatedCommand : IInputCommand
{
    public Func<bool> CanExecute { get; set; }
    public float MaxTimeToDiscard { get; set; }

    private Action _action;
    
    public PredicatedCommand(Func<bool> canExecute, Action action, float maxTimeToDiscard)
    {
        CanExecute = canExecute;
        MaxTimeToDiscard = maxTimeToDiscard;
        _action = action;
    }

    public void Execute()
    {
        _action?.Invoke();
    }
}
