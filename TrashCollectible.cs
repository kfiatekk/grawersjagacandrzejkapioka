using UnityEngine;

public class TrashCollectible : MonoBehaviour
{
    public NPCInteraction npcInteraction; // Referencja do skryptu NPCInteraction
    private bool isCollected = false; // Flaga, aby zapobiec wielokrotnemu zbieraniu

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return; // Je�li ju� zebrano, wyjd� z metody
        if (collision.CompareTag("Player"))
        {
            isCollected = true; // Oznacz �mie� jako zebrany
            Debug.Log($"[TrashCollectible] Gracz zebra� �mie�: {gameObject.name}");

            Destroy(gameObject); // Usu� obiekt �mieci
            if (npcInteraction != null)
            {
                npcInteraction.CollectTrash(); // Powiadom NPC o zebraniu �mieci
            }
            else
            {
                Debug.LogError("[TrashCollectible] Brak przypisania NPCInteraction!");
            }
        }
    }
}
