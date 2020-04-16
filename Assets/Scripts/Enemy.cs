using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int target = 0;
    [SerializeField] Transform exitPoint;
    [SerializeField] GameObject[] waypoints;
    [SerializeField] float navUpdate;
    private Transform enemy;
    Collider2D collider2D;
    private float navTime = 0;
    [SerializeField] int HealthPoint;
    bool isDead = false;
    Animator anim;
    [SerializeField] int rewardAmt;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    void Start()
    {
        enemy = GetComponent<Transform>();
        collider2D = GetComponent<Collider2D>();
        waypoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        GameManager.Instance.RegisterEnemy(this);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(waypoints != null && !isDead)
        {
            navTime += Time.deltaTime;
            if(navTime > navUpdate)
            {
                if (target < waypoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].transform.position, navTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navTime);
                }
                navTime = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CheckPoint")
        {
            target++;
        }
        else if(collision.tag == "Finish")
        {
            GameManager.Instance.RoundEscaped++;
            GameManager.Instance.TotalEscaped++;
            GameManager.Instance.UnRegisterEnemy(this);
            GameManager.Instance.isWaveOver();
        }
        else if(collision.tag == "Projectile")
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackStrength);
            Destroy(collision.gameObject);
        }
    }
    public void EnemyHit(int hitpoints)
    {
        if(HealthPoint - hitpoints > 0)
        {
            HealthPoint -= hitpoints;
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            anim.Play("E1Hurt");
        }
        else
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
            anim.SetTrigger("Die");
            Die();
        }
    }
    public void Die()
    {
        isDead = true;
        collider2D.enabled = false;
        GameManager.Instance.TotalKilled++;
        GameManager.Instance.AddMoney(rewardAmt);
        GameManager.Instance.isWaveOver();
    }
}
