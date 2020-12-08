using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private bool usingMenu;
    void Start()
    {
        if(usingMenu == true)
        {
            for (int i = 0; i < GameManager.instance.playerCharacters.Length; i++)
            {
                playerPrefab[i] = GameManager.instance.playerCharacters[i];
            }
        }

        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for(int i = 0; i< playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab[i], playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponentInChildren<PlayerInputDetection>().InitializePlayer(playerConfigs[i]);

            switch (i)
            {
                case 0: player.GetComponentInChildren<Player>().playerNumber = Player.PlayerIndex.Player1;
                    break;
                case 1: player.GetComponentInChildren<Player>().playerNumber = Player.PlayerIndex.Player2;
                    break;
            }
        }
    }

    void Update()
    {
        
    }
}
