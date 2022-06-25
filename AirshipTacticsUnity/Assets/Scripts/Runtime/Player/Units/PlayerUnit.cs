using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour, ISelectable
{
    private bool IsHovering { get; set; }

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
        if (IsHovering && Input.GetButton("Fire1"))
        {
            Select();
        }
    }

    public void Select()
    {
        Debug.Log("Unit selected by player");
    }
}
