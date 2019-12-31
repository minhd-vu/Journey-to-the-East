using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public static bool isPaused = false;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private Animator animator;
    

    private void Start()
    {
        background.SetActive(false);
        pauseMenu.SetActive(false);
        credits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        background.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        background.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Help()
    {

    }

    public void Credits()
    {
        credits.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Back()
    {
        credits.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        StartCoroutine(MainMenu.LoadNextScene(animator, 0));
    }
}
