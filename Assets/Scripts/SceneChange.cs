using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            PlayerPrefs.SetInt("cherries", PermanentUI.perm.cherries);
            PlayerPrefs.SetInt("hearts", PermanentUI.perm.hearts);

           SceneManager.LoadScene(sceneName);
        }
    }
}
