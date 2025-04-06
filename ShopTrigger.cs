using System.Collections;
using System.Linq;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject interactIcon; // Ikona "E"
    public Transform[] deliveryPoints; // Lista miejsc dostawy
    private bool missionStarted = false;
    private bool missionCompleted = false;
    private bool[] deliveriesCompleted;
    private bool playerInShop = false;
    private DialogManager dialogManager;
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    private int deliveriesToComplete;

    private void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();

        if (interactIcon == null)
        {
            Debug.LogError("InteractIcon nie jest przypisany w Inspectorze!");
        }

        if (deliveryPoints.Length == 0)
        {
            Debug.LogError("Brak przypisanych miejsc dostawy w Inspectorze!");
        }

        if (dialogManager == null)
        {
            Debug.LogError("DialogManager nie jest znaleziony w scenie!");
        }
        

        interactIcon?.SetActive(false); // Ukryj ikon� "E" na pocz�tku
        deliveriesCompleted = new bool[deliveryPoints.Length];
        deliveriesToComplete = deliveryPoints.Length;
    }

    private void Update()
    {
        if (playerInShop && Input.GetKeyDown(KeyCode.E) && !missionCompleted)
        {
            Debug.Log("Gracz nacisn�� klawisz E w sklepie");
            // Pobranie PlayerController i zmiana sprite'a
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("Found PlayerController: " + playerController.gameObject.name);
                playerController.ChangePlayerSprite();
            }
            else
            {
                Debug.LogError("PlayerController not found!");
            }
            AcceptMission();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D wywo�ane z: " + other.name);
        if (other.CompareTag("Player"))
        {
            playerInShop = true;
            if (missionCompleted)
            {
                dialogManager.ShowDialog("Pizza ju� rozniesiona i nie potrzeba wi�cej pomocy.");
            }
            else
            {
                interactIcon?.SetActive(true); // Poka� ikon� "E"
            }
            Debug.Log("Gracz wszed� do sklepu");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D wywo�ane z: " + other.name);
        if (other.CompareTag("Player"))
        {
            playerInShop = false;
            interactIcon?.SetActive(false); // Ukryj ikon� "E"
            dialogManager.HideDialog();
            Debug.Log("Gracz opu�ci� sklep");
        }
    }

    public void AcceptMission()
    {
        if (missionStarted)
        {
            Debug.LogWarning("Misja ju� zosta�a rozpocz�ta!");
            return;
        }

        missionStarted = true;
        interactIcon?.SetActive(false); // Ukryj ikon� "E"

        string deliveryMessage = "Zanie� pizz� do: ";
        for (int i = 0; i < deliveryPoints.Length; i++)
        {
            deliveryMessage += deliveryPoints[i].name;
            if (i < deliveryPoints.Length - 1)
            {
                deliveryMessage += ", ";
            }
        }

        Debug.Log(deliveryMessage);
        dialogManager.ShowDialog("Zacz��e� zadanie! " + deliveryMessage);

        

    }

    public bool HasPizza()
    {
        return missionStarted;
    }

    public bool IsMissionStarted()
    {
        return missionStarted;
    }

    public Transform[] GetDeliveryPoints()
    {
        return deliveryPoints;
    }

    public bool[] GetDeliveriesCompleted()
    {
        return deliveriesCompleted;
    }

    public void DeliverPizza(Transform deliveryPoint)
    {
        for (int i = 0; i < deliveryPoints.Length; i++)
        {
            if (deliveryPoints[i] == deliveryPoint && !deliveriesCompleted[i])
            {
                deliveriesCompleted[i] = true;
                deliveriesToComplete--;

                Debug.Log("Pizza dostarczona do: " + deliveryPoint.name);
                dialogManager.ShowDialog("Pizza dostarczona do: " + deliveryPoint.name);

                if (deliveriesToComplete > 0)
                {
                    string remainingDeliveryMessage = "Zosta�o jeszcze " + deliveriesToComplete + " miejsce/miejsca!";
                    Debug.Log(remainingDeliveryMessage);
                    StartCoroutine(ShowTemporaryDialog(remainingDeliveryMessage, 2f));
                }
                else
                {
                    missionStarted = false;
                    missionCompleted = true;
                    PlayerController playerController = FindObjectOfType<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.ChangePlayerBack();
                        Debug.Log("Gracz powr�ci� do oryginalnego wygl�du.");
                    }
                    StartCoroutine(ShowCompletionDialog());
                }

                return;
            }
        }

        int remainingDeliveries = deliveryPoints.Length - deliveriesCompleted.Count(x => x);
        string remainingDeliveriesMessage = "Zosta�o jeszcze " + remainingDeliveries + " miejsce/miejsca!";
        Debug.Log(remainingDeliveriesMessage);
        StartCoroutine(ShowTemporaryDialog(remainingDeliveriesMessage, 2f));
    }

    private IEnumerator ShowTemporaryDialog(string message, float duration)
    {
        dialogManager.ShowDialog(message);
        yield return new WaitForSeconds(duration);
        dialogManager.HideDialog();
    }

    private IEnumerator ShowCompletionDialog()
    {
        Debug.Log("Sko�czy�e� zadanie!");
        dialogManager.ShowDialog("Sko�czy�e� zadanie! Wygra�e� gr�!");

        yield return new WaitForSeconds(5);

        dialogManager.HideDialog();
        Debug.Log("Dialog ukryty po 5 sekundach");
    }
}
