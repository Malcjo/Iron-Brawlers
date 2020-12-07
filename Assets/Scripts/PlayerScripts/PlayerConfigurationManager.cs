using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

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

    public void SetPlayerSkinMaterial(int Index, Material material)
    {
        playerConfigs[Index].playerMaterial = material;
    }

    public void ReadyPlayer(int Index)
    {
        playerConfigs[Index].isReady = true;
        if(playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true))
        {
            //Load scene
        }
    }
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player joined " + pi.playerIndex);
        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
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
    public Material playerMaterial { get; set; }
}
