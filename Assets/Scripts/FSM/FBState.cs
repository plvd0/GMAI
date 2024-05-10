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

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
