using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animtor;

    private void Start()
    {
        AudioManager.instance.PlayLoop("Title Screen");
    }

    public void Play()
    {
        animtor.SetTrigger("Fade");
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(animtor.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
