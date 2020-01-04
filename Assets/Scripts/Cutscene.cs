using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private string music = "";
    [SerializeField] private Animator animator = null;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMusic(music);
    }

    public void EndCutscene()
    {
        StartCoroutine(MainMenu.LoadNextScene(animator, SceneManager.GetActiveScene().buildIndex + 1));
    }
}
