using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level");
        Debug.Log("Loading Level Success");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit successful");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }
}
