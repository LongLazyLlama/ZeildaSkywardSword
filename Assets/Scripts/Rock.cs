using UnityEngine;

public class Rock : MonoBehaviour
{
    public GameObject Player;
    public Rigidbody Rockbody;

    public float RockDamage = 2.0f;

    bool _PickedUp;
    bool _thrown;

    public void PickUp()
    {
        Rockbody.isKinematic = true;
        _PickedUp = true;

    }

    public void Update()
    {
        if (_PickedUp)
        {
            Vector3 newpos = new Vector3(Player.transform.position.x, Player.transform.position.y + 2.6f, Player.transform.position.z);
            this.transform.position = newpos;
            this.transform.forward = Player.transform.forward;

            if (PlayerController.Player.Interactible == null)
            {
                PlayerController.Player.Interactible = this.gameObject;
            }
        }
    }

    public void ThrowRock()
    {
        _PickedUp = false;
        Rockbody.isKinematic = false;

        //Takes the x and y component from the forward.
        Vector3 xzAbsoluteForward = Vector3.Scale(this.transform.forward, new Vector3(1, 0, 1));

        //Returns the rotationvalue from the forward direction.
        Quaternion forwardRotation = Quaternion.LookRotation(xzAbsoluteForward);

        //Added force.
        Vector3 force = new Vector3(0,300, 1000);

        var iets = forwardRotation * force;
        Rockbody.AddForce(iets, ForceMode.Impulse);

        _thrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_thrown && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<EnemyHealth>().ReduceHealth(RockDamage);
        }
        else
        {
            _thrown = false;
        }
    }
}
