using UnityEngine;

public class OctorokBehaviour : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject BulletSpawnPos;

    [Space]
    public float PlayerDetectRange = 8.0f;
    public float AttackRange;

    [Space]
    public float FireRate = 2.0f;

    float _timer;

    void Update()
    {
        if (!PauzeMenu.pauzeMenu.IsGamePauzed)
        {
            LookAtPlayer();
            Attack();

            _timer += Time.fixedDeltaTime;
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(this.transform.position, PlayerController.Player.transform.position) < AttackRange)
        {
            if (_timer >= FireRate)
            {
                GameObject bullet = Instantiate(BulletPrefab, BulletSpawnPos.transform.position, Quaternion.identity);
                bullet.transform.forward = BulletSpawnPos.transform.forward;

                _timer = 0;
            }
        }
    }

    private void LookAtPlayer()
    {
        if (Vector3.Distance(this.transform.position, PlayerController.Player.transform.position) < PlayerDetectRange)
        {
            Vector3 groundedPosition = new Vector3(PlayerController.Player.transform.position.x, 0, 
                PlayerController.Player.transform.position.z);

            this.transform.LookAt(groundedPosition);

            //Sets the bulletspawn to aim at the player at the height of his shield.
            Vector3 aimPosition = new Vector3(PlayerController.Player.transform.position.x,
                PlayerController.Player.transform.position.y + 1f,
                PlayerController.Player.transform.position.z);

            BulletSpawnPos.transform.LookAt(aimPosition);
        }
    }
}
