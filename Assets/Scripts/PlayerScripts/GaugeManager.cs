using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GaugeManager : MonoBehaviour
{
    public Slider gauge;
    [SerializeField] ArmourCheck armourCheck;
    [SerializeField] RectTransform gaugeRect;

    private WaitForSeconds repairTick = new WaitForSeconds(0.1f);
    private Coroutine repair;

    [SerializeField] private float currentGauge;
    private float maxGauge = 10;
    private float minGague = 0;

    [SerializeField] private Image GaugeImage;

    private Color lowDamamgeColour = new Color(0.432644f, 1, 0, 1);
    private Color midDamamgeColour = new Color(0.824272f, 1, 0, 1);
    private Color highDamamgeColour = new Color(1, 0.5828071f, 0, 1);
    private Color breakingPointColour = new Color(1, 0.1717473f, 0, 1);
    [SerializeField] private Color previousColour;
    [SerializeField] private Color currentColour;

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
        previousColour = lowDamamgeColour;
        currentColour = lowDamamgeColour;
    }
    private void Update()
    {
        if (currentGauge < 0) { currentGauge = 0; }
        GaugeVariableChecker();
        GaugeImage.color = currentColour;
    }
    private void GaugeVariableChecker()
    {
        if (currentGauge <= lowDamamgeValue)
        {
            if (currentColour == midDamamgeColour)
            {
                previousColour = midDamamgeColour;
            }
            else
            {
                previousColour = lowDamamgeColour;
            }
            currentColour = lowDamamgeColour;
            WaitTime = lowDamamgeWait;
            DamageTick = lowDamamgeTick;
        }
        else if (currentGauge <= midDamamgeValue)
        {
            if (currentColour == lowDamamgeColour)
            {
                previousColour = lowDamamgeColour;
            }
            else if (currentColour == highDamamgeColour)
            {
                previousColour = highDamamgeColour;
            }
            currentColour = midDamamgeColour;
            WaitTime = midDamamgeWait;
            DamageTick = midDamamgeTick;
        }
        else if (currentGauge <= highDamamgeValue)
        {
            if (currentColour == midDamamgeColour)
            {
                previousColour = midDamamgeColour;
            }
            else if (currentColour == breakingPointColour)
            {
                previousColour = breakingPointColour;
            }
            currentColour = highDamamgeColour;
            WaitTime = highDamamgeWait;
            DamageTick = highDamamgeTick;
        }
        else if (currentGauge <= breakingPoint)
        {
            if (currentColour == highDamamgeColour)
            {
                previousColour = highDamamgeColour;
            }
            currentColour = breakingPointColour;
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
