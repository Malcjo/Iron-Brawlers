using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Jab, LegSweep, Aerial, ArmourBreak, HeavyJab };
public enum Attackdirection { Forward, Low, Aerial, Down };
public enum HitBoxScale { Jab, ArmourBreak, Aerial };
public enum FollowDes { Centre, RightHand, RightElbow, LeftHand, LeftElbow , RightFoot, LeftFoot}
public class Hitbox : MonoBehaviour
{
    public bool viewHitBox;
    public HitBoxScale _hitBoxScale;
    public Attackdirection _attackDir;
    public AttackType _attackType;
    private FollowDes _followDes;
    MeshRenderer meshRenderer;
    Collider hitboxCollider;

    [SerializeField] private float freezeCounter;
    [SerializeField] private float freezeStep;
    [SerializeField] private float freezeMaxValue;
    public PlayerActions animantionManager;
    public Animator anim;
    [SerializeField] Player player;
    PlayerInput playerInput;
    HitBoxManager hitBoxManager;

    [SerializeField] private bool freezeCharacter;


    public int armIndex;
    public GameObject tipHitBox, midHitBox;
    public GameObject rightHand, leftHand,rightElbow, leftElbow, rightFoot, leftFoot, rightKnee, leftKnee, waist;

    public ParticleSystem punchParticle;

    Vector3 hitDirection;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        hitBoxManager = GetComponent<HitBoxManager>();
        playerInput = GetComponentInParent<PlayerInput>();
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        HideHitBoxes();
    }
    private void Update()
    {
        AttackTypeCall();
        HitBoxSize();
    }
    void AttackTypeCall()
    {
        switch(_attackType)
        {
            case AttackType.Jab:
                FollowHand();
                break;
            case AttackType.LegSweep:
                FollowHand();
                break;
            case AttackType.Aerial:
                FollowRightElbow();
                break;
            case AttackType.ArmourBreak:
                FollowCenter();
                break;
            case AttackType.HeavyJab:
                FollowHand();
                break;
        }
    }
    void HitBoxSize()
    {
        switch (_hitBoxScale)
        {
            case HitBoxScale.Jab:
                transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                break;
            case HitBoxScale.ArmourBreak:
                transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                break;
            case HitBoxScale.Aerial:
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
        }
    }
    void HitboxPosition()
    {
        switch(_followDes)
        {
            case FollowDes.Centre:
                break;
            case FollowDes.RightHand:
                tipHitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightHand.transform.rotation;
                break;
            case FollowDes.RightElbow:
                tipHitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
                break;
            case FollowDes.RightFoot:
                tipHitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
                break;
            case FollowDes.LeftHand:
                tipHitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = leftHand.transform.rotation;
                break;
            case FollowDes.LeftElbow:
                break;
            case FollowDes.LeftFoot:
                tipHitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
                break;
        }
    }
    public void FollowCenter()
    {
        tipHitBox.gameObject.transform.position = waist.transform.position;
        tipHitBox.gameObject.transform.rotation = waist.transform.rotation;
    }
    public void FollowHand()
    {
        if (armIndex == 0)
        {
            tipHitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
            tipHitBox.gameObject.transform.rotation = leftHand.transform.rotation;
        }
        else if (armIndex == 1)
        {
            tipHitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
            tipHitBox.gameObject.transform.rotation = rightHand.transform.rotation;
        }
    }
    public void FollowRightElbow()
    {
        tipHitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
    }
    public void FollowRightFoot()
    {
        tipHitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
    }
    public void FollowLeftFoot()
    {
        tipHitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
    }

    public Vector3 HitDirection()
    {
        switch (_attackDir)
        {
            case Attackdirection.Forward:
                return new Vector3(player.FacingDirection(), 0.3f, 0);
            case Attackdirection.Low:
                return new Vector3(player.FacingDirection() * 0.1f, 1f,0);
            case Attackdirection.Aerial:
                return new Vector3(player.FacingDirection(), 0.5f, 0);
            case Attackdirection.Down:
                return new Vector3(player.FacingDirection(), -0.5f, 0);
            default:
                hitDirection.x = 1; hitDirection.y = 0.5f; hitDirection.z = 0; ;
                return new Vector3(player.FacingDirection(), 0.3f, 0);
        }
    }
    public float HitStrength()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                return 3;
            case AttackType.LegSweep:
                return 7;
            case AttackType.Aerial:
                return 3;
            case AttackType.ArmourBreak:
                return 5;
            case AttackType.HeavyJab:
                return 4;
        }
        return 0;
    }
    private void PlayHitSound()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                break;
            case AttackType.LegSweep:
                break;
            case AttackType.Aerial:
                break;
            case AttackType.ArmourBreak:
                break;
            case AttackType.HeavyJab:
                break;
        }
    }
    public void ShowHitBoxes()
    {
        meshRenderer.enabled = true;
        hitboxCollider.enabled = true;
    }
    public void HideHitBoxes()
    {
        meshRenderer.enabled = false;
        hitboxCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            return;
        }
        if (other.gameObject.CompareTag("Block"))
        {
            HideHitBoxes();
        }

        if (other.gameObject.CompareTag("Hurtbox"))
        {
            var tempAttackedPlayer = other.GetComponentInParent<Player>();
            var tempAttackingPlayer = GetComponentInParent<Player>();
            var tempArmour = other.GetComponentInParent<ArmourCheck>();
            HurtBox tempHurtBox = other.GetComponent<HurtBox>();

            if (tempAttackedPlayer.GetBlocking() == true)
            {
                HideHitBoxes();
                return;
            }
            else if (tempAttackedPlayer.GetBlocking() == false)
            {
                DamagePlayer(tempAttackedPlayer, tempAttackingPlayer);

                if (tempHurtBox.location == LocationTag.Chest)
                {
                    Debug.Log("Hit Chest");
                    if (tempArmour.ChestArmourType == ArmourCheck.Armour.none)
                    {
                        //Player just punch sound
                        PlayHitSound();
                        FindObjectOfType<AudioManager>().Play("Punch Hit");
                        Instantiate(punchParticle, transform.position, transform.rotation);
                        return;
                    }
                    else if(tempArmour.ChestArmourType == ArmourCheck.Armour.armour)
                    {
                        //Player Armour break sound
                        PlayHitSound();
                        Instantiate(punchParticle, transform.position, transform.rotation);
                        tempArmour.RemoveChestArmour();
                    }
                }
                else if (tempHurtBox.location == LocationTag.Legs)
                {
                    Debug.Log("Hit Legs");
                    if (tempArmour.LegArmourType == ArmourCheck.Armour.none)
                    {
                        //Player just punch sound
                        PlayHitSound();
                        FindObjectOfType<AudioManager>().Play("Punch Hit");
                        Instantiate(punchParticle, transform.position, transform.rotation);
                        return;
                    }
                    else if (tempArmour.LegArmourType == ArmourCheck.Armour.armour)
                    {
                        //Player Armour break sound
                        PlayHitSound();
                        Instantiate(punchParticle, transform.position, transform.rotation);
                        tempArmour.RemoveLegArmour();
                    }
                }
            }
        }
    }
    void DamagePlayer(Player attackedPlayer, Player attackingplayer)
    {
        attackedPlayer.FreezeCharacterBeingAttacked(HitDirection(), HitStrength());
        attackingplayer.FreezeCharacterAttacking();
        attackedPlayer.HitStun();
        Debug.Log("Hit Player");
        HideHitBoxes();
    }
}
