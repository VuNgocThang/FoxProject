using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladders : MonoBehaviour
{
    private enum LadderPart { complete, bottom, top };

    [SerializeField] private LadderPart part = LadderPart.complete;
    private List<GameObject> list = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canClimb = true;
                    player.ladders = this;
                    break;
                case LadderPart.bottom:
                    player.bottomLadders = true;
                    break;
                case LadderPart.top:
                    player.topLadders = true;
                    break;
                default:
                    break;
            }
        }*/
    }
 /*   private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canClimb = false;
                    break;
                case LadderPart.bottom:
                    player.bottomLadders = false;
                    break;
                case LadderPart.top:
                    player.topLadders = false;
                    break;
                default:
                    break;
            }
        }
    }*/

}
