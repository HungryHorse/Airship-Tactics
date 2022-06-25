using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractUnit : MonoBehaviour, IHealth, IDamageDealer
{
    public float Damage { get; set; }

    public float Health { get; private set; }

    public abstract UnitClasses Class { get; }

    public void DealDamage(IHealth target)
    {
        target.TakeDamage(Damage);
    }

    protected void Die()
    {
        Debug.Log($"{Class} {gameObject.name} has died");
    }

    public void Heal(float heal)
    {
        Health += heal;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
}
