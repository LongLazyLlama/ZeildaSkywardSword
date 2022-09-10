using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int RupeeValue;

    [Tooltip("Item number: 1 = Rupee, 2 = MaxHPHeart, 3 = RecoveryHeart")]
    public int ItemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (ItemType)
            {
                case 1:
                    //Rupee
                    Destroy(this.gameObject);
                    RupeeManager.Manager.IncreaseRupeeCount(RupeeValue);
                    break;
                case 2:
                    //MaxHPHeart
                    Destroy(this.gameObject);
                    LivesManager.Manager.IncreaseMaxHP();
                    break;
                case 3:
                    //RecoveryHeart
                    Destroy(this.gameObject);
                    LivesManager.Manager.IncreaseCurrentHP();
                    break;
            }
        }
    }
}
