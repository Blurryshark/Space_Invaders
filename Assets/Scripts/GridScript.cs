using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject defender;
    public GameObject barricade;
    public GameObject missile;
    public float missileForceMultipler = 50f;
    public bool missileIs = false;
    public int enemyCount;
    private Transform t;
    public float spacingFactorX = 1f;
    public float spacingFactorY = 1f;
    public Material greenMat;
    public Material redMat;
    public Material blueMat;
    public Material pinkMat;
    public float increment = 0.75f;
    private bool goingRight;
    private float startingX = 0.5f;
    private float startingY = 3.5f;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI highScoreText;

    public int playerScore;
    public int highScore;
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        playerScore = 0;
        highScore = PlayerPrefs.GetInt("highscore");
        playerScoreText.SetText("00000");
        
        AlienScript.onAlienDied += AlienScriptOnonAlienDied;
        KillBox.onMissileMiss += KillBoxOnonMissileMiss;
        BarricadeScript.onBarricadeHit += BarricadeScriptOnonBarricadeHit;

        void BarricadeScriptOnonBarricadeHit(String name)
        {
            if (!name.Contains("Alien"))
                missileIs = false;
        }

        void KillBoxOnonMissileMiss(String name)
        {
            if (!name.Contains("Alien"))
                missileIs = false;
        }

        void AlienScriptOnonAlienDied(String name)
        {
            missileIs = false;
            enemyCount--;
            if (enemyCount == 50)
            {
                CancelInvoke();
                InvokeRepeating("moveGrid", .01f, 0.50f);
            }
            if (enemyCount == 30)
            {
                CancelInvoke();
                InvokeRepeating("moveGrid", .01f, 0.25f);
            }

            if (name.Contains("0"))
            {
                playerScore += 10;
            } else if (name.Contains("1"))
            {
                playerScore += 50;
            } else if (name.Contains("2"))
            {
                playerScore += 100;
            } else if (name.Contains("3"))
            {
                playerScore += 200;
            }
        }

        goingRight = true;
        t = GetComponent<Transform>();
        t.position = new Vector3(startingX, startingY, -10f);
        Vector3 currentPos = t.position;
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                Vector3 SpawnPos = new Vector3(currentPos.x + (k * spacingFactorX), currentPos.y + (i * spacingFactorY), currentPos.z);
                GameObject obj = Instantiate(myPrefab, SpawnPos, Quaternion.identity, t);
                //colorPicker(i, obj);
                obj.GameObject().name = i + "_alien";
                enemyCount++;
                SpawnPos = new Vector3(currentPos.x + (k * spacingFactorX * -1), currentPos.y + (i *spacingFactorY), currentPos.z);
                obj = Instantiate(myPrefab, SpawnPos, Quaternion.identity, t);
                //colorPicker(i, obj);
                obj.GameObject().name = i + "_alien";
                
                enemyCount++;
            }
        }

        Instantiate(barricade, new Vector3(7.5f, -2, 0), Quaternion.identity);
        Instantiate(barricade, new Vector3(2.75f, -2,0), Quaternion.identity);
        Instantiate(barricade, new Vector3(-1.75F, -2, 0), Quaternion.identity);
        Instantiate(barricade, new Vector3(-6.25f, -2, 0), Quaternion.identity);
        InvokeRepeating("moveGrid", 1f, 0.75f);
    }

    private void Update()
    {
        float velocity = Input.GetAxis("Horizontal");
        Vector3 force = Vector3.left * velocity;
        defender.GameObject().GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("ley released");
            defender.GameObject().GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (!missileIs)
            {
                Vector3 defenderPos = defender.GetComponent<Transform>().position;
                Vector3 spawnPos = new Vector3(defenderPos.x, defenderPos.y + 0.25f, defenderPos.z - 10f);
                GameObject obj = Instantiate(missile, spawnPos, Quaternion.identity);
                Vector3 missileForce = Vector3.up * missileForceMultipler;
                obj.GetComponent<Rigidbody>().AddForce(missileForce, ForceMode.Impulse);
                missileIs = true;
            }
        }

        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("RESET");
        }

        String newScore = scoreFormat(playerScore);
        if (playerScore > highScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetInt("highscore", highScore);
        }

        String newHighScore = scoreFormat(highScore);
        playerScoreText.SetText(newScore);
        highScoreText.SetText(newHighScore);
        
    }

    public void moveGrid()
    {
        Vector3 currentPos = t.position;
        Vector3 newPos;
        if (goingRight)
        {
            if (currentPos.x <= -4.5f)
            {
                goingRight = false;
                newPos = new Vector3(currentPos.x, currentPos.y - increment, currentPos.z);
                t.position = newPos;
                return;
            }
            newPos = new Vector3(currentPos.x - increment, currentPos.y, currentPos.z);
            t.position = newPos;
        }
        else
        {
            if (currentPos.x >= 4.5f)
            {
                goingRight = true;
                newPos = new Vector3(currentPos.x, currentPos.y - increment, currentPos.z);
                t.position = newPos;
                return;
            }
            newPos = new Vector3(currentPos.x + increment, currentPos.y, currentPos.z);
            t.position = newPos;
        }
    }

    // public void colorPicker(int i, GameObject obj)
    // {
    //     if (i == 0)
    //     { 
    //         obj.GetComponent<Renderer>().material = blueMat;
    //     } else if (i == 1) {
    //         obj.GetComponent<Renderer>().material = greenMat;
    //     } else if (i == 2) {
    //         obj.GetComponent<Renderer>().material = redMat;
    //     } else if (i == 3) {
    //         obj.GetComponent<Renderer>().material = pinkMat;
    //     }
    // }

    public String scoreFormat(int score)
    {
        String newScore;
        if (score == 0)
        {
            newScore = "00000";
        } else if (score >= 1 & score < 10)
        {
            newScore = "0000" + score.ToString();
        } else if (score >= 10 & score < 100)
        {
            newScore = "000" + score.ToString();
        } else if (score >= 100 & score < 1000)
        {
            newScore = "00" + score.ToString();
        } else if (score >= 1000 & score < 10000)
        {
            newScore = "0" + score.ToString();
        } else if (score >= 10000 & score < 99999)
        {
            newScore = score.ToString();
        }
        else
        {
            newScore = "99999";
        }

        return newScore;
    }


}
