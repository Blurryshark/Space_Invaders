using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox_1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
