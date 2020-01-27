using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string scene;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MainMenu.LoadNextScene(GameObject.FindWithTag("Fade to Black").GetComponent<Animator>(), SceneManager.GetSceneByName(scene).buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MainMenu.LoadNextScene(GameObject.FindWithTag("Fade to Black").GetComponent<Animator>(), SceneManager.GetSceneByName(scene).buildIndex);
        }
    }
}
