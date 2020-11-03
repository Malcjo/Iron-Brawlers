using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] PlayerActions playerActions;
    public void Jab()
    {
        playerActions.JabCombo();
    }
    public void AerialAttack()
    {
        playerActions.AerialAttack();
    }
    public void LegSweep()
    {
        playerActions.LegSweep();
    }
    public void ArmourBreak()
    {
        playerActions.ArmourBreak();
    }
    private void Block() 
    {
        playerActions.Block();
    }
    private void StopBlock()
    {
        playerActions.StopBlock();
    }
    public void Heavy()
    {
        playerActions.Heavy();
    }
}
