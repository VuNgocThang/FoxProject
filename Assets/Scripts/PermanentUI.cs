using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUI : MonoBehaviour
{
    public int cherries = 0;
    public Text cherryText;
    public int hearts = 5;
    public Text heartsText;

    public static PermanentUI perm;
    private void Awake()
    {
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        cherries = PlayerPrefs.GetInt("cherries",0);
        hearts = PlayerPrefs.GetInt("hearts",5);
        cherryText.text=cherries.ToString();
        heartsText.text=hearts.ToString();
    }


}
