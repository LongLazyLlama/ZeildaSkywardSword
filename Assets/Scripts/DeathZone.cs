using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LivesManager.Manager.InstaKill();
        }
    }
}
