using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public GameObject Player;
    public Text Name;
    public Text TextPanel;
    public Image Image;

    [Space]
    public float TypeDelay;
    public string NPC_Name;

    [Space]
    public string CompleteDialogue;

    public void StartDialogue()
    {
        this.GetComponent<Interactible>().ShowInteractionMessage(false);
        FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("Dialogue");

        Name.text = NPC_Name;
        Image.gameObject.SetActive(true);

        StartCoroutine(TypeWriter());
    }

    public void StopDialogue()
    {
        StopCoroutine(TypeWriter());
        TextPanel.text = null;

        FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("Player");
        Image.gameObject.SetActive(false);
    }

    IEnumerator TypeWriter()
    {
        //For each letter in text piece.
        for (int i = 0; i < CompleteDialogue.Length; i++)
        {
            string currentText = CompleteDialogue.Substring(0, i);
            TextPanel.text = currentText;

            yield return new WaitForSeconds(TypeDelay);
        }
    }
}
