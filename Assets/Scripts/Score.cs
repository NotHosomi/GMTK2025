using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score _i;
    [SerializeField] TextMeshProUGUI display;

    int score;
    // Start is called before the first frame update
    void Start()
    {
        if(_i == null)
        {
            _i = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(float points)
    {
        score += (int)points;
        display.SetText(score.ToString());
    }
}
