using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerCharacterEnum.Characters character;
    [SerializeField]
    private Player player;
    private PlayerInput playerInput;
    //private PlayerControls playerControls;
    private Player.PlayerIndex _PlayerNumber;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] private GameObject sol = null;
    [SerializeField] private GameObject goblin = null;
    GameObject playerCharacter = null;

    public int PlayerIndex;
    [SerializeField] private float testfloat;
    [SerializeField] Scene currentScene;
    [SerializeField] Scene menuScene;
    private bool readyAndWaiting = false;

    public bool canAct = false;
    public bool read = false;

    public bool Readied = false;
    public float chara = 0;
    public bool primed = false;

    [SerializeField]
    private bool JumpInputQueued;
    [SerializeField]
    private bool BlockInputQueued;
    [SerializeField]
    private bool AttackInputQueued;
    [SerializeField]
    private bool ArmourBreakQueued;
    [SerializeField]
    private bool CrouchInputHeld;
    [SerializeField]
    private bool blockInputHeld;
    [SerializeField]
    private bool ArmourBreakInputQueued;
    [SerializeField]
    private float HorizontalValue;
    [SerializeField]
    private float horizontalInput;
    [SerializeField]
    private bool heavyQueued;
    [SerializeField]
    private bool UpDirectionHeld;

    [SerializeField] Player.Wall currentWall;
    [SerializeField] private bool Standalone = false;

    private CameraScript cameraScript;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        menuScene = SceneManager.GetSceneByBuildIndex(0);
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        //currentWall = player.GetCurrentWall();
        if(playerCharacter == null)
        {
            if(SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByBuildIndex(1).buildIndex)
            {
                currentScene = SceneManager.GetActiveScene();
                StartGame();
            }
        }
        if (readyAndWaiting)
        {
            GameManager.instance.ReadyPlayer(PlayerIndex);
        }
    }
    private void StartGame()
    {
        playerCharacter = Instantiate(playerPrefab);
        GameManager.instance.AddPlayerToList(playerCharacter);
        cameraScript = GameManager.instance.GetCameraScript();
        cameraScript.AddPlayers(playerCharacter);
        player = playerCharacter.GetComponent<Player>();
        player.SetUpInputDetectionScript(this);
        player.playerNumber = _PlayerNumber;
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    public void SetInput(PlayerInput input)
    {
        this.playerInput = input;
    }

    public void SetPlayerNumber(PlayerInputManager inputManager)
    {
        if (inputManager.playerCount == 1)
        {
            _PlayerNumber = Player.PlayerIndex.Player1;
        }
        else if (inputManager.playerCount == 2)
        {
            _PlayerNumber = Player.PlayerIndex.Player2;
        }
    }
    #region GetInputs
    public float GetHorizontal()
    {
        return HorizontalValue;
    }
    public bool ShouldJump(){
        if (JumpInputQueued)
        {
            JumpInputQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldAttack(){
        if (AttackInputQueued)
        {
            AttackInputQueued = false;
            return true;
        }
        return false;
    }

    public bool ShouldCrouch(){
        return CrouchInputHeld;
    }
    public bool ShouldBlock()
    {
        return blockInputHeld;
    }
    public bool ShouldArmourBreak()
    {
        if (ArmourBreakInputQueued)
        {
            ArmourBreakInputQueued = false;
            return true;
        }
        return false;
    }
    public bool ShouldHeavy()
    {
        if (heavyQueued)
        {
            heavyQueued = false;
            return true;
        }
        return false;
    }

    public bool ShouldUpDirection()
    {
        return UpDirectionHeld;
    }
    #endregion
    void CharacterSwitch()
    {
        character = (PlayerCharacterEnum.Characters)chara;
        switch (character)
        {
            case PlayerCharacterEnum.Characters.Sol:
                playerPrefab = sol;
                break;
            case PlayerCharacterEnum.Characters.Goblin:
                playerPrefab = goblin;
                break;
        }
    }

    public void HorizontalInput(CallbackContext context)
    {
        if(currentScene.buildIndex == SceneManager.GetSceneByBuildIndex(0).buildIndex && !Readied)
        {
            if (context.started)
            {
                testfloat = context.ReadValue<float>();
                primed = false;
                if(context.ReadValue<float>() >=1f)
                {
                    chara--;
                    if(chara < 0)
                    {
                        chara = (int)PlayerCharacterEnum.Characters.End - 1;
                    }
                    CharacterSwitch();
                }
                else if (context.ReadValue<float>() <=1f)
                {
                    chara++;
                    if(chara == (int)PlayerCharacterEnum.Characters.End)
                    {
                        chara = 0;
                    }
                    CharacterSwitch();
                }
            }
            if (context.canceled)
            {
                primed = true;
            }
        }
        else
        {
            if (player != null)
            {
                horizontalInput = context.ReadValue<float>();
                if (horizontalInput <= 0.35f && horizontalInput >= -0.35f)
                {
                    if (horizontalInput < 0 && horizontalInput >= -0.35f)
                    {
                        horizontalInput = -0;
                    }
                    else if (horizontalInput > 0 && horizontalInput <= 0.35f)
                    {
                        horizontalInput = 0;
                    }
                }
                HorizontalValue = horizontalInput;
                player.GetPlayerInputFromInputScript(HorizontalValue);
                WallCheck();
            }
        }
    }

    public void Activate(CallbackContext context)
    {
        if (canAct)
        {
            if(context.started && primed)
            {
                primed = false;
                Readied = true;
                if (Readied)
                {
                    readyAndWaiting = true;
                }
            }
            //else if (context.canceled)
            //{
            //    Debug.Log("Active cancelled");
            //    primed = true;
            //    Readied = false;
            //}
        }
    }

    public void JumpInput(CallbackContext context)
    {
        if (context.started)
        {
            JumpInputQueued = true;
        }
    }

    public void AttackInput(CallbackContext context)
    {
        if (context.started)
        {
            AttackInputQueued = true;
        }
    }
    public void CrouchInput(CallbackContext context)
    {
        if (context.started)
        {
            CrouchInputHeld = true;
        }
        if (context.canceled)
        {
            CrouchInputHeld = false;
        }
    }

    public void BlockInput(CallbackContext context)
    {
        if (context.started)
        {
            blockInputHeld = true;
        }
        if (context.canceled)
        {
            blockInputHeld = false;
        }
    }

    public void ArmourBreakInput(CallbackContext context)
    {
        if (context.started)
        {
            ArmourBreakInputQueued = true;
        }
    }
    public void HeavyInput(CallbackContext context)
    {
        if (context.started)
        {
            heavyQueued = true;
        }
    }

    public void UpDirectionInput(CallbackContext context)
    {
        if (context.started)
        {
            UpDirectionHeld = true;
        }
        if (context.canceled)
        {
            UpDirectionHeld = false;
        }
    }

    public void WallCheck()
    {
        switch (player.GetCurrentWall())
        {
            case Player.Wall.none:
                break;
            case Player.Wall.leftWall:
                if (HorizontalValue < 0)
                {
                    HorizontalValue = 0;
                }
                break;
            case Player.Wall.rightWall:
                if (HorizontalValue > 0)
                {
                    HorizontalValue = 0;
                }
                break;
        }
    }
}   

