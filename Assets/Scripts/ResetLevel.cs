﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(0, 10, 0);
        other.gameObject.GetComponent<Player>().lives--;
    }
}
