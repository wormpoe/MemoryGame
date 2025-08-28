using UnityEngine;

public abstract class Dialog : MonoBehaviour
{
    protected void Hide()
    {
        Destroy(gameObject);
    }
}
