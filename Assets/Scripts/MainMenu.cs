using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public AudioSource music;
    // Start is called before the first frame update
    public void PlayGame()
    {
        StartCoroutine(LoadLevel());   
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

   IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");
        float t = 1;
        while (t >= 0)
        {
            t -= Time.deltaTime;
            music.volume = t;
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
