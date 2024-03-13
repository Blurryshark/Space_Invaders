using System;

using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class AlienScript : MonoBehaviour
{
    public delegate void alienDied(String name);
    public static event alienDied onAlienDied;
    public GameObject alienMissile;
    public float missileForceMultiplier = 50f;

    private void Start()
    {
        if (name.Contains("0"))
        {
            GetComponent<Animator>().SetInteger("Row", 0);
        } else if (name.Contains("1"))
        {
            GetComponent<Animator>().SetInteger("Row", 1);
        } else if (name.Contains("2"))
        {            
            GetComponent<Animator>().SetInteger("Row", 2);
        } else if (name.Contains("3"))
        {
            GetComponent<Animator>().SetInteger("Row", 3);
        }
        InvokeRepeating("fire", 0.1f, 3f);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.name.Contains("Alien"))
        {
            Destroy(other.gameObject);
            this.GameObject().GetComponent<Animator>().SetTrigger("hit");
            Destroy(gameObject.GetComponent<Collider>());
            Debug.Log(other.name + " fired!");
            onAlienDied.Invoke(this.gameObject.name);
        }
        Debug.Log(other.name + " collided!");
    }

    private void fire()
    {
        int rand = Random.Range(0, 50);
        if (rand == 1)
        {
            GetComponent<Animator>().SetTrigger("shoot");
            Vector3 defenderPos = gameObject.GetComponent<Transform>().position;
            Vector3 spawnPos = new Vector3(defenderPos.x, defenderPos.y - 0.25f, defenderPos.z + 10f);
            GameObject obj = Instantiate(alienMissile, spawnPos, Quaternion.identity);
            Vector3 missileForce = Vector3.down * missileForceMultiplier;
            obj.GetComponent<Rigidbody>().AddForce(missileForce, ForceMode.Impulse);
            Debug.Log(obj.name + " fired!");

        }
    }
}
