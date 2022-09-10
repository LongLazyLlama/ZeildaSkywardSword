using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    public Animator Animator;
    public GameObject SwordObject;
    public Collider SwordHitbox;

    public float SwordDamage = 1;

    bool _isAttacking;

    public void Start()
    {
        SwordObject.gameObject.SetActive(false);
        SwordHitbox.enabled = false;
    }

    public void OnLeftMouseButtonUsed(InputAction.CallbackContext context)
    {
        if (context.started && _isAttacking == false)
        {
            SwordObject.gameObject.SetActive(true);
            SwordHitbox.enabled = true;

            Animator.SetBool("Attacking", true);

            _isAttacking = true;
        }
    }

    public void OnAttackEnd()
    {
        SwordObject.gameObject.SetActive(false);
        SwordHitbox.enabled = false;

        Animator.SetBool("Attacking", false);

        _isAttacking = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy " + other.gameObject + " hit!");
            other.gameObject.GetComponentInParent<EnemyHealth>().ReduceHealth(SwordDamage);
        }
    }

}
