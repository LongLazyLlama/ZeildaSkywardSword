using UnityEngine;
using UnityEngine.UI;

public class RupeeManager : MonoBehaviour
{
    public static RupeeManager Manager;

    public Text RupeeCounter;
    public Text CounterShading;

    int _totalRupees = 0;

    void Start()
    {
        Manager = this;

        IncreaseRupeeCount(0);
    }

    public void IncreaseRupeeCount(int amount)
    {
        _totalRupees += amount;
        _totalRupees = Mathf.Max(_totalRupees, 0);

        RupeeCounter.text = _totalRupees.ToString();
        CounterShading.text = _totalRupees.ToString();
    }
}
