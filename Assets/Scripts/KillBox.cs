using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public delegate void missileMiss(String s);
    public static event missileMiss onMissileMiss;
    private void OnTriggerEnter(Collider other)
    {
        onMissileMiss.Invoke(other.name);
        Destroy(other.gameObject);
    }
}
