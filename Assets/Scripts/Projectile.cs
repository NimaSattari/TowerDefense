using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProType
{
    rock , arrow, fireball
};

public class Projectile : MonoBehaviour
{
    [SerializeField] int AttackStrengh;
    [SerializeField] ProType projectileType;

    public int AttackStrength
    {
        get
        {
            return AttackStrengh;
        }
    }

    public ProType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
