using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    public GameObject ShieldObject;

    public void OnRightMouseButtonUsed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShieldObject.gameObject.SetActive(true);
        }
        else if (context.canceled)
        {
            ShieldObject.gameObject.SetActive(false);
        }
    }
}
