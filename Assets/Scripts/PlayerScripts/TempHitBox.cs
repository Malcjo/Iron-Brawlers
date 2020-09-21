using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempHitBox : MonoBehaviour
{
    public Attackdirection _attackDir;
    public enum Attackdirection { Forward, Neutral, High, Low };

    public AttackType _attackType;
    public enum AttackType { Jab, LegSweep };

    private bool followArm = false;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Player player;
    [SerializeField] private ArmourCheck armourCheck;

    public int armIndex;
    public GameObject rightArm, leftArm, foot;

    private Vector3 hitDirection;

    private float countdownDelay;
    private float maxCountdownDelay;
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        player = GetComponentInParent<Player>();
        armourCheck = GetComponentInParent<ArmourCheck>();
    }
    private void Update()
    {
        AttackTypeCall();
        Delay();
    }
    void AttackTypeCall()
    {
        switch(_attackType)
        {
            case AttackType.Jab:
                FollowArm();
                break;

            case AttackType.LegSweep:
                FollowLeg();
                break;
            default:
                break;
        }
    }
    public void FollowArm()
    {
        if (armIndex == 0)
        {
            this.gameObject.transform.position = leftArm.transform.position;
            this.gameObject.transform.rotation = leftArm.transform.rotation;
        }
        else if (armIndex == 1)
        {
            this.gameObject.transform.position = rightArm.transform.position;
            this.gameObject.transform.rotation = rightArm.transform.rotation;
        }
    }
    public void FollowLeg()
    {
        this.gameObject.transform.position = foot.transform.position;
        this.gameObject.transform.rotation = foot.transform.rotation;
    }

    public Vector3 HitDirection()
    {
        switch (_attackDir)
        {
            case Attackdirection.Forward:
                return new Vector3(playerInput.FacingDirection, 0.3f, 0);
            case Attackdirection.Neutral:
                return Vector3.zero;
            case Attackdirection.High:
                return Vector3.zero;
            case Attackdirection.Low:
                return new Vector3(playerInput.FacingDirection * 0.1f, 1f,0);
            default:
                hitDirection.x = 1; hitDirection.y = 0.5f; hitDirection.z = 0; ;
                return new Vector3(hitDirection.x * playerInput.FacingDirection, hitDirection.y, 0);
        }
    }
    
    public float HitStrength()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                return 15;
            case AttackType.LegSweep:
                return 10;
            default:
                return 15;
        }
    }
    void Delay()
    {
        countdownDelay -= 1 * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var _tempArmourCheck = GetComponent<ArmourCheck>();
        }
        if(other.tag == "Block")
        {
            this.gameObject.SetActive(false);
        }
        if(other.tag == "LegArmour")
        {
            var _tempArmourCheck = other.GetComponentInParent<ArmourCheck>();
            _tempArmourCheck.LegArmourType = ArmourCheck.Armour.none;
            _tempArmourCheck.SetArmourOff(ArmourCheck.ArmourType.Legs);
        }
        if(other.tag == "ChestArmour")
        {
            var _temptArmourCheck = other.GetComponentInParent<ArmourCheck>();
            _temptArmourCheck.ChestArmourType = ArmourCheck.Armour.none;
            _temptArmourCheck.SetArmourOff(ArmourCheck.ArmourType.Chest);
        }
    }

}
