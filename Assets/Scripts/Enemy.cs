using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> s_Enemies;
    public static int s_count = 0;

    public float turnSpeed = 180f;
    public float targetSpeed;
    public float lerpRate = 5f;
    public float speedHi = 10f;
    public float speedBase = 6f;
    public float speedLo = 4f;
    private Rigidbody2D rb;
    private float currentSpeed;
    private float turnInput;
    private float speedInput;

    Transform player;
    float heading;
    float moodClock = 0;
    float meanderClock = 0;
    bool turning = false; // meandering only
    enum E_Mood { attacking, meandering, fleeing };
    E_Mood mood = E_Mood.attacking;
    public static Enemy NewEnemy(Vector2 pos, float rot)
    {
        Debug.Log("New enemy");
        Enemy enemy = (GameObject.Instantiate(Resources.Load("Prefab/Enemy")) as GameObject).GetComponent<Enemy>();
        enemy.transform.position = pos;
        enemy.player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy.rb = enemy.GetComponent<Rigidbody2D>();
        enemy.rb.SetRotation(rot);
        return enemy;
    }

    // Start is called before the first frame update
    void Start()
    {
        s_Enemies.Add(this);
        ++s_count;
    }

    private void OnDestroy()
    {
        s_Enemies.Remove(this);
        --s_count;
    }

    // Update is called once per frame
    void Update()
    {
        moodClock -= Time.deltaTime;
        if(moodClock < 0)
        {
            switch(mood)
            {
                case E_Mood.attacking:
                    mood = E_Mood.fleeing;
                    targetSpeed = speedHi;
                    moodClock = Random.Range(3.0f, 5.0f);
                    break;
                case E_Mood.meandering:
                case E_Mood.fleeing:
                    int opt = Random.Range(0, 4);
                    if (opt == 0)
                    {
                        mood = E_Mood.attacking;
                        moodClock = Random.Range(5.0f, 8.0f);
                    }
                    else
                    {
                        mood = E_Mood.meandering;
                        moodClock = Random.Range(3.0f, 5.0f);
                    }
                    if(mood == E_Mood.meandering)
                    {
                        targetSpeed = speedLo;
                    }
                    else
                    {
                        targetSpeed = speedHi;
                    }
                    break;
            }
        }
        switch(mood)
        {
            case E_Mood.attacking:
                Attack();
                break;
            case E_Mood.fleeing:
                Flee();
                break;
            case E_Mood.meandering:
                Meander();
                break;
        }
    }

    void PickMood(List<E_Mood> options)
    {
        mood = options[Random.Range(0, options.Count)];
    }

    void Attack()
    {
        // in range?
        if((player.position - transform.position).magnitude < 4)
        {
            targetSpeed = speedBase;
            TurnAway();
        }
        else
        {
            float angle = AngleToPlayer();
            if (angle < 0.5 || angle > (2 * 3.145) - 0.5)
            {
                targetSpeed = speedHi;
            }
            else if (angle > (3.145 / 2) && angle < (1.5 * 3.145)) ;
            else
            {
                targetSpeed = speedLo;
            }
            TurnToward();
        }
    }

    void Flee()
    {
        TurnAway();
        if ((player.position - transform.position).magnitude < 16)
        {
            targetSpeed = speedBase;
        }
        else
        {
            targetSpeed = speedLo;
        }
    }

    void Meander()
    {
        meanderClock -= Time.deltaTime;
        if(meanderClock < 0)
        {
            turnInput = Random.Range(-1.0f, 1.0f);
            meanderClock = Random.Range(0.1f, 0.8f);
        }
    }

    void TurnToward()
    {
        if (AngleToPlayer() > 3.142)
        {
            turnInput = -1;
        }
        else
        {
            turnInput = 1;
        }
    }

    void TurnAway()
    {
        if (AngleToPlayer() > 3.142)
        {
            turnInput = 1;
        }
        else
        {
            turnInput = -1;
        }
    }

    void FixedUpdate()
    {
        float rotationAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpRate * Time.fixedDeltaTime);

        // todo: do a series of rays in front, if they touch the net, turn

        rb.velocity = transform.up * currentSpeed;
    }

    float AngleToPlayer()
    {
        Vector2 heading = transform.up;
        Vector2 delta = player.position - transform.position;
        return Vector2.Angle(heading, delta);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, 4*transform.up + transform.position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position);
    }
}
