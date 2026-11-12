using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] DialogueData dialogueData;
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] GameObject promptPrefab;
    [SerializeField] GameObject confirmPrompt;
    [SerializeField] float promptHeight = 2f;
    [SerializeField] float promptRadius = 10f;
    [SerializeField] float interactRadius = 2f;
    PlayerController playerController;
    Transform player;
    GameObject currentPrompt;
    bool playerInRange;
    bool promptIsMine;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;

        playerController = player.GetComponent<PlayerController>();
    }
    void Update()
    {

        if (!playerController.enabled)
        {
            HidePrompt();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < promptRadius)
            ShowPrompt();
        else
            HidePrompt();

        playerInRange = distance < interactRadius;

        if (playerInRange && !dialogueUI.isDialoguePlaying)
        {
            confirmPrompt.SetActive(true);
            promptIsMine = true;
        }
        else if (promptIsMine)
        {
            confirmPrompt.SetActive(false);
            promptIsMine = false;
        }

        if (!playerInRange)
            return;

        if (dialogueUI.isDialoguePlaying)
            return;

        if (BattleInput.Confirm())
            dialogueUI.Play(dialogueData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowPrompt();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HidePrompt();
        }
    }
    void ShowPrompt()
    {
        if (currentPrompt != null)
            return;

        currentPrompt = Instantiate(promptPrefab, transform);
        currentPrompt.transform.localPosition = new Vector3(0f, promptHeight, 0f);
    }

    void HidePrompt()
    {
        if (currentPrompt != null)
            Destroy(currentPrompt);
    }
}
