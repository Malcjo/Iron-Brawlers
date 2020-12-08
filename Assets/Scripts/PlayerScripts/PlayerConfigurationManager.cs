using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    [SerializeField] private List<PlayerConfiguration> playerConfigs;

    [SerializeField] private int maxPlayers = 2;

    public static PlayerConfigurationManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Singleton trying to create instance");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }


    //public void SetPlayerCharacter(int Index, int characterIndex)
    //{
    //    GameManager.instance.SetPlayerCharacter(Index, Characters[characterIndex]);
    //}

    //public void ReadyPlayer(int Index)
    //{
    //    playerConfigs[Index].isReady = true;
    //    if(playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true))
    //    {
    //        //Load scene
    //    }
    //}
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player joined " + pi.playerIndex);
        pi.transform.SetParent(transform);

        if (!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(pi));
            Debug.Log("added player to list, list size now " + playerConfigs.Count);
        }
    }
    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        playerIndex = pi.playerIndex;
        input = pi;
    }
    public PlayerInput input { get; set; }
    public int playerIndex { get; set; }
    public bool isReady { get; set; }
    public GameObject playerCharacter { get; set; }
}
