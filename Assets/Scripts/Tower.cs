using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] float timeAttack;
    [SerializeField] float attackRadius;
    [SerializeField] private Projectile projectile;
    private Enemy target = null;
    float attackCounter;
    bool isAttacking = false;

    private void Update()
    {
        attackCounter -= Time.deltaTime;
        if (target == null || target.IsDead)
        {
            Enemy nearestEnemy = GetNearestEnemyInRange();
            if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
            {
                target = nearestEnemy;
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttacking = true;
                attackCounter = timeAttack;
            }
            else
            {
                isAttacking = false;
            }
            if (Vector2.Distance(transform.localPosition, target.transform.localPosition) > attackRadius)
            {
                target = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttacking = false;
        Projectile newprojectile = Instantiate(projectile) as Projectile;
        newprojectile.transform.localPosition = transform.localPosition;
        if(newprojectile.ProjectileType == ProType.arrow)
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
        }
        else if(newprojectile.ProjectileType == ProType.fireball)
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Fireball);
        }
        else if (newprojectile.ProjectileType == ProType.rock)
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
        }
        if (target == null)
        {
            Destroy(newprojectile);
        }
        else
        {
            StartCoroutine(MoveProjectile(newprojectile));
        }
    }

    IEnumerator MoveProjectile(Projectile projectileMove)
    {
        while(getTargetDistance(target) > 0.01f && projectileMove != null && target != null)
        {
            var dir = target.transform.localPosition - transform.localPosition;
            var angleDir = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectileMove.transform.rotation = Quaternion.AngleAxis(angleDir, Vector3.forward);
            projectileMove.transform.localPosition = Vector2.MoveTowards(projectileMove.transform.localPosition, target.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if(projectileMove != null || target == null)
        {
            Destroy(projectileMove);
        }
    }

    float getTargetDistance(Enemy enemy)
    {
        if(enemy == null)
        {
            enemy = GetNearestEnemyInRange();
            if(enemy == null)
            {
                return 0;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));
    }

    List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in GameManager.Instance.EnemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    Enemy GetNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach(Enemy enemy in GetEnemiesInRange())
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}
