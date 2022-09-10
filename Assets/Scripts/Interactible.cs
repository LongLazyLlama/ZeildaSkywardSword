using UnityEngine;
using UnityEngine.UI;

public class Interactible : MonoBehaviour
{
    public PlayerController controller;
    public PauzeMenu pauzeMenu;

    public Image image;
    public Text text;

    [Space]
    [Tooltip("What type of interactible is this ? 1 = bird, 2 = rock, 3 = NPC")]
    public int TypeOfInteractible;

    [Space]
    public string InteractionMessage;

    public void ShowInteractionMessage(bool status)
    {
        text.text = InteractionMessage;

        image.gameObject.SetActive(status);
        //text.gameObject.SetActive(status);
    }

    public void Interact()
    {
        switch (TypeOfInteractible)
        {
            case 1:
                //Bird
                this.GetComponent<BirdController>().SitOnBird();
                break;
            case 2:
                //Rock
                this.GetComponent<Rock>().PickUp();
                break;
            case 3:
                //NPC
                this.GetComponent<Dialogue>().StartDialogue();
                break;
        }
    }

    public void ExitInteraction()
    {
        switch (TypeOfInteractible)
        {
            case 1:
                //Bird
                this.GetComponent<BirdController>().LeaveBird();
                break;
            case 2:
                //Rock
                this.GetComponent<Rock>().ThrowRock();
                break;
            case 3:
                //NPC
                this.GetComponent<Dialogue>().StopDialogue();
                break;
        }
    }
}
