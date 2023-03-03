using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClimbDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController PlayerController;
    bool isPressed = false;
    private void Update()
    {
        if (isPressed)
        {
            PlayerController.ClimbDown();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}


