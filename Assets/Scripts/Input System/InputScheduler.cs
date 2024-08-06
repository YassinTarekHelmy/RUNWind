using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScheduler : MonoBehaviour
{
    private Queue<IInputCommand> _inputQueue = new Queue<IInputCommand>();
    
    private bool _isExecuting = false;

    private IInputCommand _currentCommand;
    private float _curerntTime;

    private void Update()
    {
            
        if (!_isExecuting) {
            
            if (_inputQueue.Count == 0)
                return;
            
            _currentCommand = _inputQueue.Dequeue();
            _curerntTime = Time.time;
            
            _isExecuting = true;
        }
        
        if (_currentCommand != null && _currentCommand.CanExecute())
        {
            
            _currentCommand.Execute();
            
            _isExecuting = false;
        } else if (_curerntTime + _currentCommand.MaxTimeToDiscard < Time.time)
        {
            _isExecuting = false;
            _currentCommand = null;

        }
            
        
    }

    public void AddCommand(IInputCommand command)
    {
        _inputQueue.Enqueue(command);
    }

}
