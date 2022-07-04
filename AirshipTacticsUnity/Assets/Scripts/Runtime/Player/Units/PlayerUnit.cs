using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerUnit : MonoBehaviour, ISelectable
{
    private bool IsHovering { get; set; }

    public AbstractUnit BaseUnit { get; set; }
    private PlayerUnitController UnitController { get; set; }

    [Inject]
    public void Construct(PlayerUnitController unitController)
    {
        UnitController = unitController;
    }

    private void OnMouseEnter()
    {
        IsHovering = true;
    }

    private void OnMouseExit()
    {
        IsHovering = false;
    }

    private void Update()
    {
        if (IsHovering && Input.GetButtonUp("Fire1"))
        {
            Select();
        }
    }

    public void Select()
    {
        UnitController.Select(this);
    }
}
