﻿using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class boss1_script : MonoBehaviour
{
    public static boss1_script Instance { get; set; }
    
    private float min = -9f;
    private float max = 9f;
    private Transform topSpawn, bottomSpawn;
    private int range;

    public float speed = 3f;
    public int bossLife = 5;
    public Transform[] enemy;
    public Transform bomb;
    public Transform boss;
    public float spawnTime = 2f;

    private void Awake()
    {
        topSpawn = GameObject.FindGameObjectWithTag("spawnTop").transform;
        bottomSpawn = GameObject.FindGameObjectWithTag("spawnBottom").transform;
        Physics.IgnoreLayerCollision(12, 9);
        GetComponent<Renderer>().material.color = Color.green;
    }

    void Start()
    {
        transform.position = new Vector3(0, 1.5f, -10);
        StartCoroutine(spawnEnemy());
        StartCoroutine(spawnAdd());
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.PingPong(Time.time * speed, max - min) + min, transform.position.y, transform.position.z);
        range = Random.Range(8, 18);
        switch (bossLife)
        {
            case 3:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 1:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            default:
                break;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            bossLife--;
            Debug.Log(bossLife);
        }

        if (bossLife <= 0)
        {
            Destroy(gameObject);
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

            for (int k = 0; k < bombs.Length; k++)
            {
                Destroy(bombs[k]);
            }

            FindObjectOfType<SceneManagerScript>().Invoke("GoToMainMenu", 5f);
        }

    }

    IEnumerator spawnEnemy()
    {
        int tempcount = -1;
        while(true)
        {
            if (tempcount >= range)
            {
                Instantiate(bomb, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.Euler(0, 90, 0));
                tempcount = 0;
            }
            else
            {
                Instantiate(enemy[0], new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.Euler(0, 90, 0));
                tempcount++;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // too lazy make this modular lol
    IEnumerator spawnAdd()
    {
        while (true)
        {
            switch (bossLife)
            {
                case 5:
                case 4:
                    Instantiate(enemy[0], RoundManager.RandomPointInBounds(topSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, 0, 0));
                    Instantiate(enemy[0], RoundManager.RandomPointInBounds(bottomSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, -180, 0));
                    Instantiate(enemy[0], RoundManager.RandomPointInBounds(topSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, 0, 0));
                    Instantiate(enemy[0], RoundManager.RandomPointInBounds(bottomSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, -180, 0));
                    break;
                case 3:
                case 2:
                    spawnTime = 1.5f;
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(topSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, 0, 0));
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(bottomSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, -180, 0));
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(topSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, 0, 0));
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(bottomSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, -180, 0));
                    break;
                case 1:
                    spawnTime = 1f;
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(topSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, 0, 0));
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(bottomSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, -180, 0));
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(topSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, 0, 0));
                    Instantiate(enemy[Random.Range(0, 2)], RoundManager.RandomPointInBounds(bottomSpawn.GetComponent<Collider>().bounds), Quaternion.Euler(0, -180, 0));
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
