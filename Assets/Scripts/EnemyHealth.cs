using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth enemyHealth;

    public GameObject Rupee;

    [Tooltip("DropChance out of 100")]
    public int DropChance;

    public float EnemyHP = 2;

    private void Start()
    {
        enemyHealth = this;
    }

    public void ReduceHealth(float amount)
    {
        EnemyHP -= amount;

        if (EnemyHP <= 0)
        {
            int r = Random.Range(1, 101);
            if (r <= DropChance)
            {
                Instantiate(Rupee, this.transform.position, Quaternion.Euler(-90, 0, 0));
            }

            Destroy(this.gameObject);
        }
    }
}
