using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;

public abstract class AbstractUnit : MonoBehaviour, IHealth, IDamageDealer
{
    public readonly Vector3 Offset = Vector3.up * 2f;

    public float Damage { get; set; }

    public float Health { get; private set; }

    public abstract UnitClasses Class { get; }

    private AbstractAnimation MovementAnimation { get; set; }

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

    public void MoveToTile(MapTile mapTile)
    {
        MovementAnimation?.Stop();
        MovementAnimation = new Tween(0.5f)
            .SetEasing(Easing.EaseInOutSine)
            .For(transform)
                .MoveTo(mapTile.transform.position + Offset)
            .Start();
    }
}
