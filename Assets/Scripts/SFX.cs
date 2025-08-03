using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX _i;
    // Start is called before the first frame update
    void Start()
    {
        _i = this;
        m_oSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum E_Sfx
    {
        good,
        verygood,
        bad,
        womp,
        crunch,
        splash
    }
    [SerializeField] AudioClip m_oGood;
    [SerializeField] AudioClip m_oVeryGood;
    [SerializeField] AudioClip m_oBad;
    [SerializeField] AudioClip m_oWompWomp;
    [SerializeField] AudioClip m_oCrunch;
    [SerializeField] AudioClip[] m_aSplash;
    AudioSource m_oSfx;

    public void PlaySound(E_Sfx sound)
    {
        switch (sound)
        {
            case E_Sfx.good:
                m_oSfx.PlayOneShot(m_oGood);
                break;
            case E_Sfx.verygood:
                m_oSfx.PlayOneShot(m_oVeryGood);
                break;
            case E_Sfx.bad:
                m_oSfx.PlayOneShot(m_oBad);
                break;
            case E_Sfx.womp:
                m_oSfx.PlayOneShot(m_oWompWomp);
                break;
            case E_Sfx.crunch:
                m_oSfx.PlayOneShot(m_oCrunch);
                break;
            case E_Sfx.splash:
                m_oSfx.PlayOneShot(m_aSplash[Random.Range(0, m_aSplash.Length)]);
                break;
        }
    }
}
