using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    private void Start()
    {
        AudioManager.instance.PlayMusic("Title Screen");
    }

    public void Play()
    {
        StartCoroutine(LoadNextScene(animator, SceneManager.GetActiveScene().buildIndex + 1));
    }
    public static IEnumerator LoadNextScene(Animator animator, int index)
    {
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(index);
    }
}
