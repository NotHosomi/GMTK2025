using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{


    float scrapClock = 0;
    float fishClock = 0;
    float enemyClock = 0;
    int enemyLimit = 6;

    // Start is called before the first frame update
    void Awake()
    {
        Enemy.s_Enemies = new List<Enemy>();
        Crate.s_crates = new List<Crate>();
        Fish.s_fish = new List<Fish>();
    }

    // Update is called once per frame
    void Update()
    {
        scrapClock -= Time.deltaTime;
        if(scrapClock < 0)
        {
            SpawnScrap();
            scrapClock = Random.Range(1.5f, 2.5f);
        }
        fishClock -= Time.deltaTime; ;
        if(fishClock < 0)
        {
            SpawnFish();
            fishClock = Random.Range(1f, 4f);
        }
        enemyClock -= Time.deltaTime; ;
        if(enemyClock < 0)
        {
            enemyClock = Random.Range(3.0f, 10.0f);
            if(Enemy.s_count < enemyLimit)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnScrap()
    {
        float x = Random.Range(-32, 32);
        float y = Random.Range(-24, 24);
        GameObject obj = GameObject.Instantiate(Resources.Load("Prefab/Scrap")) as GameObject;
        obj.transform.position = new Vector3(x, y, 0);
    }

    void SpawnFish()
    {
        const float val = 25.0f;
        float x = Random.Range(-32, 32);
        float y = Random.Range(-24, 24);
        Fish.NewFish(new Vector2(x, y));
    }

    void SpawnEnemy()
    {
        float x = 0;
        float y = 0;
        float rot = 0;
        int edge = Random.Range(0, 4);
        switch (edge)
        {
            case 0: // top
                y = (25 + 2);
                x = Random.Range(-32, 32);
                rot = 180;
                break;
            case 1: // bottom
                y = -(25 + 2);
                x = Random.Range(-32, 32);
                rot = 0;
                break;
            case 2: // left
                x = -(33.333f + 2);
                y = Random.Range(-25, 25);
                rot = 90;
                break;
            case 3: // right
                x = (33.333f + 2);
                y = Random.Range(-25, 25);
                rot = 270;
                break;
        }
        Enemy.NewEnemy(new Vector2(x, y), rot);
    }
}
