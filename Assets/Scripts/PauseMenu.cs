using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public static bool isPaused;
    [SerializeField] private GameObject background = null;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject credits = null;
    [SerializeField] private GameObject help = null;
    [SerializeField] private Animator animator = null;
    private void Start()
    {
        AudioManager.instance.PlayMusic("Music Forest Theme");

        background.SetActive(false);
        pauseMenu.SetActive(false);
        credits.SetActive(false);
        help.SetActive(false);
        isPaused = false;
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
        credits.SetActive(false);
        help.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        background.SetActive(true);
        pauseMenu.SetActive(true);
        credits.SetActive(false);
        help.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Help()
    {
        credits.SetActive(false);
        help.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Credits()
    {
        credits.SetActive(true);
        help.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void Back()
    {
        credits.SetActive(false);
        pauseMenu.SetActive(true);
        help.SetActive(false);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        StartCoroutine(MainMenu.LoadNextScene(animator, 0));
    }
}
