using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    AnimationManagerNew animationManager;
    Hitbox hitboxScript;
    Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
        hitboxScript = GetComponentInChildren<Hitbox>();
        animationManager = GetComponentInChildren<AnimationManagerNew>();
    }
    public void Jab()
    {
        hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.Jab;
        animationManager.JabCombo();
    }
    public void AerialAttack()
    {
        hitboxScript._attackType = AttackType.Aerial;
        hitboxScript._attackDir = Attackdirection.Aerial;
        animationManager.AerialAttack();
        player.inAnimation = false;
    }
    public void LegSweep()
    {
        hitboxScript._attackDir = Attackdirection.Low;
        hitboxScript._attackType = AttackType.LegSweep;
        animationManager.LegSweep();
    }
    public void ArmourBreak()
    {
        animationManager.ArmourBreak();
    }

    public void Heavy()
    {
        hitboxScript._attackDir = Attackdirection.Forward;
        hitboxScript._attackType = AttackType.HeavyJab;
        animationManager.Heavy();
    }
}
