using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefenderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        GetComponent<Animator>().SetTrigger("explodeTrigger");
        StartCoroutine("credits");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("ShootTrigger");
        }
        
    }

    IEnumerator credits()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Credits");
    }
}
