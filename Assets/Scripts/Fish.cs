using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public static Fish NewFish(Vector2 pos)
    {
        Fish fish = (GameObject.Instantiate(Resources.Load("Prefab/Fish")) as GameObject).GetComponent<Fish>();
        return fish;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
