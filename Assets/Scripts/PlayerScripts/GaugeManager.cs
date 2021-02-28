using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GaugeManager : MonoBehaviour
{
    public Slider gauge;
    [SerializeField] ArmourCheck armourCheck;

    private WaitForSeconds repairTick = new WaitForSeconds(0.1f);
    private Coroutine repair;

    [SerializeField] private float currentGauge;
    private float maxGauge = 10;
    private float minGague = 0;

    [SerializeField] private float lowDamamgeValue = 2;
    [SerializeField] private float midDamamgeValue = 4;
    [SerializeField] private float highDamamgeValue = 7;
    [SerializeField] private float breakingPoint = 9f;

    private float DamageTick;
    private float lowDamamgeTick = 10;
    private float midDamamgeTick = 20;
    private float highDamamgeTick = 30;
    private float breakingPointTick = 50;

    private float WaitTime;
    private float lowDamamgeWait = 1.5f;
    private float midDamamgeWait = 2;
    private float highDamamgeWait = 3.5f;
    private float breakingPointWait = 4;

    private void Start()
    {
        currentGauge = minGague;
        gauge.maxValue = maxGauge;
        gauge.minValue = minGague;
        gauge.value = minGague;
    }
    private void Update()
    {
        if (currentGauge < 0) { currentGauge = 0; }
        GaugeVariableChecker();
    }
    public float GetArmourGaugeValue()
    {
        return currentGauge;
    }
    private void GaugeVariableChecker()
    {
        if (currentGauge <= lowDamamgeValue)
        {
            WaitTime = lowDamamgeWait;
            DamageTick = lowDamamgeTick;
        }
        else if (currentGauge <= midDamamgeValue)
        {
            WaitTime = midDamamgeWait;
            DamageTick = midDamamgeTick;
        }
        else if (currentGauge <= highDamamgeValue)
        {
            WaitTime = highDamamgeWait;
            DamageTick = highDamamgeTick;
        }
        else if (currentGauge <= breakingPoint)
        {
            WaitTime = breakingPointWait;
            DamageTick = breakingPointTick;
        }
    }

    public void TakeDamage(float amount, ArmourCheck.ArmourPlacement Placement, Player defendingPlayer, AttackType attackType)
    {
        if(currentGauge + amount <= breakingPoint)
        {
            currentGauge += amount;
            gauge.value = currentGauge;

            if(repair != null)
            {
                StopCoroutine(repair);
            }
            repair = StartCoroutine(RepairGauge());
        }
        else
        {
            armourCheck.DestroyArmour(Placement, defendingPlayer, attackType);
            resetGauge();
        }

    }
    private IEnumerator RepairGauge()
    {
        yield return new WaitForSeconds(WaitTime);
        while (currentGauge < breakingPoint)
        {
            currentGauge -= maxGauge / DamageTick;
            gauge.value = currentGauge;
            yield return repairTick;
            if (currentGauge == minGague)
            {
                setGaugeToZero();
            }
        }
        repair = null;
    }
    private void setGaugeToZero()
    {
        StopCoroutine(repair);
        currentGauge = minGague;
        gauge.value = currentGauge;
    }
    private void resetGauge()
    {
        currentGauge = minGague;
        gauge.value = currentGauge;
    }
}
