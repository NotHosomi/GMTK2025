using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    float scrapClock = 0;
    float fishClock = 0;
    float enemyClock = 0;
    int enemyLimit = 5;
    int fishLimit = 13;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scrapClock -= Time.deltaTime;
        if(scrapClock < 0)
        {
            SpawnScrap();
            scrapClock = Random.Range(0.5f, 2.5f);
        }
        fishClock -= Time.deltaTime; ;
        if(fishClock < 0)
        {

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
        const float val = 25.0f;
        float x = Random.Range(-val, val);
        float y = Random.Range(-val, val);
        GameObject obj = GameObject.Instantiate(Resources.Load("Prefab/Scrap")) as GameObject;
        obj.transform.position = new Vector3(x, y, 0);
    }

    void SpawnFish()
    {
        const float val = 25.0f;
        float x = Random.Range(-val, val);
        float y = Random.Range(-val, val);
        Fish.NewFish(new Vector2(x, y));
    }

    void SpawnEnemy()
    {
        const float val = 25.0f;
        float x = 0;
        float y = 0;
        float rot = 0;
        int edge = Random.Range(0, 4);
        switch (edge)
        {
            case 0: // top
                y = (val + 2);
                x = Random.Range(-val, val);
                rot = 180;
                break;
            case 1: // bottom
                y = -(val + 2);
                x = Random.Range(-val, val);
                rot = 0;
                break;
            case 2: // left
                x = -(val + 2);
                y = Random.Range(-val, val);
                rot = 90;
                break;
            case 3: // right
                x = (val + 2);
                y = Random.Range(-val, val);
                rot = 270;
                break;
        }
        Enemy.NewEnemy(new Vector2(x, y), rot);
    }
}
