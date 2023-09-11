using UnityEngine;

public class MoneyItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMainService player))
        {
            player.AddOneMoney();
            Destroy(gameObject);
        }
    }
}
