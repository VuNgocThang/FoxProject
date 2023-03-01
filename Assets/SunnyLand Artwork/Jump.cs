using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Jump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController PlayerController;
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerController.JumpOrClimb();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }
}

