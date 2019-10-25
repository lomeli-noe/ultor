using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    string clickButtonSound = "ClickButton";

    [SerializeField]
    string drumTransitionSound = "DrumTransition";

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
        audioManager.PlaySound(drumTransitionSound);
        audioManager.StopSound("intro song");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
    }

    public void QuitGame()
    {
        audioManager.PlaySound(clickButtonSound);
        Application.Quit();
    }
}
