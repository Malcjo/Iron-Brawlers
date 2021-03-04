using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
        OldGameManager.instance.FindPlayers();
        Destroy(this);
    }
}
