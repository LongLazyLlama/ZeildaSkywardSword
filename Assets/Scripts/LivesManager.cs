using UnityEngine;

using Random = UnityEngine.Random;

public enum Sounds
{
    Hit1,
    Hit2,
    Hit3,
    Hit4
}

public class LivesManager : MonoBehaviour
{
    public static LivesManager Manager;

    public Transform MaxHealthBar;
    public Transform CurrentHealthBar;

    public GameObject Player;
    public GameObject EmptyHeart;
    public GameObject FullHeart;
    public GameObject HalfHeart;

    [Range(1, 20)]
    public int MaxHP;
    [Range(0, 20)]
    public float CurrentHP;

    public bool Invincible;

    float _previousMaxHP;
    float _previousHP;

    int _lastHeartIndex;

    string _randomSound;

    private void Start()
    {
        Manager = this;
    }

    private void Update()
    {
        SetPlayerMaxHP();
        SetPlayerCurrentHP();

        if (!Invincible)
        {
            IsPlayerDead();
        }
    }

    private void SetPlayerMaxHP()
    {
        if (MaxHP != _previousMaxHP)
        {
            foreach (Transform child in MaxHealthBar)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < MaxHP; i++)
            {
                var heart = Instantiate(EmptyHeart);
                heart.transform.SetParent(MaxHealthBar);
            }

            _previousMaxHP = MaxHP;
        }
    }

    private void SetPlayerCurrentHP()
    {
        if (CurrentHP != _previousHP)
        {
            if (CurrentHP > MaxHP){ CurrentHP = MaxHP; return; }

            foreach (Transform child in CurrentHealthBar)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < Mathf.Floor(CurrentHP); i++)
            {
                var heart = Instantiate(FullHeart);
                heart.transform.SetParent(CurrentHealthBar);
            }

            if (CurrentHP%1 > 0)
            {
                var heart = Instantiate(HalfHeart);
                heart.transform.SetParent(CurrentHealthBar);
            }
            //RescaleCurrentHeart();
            _previousHP = CurrentHP;
        }
    }

    private void RescaleCurrentHeart()
    {
        _lastHeartIndex = Mathf.CeilToInt(CurrentHP - 1);

        Transform child = CurrentHealthBar.GetChild(Mathf.Max(_lastHeartIndex, 0));
        child.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    private void IsPlayerDead()
    {
        if (CurrentHP <= 0)
        {
            Player.GetComponent<PlayerController>().enabled = false;

            //FindObjectOfType<AudioManager>().Play("SpyroFinalHit");

            FindObjectOfType<SwitchScene>().LevelToLoad = 3;
            FindObjectOfType<SwitchScene>().FadeToLevel();

            Destroy(Player);
            this.enabled = false;
        }
    }

    public void IncreaseMaxHP()
    {
        MaxHP += 1;
        CurrentHP = MaxHP;
    }

    public void IncreaseCurrentHP()
    {
        CurrentHP += 1;
    }

    public void ReducePlayerHP(float amount)
    {
        CurrentHP -= amount;

        //Sounds sounds = (Sounds)Random.Range(0, 4);
        //_randomSound = sounds.ToString();

        //FindObjectOfType<AudioManager>().Play(_randomSound);
    }

    public void InstaKill()
    {
        CurrentHP = 0;
    }
}
