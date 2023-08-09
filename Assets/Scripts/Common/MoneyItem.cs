using UnityEngine;

public class MoneyItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMain player))
        {
            player.AddOneMoney();
            Destroy(gameObject);
        }
    }
}
