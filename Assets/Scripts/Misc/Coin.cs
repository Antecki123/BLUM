using UnityEngine;

public interface ICollectable
{
    public void Collect();
}

public class Coin : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}