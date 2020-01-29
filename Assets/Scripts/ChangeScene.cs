using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [HideInInspector]
    public enum Scenes
    {
        Silkan = 3,
    };

    [SerializeField] private Scenes scene = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(MainMenu.LoadNextScene(GameObject.FindWithTag("Fade to Black").GetComponent<Animator>(), (int)scene));
        }
    }
}
