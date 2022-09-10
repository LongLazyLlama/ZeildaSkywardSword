using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauzeMenu : MonoBehaviour
{
    public static PauzeMenu pauzeMenu;

    public GameObject Menu;
    public GameObject MenuBackground;

    [HideInInspector]
    public bool IsGamePauzed;

    public void Awake()
    {
        pauzeMenu = this;
    }

    public void OnPauze(InputAction.CallbackContext context)
    {
        if (context.started && !Menu.activeSelf)
        {
            SetActiveState(true, false, "UI");
            IsGamePauzed = true;
        }
        else if (context.started && Menu.activeSelf)
        {
            SetActiveState(false, true, "Player");
            IsGamePauzed = false;
        }
    }

    private void SetActiveState(bool state, bool disableAll, string actionmap)
    {
        Cursor.visible = state;

        FindObjectOfType<PlayerController>().enabled = !state;
        FindObjectOfType<CharacterController>().enabled = !state;

        Menu.SetActive(state);
        MenuBackground.SetActive(state);

        if (disableAll)
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        FindObjectOfType<PlayerInput>().SwitchCurrentActionMap(actionmap);
    }

    public void Continue()
    {
        SetActiveState(false, false, "Player");
    }

    public void Quit()
    {
        Debug.Log("Closing game application.");
        Application.Quit();
    }
}
