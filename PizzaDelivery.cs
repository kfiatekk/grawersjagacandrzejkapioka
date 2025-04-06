using System.Linq;
using UnityEngine;

public class PizzaDelivery : MonoBehaviour
{
    public GameObject interactIcon;
    private ShopTrigger shop;
    private bool playerInDeliveryPoint = false;
    private bool isDelivered = false; // Flaga oznaczaj¹ca dostarczon¹ pizzê
    private DialogManager dialogManager;

    private void Start()
    {
        shop = FindObjectOfType<ShopTrigger>();
        dialogManager = FindObjectOfType<DialogManager>();

        if (shop == null)
        {
            Debug.LogError("Nie znaleziono ShopTrigger!");
        }
        if (dialogManager == null)
        {
            Debug.LogError("DialogManager nie jest znaleziony w scenie!");
        }
        if (interactIcon == null)
        {
            Debug.LogError("InteractIcon nie jest przypisany w Inspectorze!");
        }

        interactIcon?.SetActive(false);
    }

    private void Update()
    {
        if (!isDelivered && playerInDeliveryPoint && Input.GetKeyDown(KeyCode.E) && shop.IsMissionStarted())
        {
            CheckDelivery();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDelivered && other.CompareTag("Player") && shop.IsMissionStarted())
        {
            playerInDeliveryPoint = true;
            interactIcon?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDeliveryPoint = false;
            interactIcon?.SetActive(false);
        }
    }

    private void CheckDelivery()
    {
        if (shop == null)
        {
            Debug.LogError("Nie znaleziono ShopTrigger!");
            return;
        }

        Transform[] deliveryTargets = shop.GetDeliveryPoints();
        bool[] deliveriesCompleted = shop.GetDeliveriesCompleted();

        for (int i = 0; i < deliveryTargets.Length; i++)
        {
            if (deliveryTargets[i] == this.transform && !deliveriesCompleted[i])
            {
                shop.DeliverPizza(this.transform);
                isDelivered = true; // Oznacz punkt jako dostarczony
                interactIcon?.SetActive(false); // Ukryj ikonê "E"
                playerInDeliveryPoint = false; // Dezaktywuj interakcjê
                return;
            }
        }

        int remainingDeliveries = shop.GetDeliveryPoints().Length - deliveriesCompleted.Count(x => x);
        string remainingMessage = "Zosta³o jeszcze " + remainingDeliveries + " miejsce/miejsca!";
        dialogManager.ShowDialog(remainingMessage);
    }
}
