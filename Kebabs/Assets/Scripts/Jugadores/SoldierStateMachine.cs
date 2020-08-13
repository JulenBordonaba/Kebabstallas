using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStateMachine : MonoBehaviour
{
    public SoldierState state;

    public bool IA = false;

    public void SetState(SoldierState _state)
    {
        state = _state;
        if(state!=null)
            try
            {
                StartCoroutine(state.Start());
            }
            catch
            {

            }
        
    }

    protected virtual void Update()
    {

        if(state!=null && IA)
        {
            state.Update();
        }
        
    }


}
