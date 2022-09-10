using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float RotateSpeed = 50;

    public Vector3 RotateAxis = Vector3.up;
    void Update()
    {
        if (!PauzeMenu.pauzeMenu.IsGamePauzed)
        {
            this.transform.Rotate(RotateAxis, RotateSpeed * Time.deltaTime);
        }
    }
}
