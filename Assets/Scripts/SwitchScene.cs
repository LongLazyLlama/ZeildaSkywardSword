using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class SwitchScene : MonoBehaviour
{
    public Animator Animator;

    public int LevelToLoad;
    public bool ChangeSceneAutomaticly;
    public float ChangeTime;

    private float _timer;

    private void Update()
    {
        AutomaticlyChangeScene();
    }

    private void AutomaticlyChangeScene()
    {
        if (ChangeSceneAutomaticly)
        {
            _timer += Time.deltaTime;

            if (_timer >= ChangeTime)
            {
                FadeToLevel();

                Cursor.visible = true;
            }
        }
    }

    public void FadeToLevel()
    {
        Animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        //Changes scene after the fade is complete.
        SceneManager.LoadScene(LevelToLoad);
    }

    public void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        //When the player is in the main menu any button can be used for this code.
        if (SceneManager.GetActiveScene().name == "Start_Screen" && context.started)
        {
            Cursor.visible = false;

            FadeToLevel();
        }
    }
}
