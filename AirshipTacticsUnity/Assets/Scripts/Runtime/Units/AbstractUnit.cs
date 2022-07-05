using System.Collections;
using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;
using Zenject;

public abstract class AbstractUnit : MonoBehaviour, IHealth, IDamageDealer
{
    public readonly Vector3 Offset = Vector3.up * 2f;

    public float Damage { get; set; }

    public float Health { get; private set; }

    public abstract UnitClasses Class { get; }

    private Flow MovementAnimation { get; set; }

    private MapController MapController { get; set; }

    public MapTile CurrentLocation { get; set; }

    [Inject]
    public void Construct(MapController mapController)
    {
        MapController = mapController;
    }

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

    public void MoveToTile(MapTile target)
    {
        MovementAnimation?.Stop();
        MovementAnimation = new Flow();

        List<MapTile> pathfinding = PathFinding.AStar(MapController.MapTiles, CurrentLocation, target);

        foreach (MapTile mapTile in pathfinding)
        {
            MovementAnimation.Queue(new Tween(0.5f)
                .For(transform)
                    .MoveTo(mapTile.transform.position + Offset)
                .OnCompleted(() => CurrentLocation = mapTile)
            );
        }

        MovementAnimation.Start();
    }
}
