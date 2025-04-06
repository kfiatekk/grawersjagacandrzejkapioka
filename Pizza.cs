using UnityEngine;

public class Pizza : MonoBehaviour
{
    private bool isCarried = false; // Czy gracz ma pizz�?

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCarried)
        {
            Debug.Log("Gracz podni�s� pizz�!");
            isCarried = true;
            gameObject.SetActive(false); // Ukryj pizz� po podniesieniu
        }
    }

    public bool IsCarried()
    {
        return isCarried;
    }

    public void DropPizza()
    {
        if (isCarried)
        {
            Debug.Log("Pizza zosta�a upuszczona!");
            isCarried = false;
            gameObject.SetActive(true); // Poka� pizz� po upuszczeniu
        }
    }
}