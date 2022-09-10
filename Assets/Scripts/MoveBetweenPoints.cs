using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public float Speed;
    public Vector3 Direction;

    Rigidbody Body;

    void Start()
    {
        Body = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Body.velocity = Direction * Speed;
        Body.position += Body.velocity * Time.deltaTime;
    }
}
