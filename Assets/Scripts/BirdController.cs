using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    public GameObject Player;

    public Vector3 LeaveOffset;

    public void SitOnBird()
    {
        FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("BirdController");
        Player.GetComponent<CharacterController>().enabled = false;

        Player.transform.position = this.transform.position;
        Player.transform.rotation = this.transform.rotation;

    }

    private void Update()
    {
        //Player.transform.position = this.transform.position;
    }

    public void LeaveBird()
    {
        Player.transform.position = this.transform.position + LeaveOffset;

        FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("Player");
        Player.GetComponent<CharacterController>().enabled = true;
    }
}
