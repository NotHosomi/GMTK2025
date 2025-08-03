using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public static List<Fish> s_fish;
    SpriteRenderer sr;
    public bool isNetted = false;
    public static Fish NewFish(Vector2 pos)
    {
        Fish fish = (GameObject.Instantiate(Resources.Load("Prefab/Fish")) as GameObject).GetComponent<Fish>();
        fish.transform.position = pos;
        float s = 0.6f + Random.Range(0, 0.4f) + Random.Range(0, 0.4f);
        fish.transform.localScale = new Vector3(s, s, 1);
        return fish;
    }
    private void Awake()
    {
        s_fish.Add(this);
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        s_fish.Remove(this);
    }

    private void Update()
    {
        Animate();
    }

    [SerializeField] List<Sprite> frames;
    const float frameTime = 1.0f / 8.0f;
    float animClock = frameTime;
    int frameIdx = 3;
    void Animate()
    {
        animClock -= Time.deltaTime;
        if(animClock < 0)
        {
            animClock += frameTime;
            frameIdx = (frameIdx + 1);
            if(frameIdx == frames.Count)
            {
                if (Random.Range(0,100) < 2.5f && !isNetted)
                {
                    Destroy(gameObject);
                }
                frameIdx = 0;
            }
            sr.sprite = frames[frameIdx];
        }
    }
}
