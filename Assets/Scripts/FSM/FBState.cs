using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FBState
{
    protected FireBotFSM botController;

    public FBState(FireBotFSM botController)
    {
        this.botController = botController; 
    }

    public virtual void Enter() { } // Used when entering a state to define the behaviour
    public virtual void Execute() { } // Used during the runtime of the state as defined by FireBotFSM
    public virtual void Exit() { } // Used when exiting a state to define the behaviour
}