using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputCommand
{
    public Func<bool> CanExecute { get; set; }
    public float MaxTimeToDiscard { get; set; }
    public void Execute();
}
