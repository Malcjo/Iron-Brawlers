using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempRoundManager : MonoBehaviour
{
    public static TempRoundManager instance;
    public int player1Rounds, player2Rounds;
    [SerializeField] private GameObject player1, player2;
    [SerializeField] private Transform player1Spawn, Player2Spawn;
    [SerializeField] private int leftBounds;
    [SerializeField] private int rightBounds;
    [SerializeField] private int belowBounds;
    [SerializeField] private int highBounds;
    // Start is called before the first frame update
    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    if (instance != this)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
    void Start()
    {
        
    }
    private void TrackPlayers()
    {
        if (player1Rounds == 3 || player2Rounds == 3)
        {
            SetRoundsToZero();
            SceneManager.LoadScene(0);
        }
        TrackPlayer1();
        TrackPlayer2();
    }
    private void TrackPlayer1()
    {
        if (player1.transform.position.y <= belowBounds)
        {
            player2Rounds++;
            ResetPlayers();
        }
    }

    private void TrackPlayer2()
    {
        if (player2.transform.position.y <= belowBounds)
        {
            player1Rounds++;
            ResetPlayers();
        }
    }
    private void ResetPlayers()
    {
        player1.gameObject.GetComponent<ArmourCheck>().SetAllArmourOn();
        player1.gameObject.GetComponent<Player>().StopMovingCharacterOnYAxis();
        player1.gameObject.GetComponent<Player>().StopMovingCharacterOnXAxis();
        player1.transform.position = player1Spawn.transform.position;
        player2.gameObject.GetComponent<ArmourCheck>().SetAllArmourOn();
        player2.gameObject.GetComponent<Player>().StopMovingCharacterOnYAxis();
        player2.gameObject.GetComponent<Player>().StopMovingCharacterOnXAxis();
        player2.transform.position = Player2Spawn.transform.position;
    }
    public void SetRoundsToZero()
    {
        player1Rounds = 0;
        player2Rounds = 0;
    }
    // Update is called once per frame
    void Update()
    {
        TrackPlayers();
    }
}
