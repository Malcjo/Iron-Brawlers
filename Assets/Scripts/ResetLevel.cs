using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = new Vector3(0, 10, 0);
    }
}
