using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    private Button _button { get { return GetComponent<Button>(); } }
    void Start()
    {
        _button.onClick.AddListener(() => FindObjectOfType<AudioManager>().PlaySound("Pauze"));
    }
    public void HoverSound()
    {
        FindObjectOfType<AudioManager>().PlaySound("Select");
    }
}
