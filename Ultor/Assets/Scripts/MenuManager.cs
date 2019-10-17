using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    string clickButtonSound = "ClickButton";

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audiomanager found!");
        }
    }

    public void StartGame()
    {
        audioManager.PlaySound(clickButtonSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
    }

    public void QuitGame()
    {
        audioManager.PlaySound(clickButtonSound);
        Application.Quit();
    }
}
