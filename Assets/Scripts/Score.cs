using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score _i;
    [SerializeField] TextMeshProUGUI display;

    int score = 0;
    int fish = 0;
    int ships = 0;
    bool lockScore = false;
    // Start is called before the first frame update
    void Start()
    {
        if(_i != null)
        {
            Destroy(_i);
        }
        _i = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(float points, bool isEnemy)
    {
        if(lockScore) { return; }

        if(isEnemy)
        {
            ++ships;
        }
        else
        {
            ++fish;
        }
        score += (int)points;
        display.SetText(score.ToString());
    }

    public void OnGameover()
    {
        lockScore = true;
        GameObject.Find("FishVal").GetComponent<TextMeshPro>().SetText(fish.ToString());
        GameObject.Find("ShipsVal").GetComponent<TextMeshPro>().SetText(fish.ToString());
    }
}
