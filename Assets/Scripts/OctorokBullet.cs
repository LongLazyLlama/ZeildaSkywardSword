using UnityEngine;

public class OctorokBullet : MonoBehaviour
{
    public float Speed = 2;
    public float BulletDamage = 1.5f;

    void Update()
    {
        if (!PauzeMenu.pauzeMenu.IsGamePauzed)
        {
            this.transform.position += this.transform.forward * Speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                LivesManager.Manager.ReducePlayerHP(BulletDamage);
                Destroy(this.gameObject);
                break;
            case "Shield":
                Speed = -Speed;
                break;
            case "Enemy":
                other.gameObject.GetComponentInParent<EnemyHealth>().ReduceHealth(9999);
                Destroy(this.gameObject);
                break;
            case "Untagged":
                Destroy(this.gameObject);
                break;
        }
    }
}
