using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatVFX : MonoBehaviour
{
    [SerializeField] List<Sprite> hullSheet;
    [SerializeField] List<Sprite> sailSheet;
    [SerializeField] List<Vector2> offset;

    SpriteRenderer hullSR = null;
    SpriteRenderer sailSR = null;

    enum E_CompassDir
    {
        N, NE, E, SE, S, SW, W, NW
    }

    private void Start()
    {
        hullSR = GetComponent<SpriteRenderer>();
        if(sailSheet != null && transform.childCount > 0)
        {
            sailSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        SetSprite();
    }


    // Update is called once per frame
    void Update()
    {
        SetSprite();
    }

    void SetSprite()
    {
        float rot = transform.parent.rotation.eulerAngles.z;
        float increment = 45;
        rot += increment / 2;
        int dir = (int)(rot / increment) % 8;
        Debug.Log(dir);
        hullSR.sprite = hullSheet[dir];
        if(sailSR != null)
        {
            sailSR.sprite = sailSheet[dir];
        }
        float z = (rot % increment) - increment / 2;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }
}
