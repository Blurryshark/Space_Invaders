using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeScript : MonoBehaviour
{
    public delegate void barricadeHit(String s);
    public static event barricadeHit onBarricadeHit;
    private void OnTriggerEnter(Collider other)
    {
        onBarricadeHit.Invoke(other.name);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
