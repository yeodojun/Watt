using UnityEngine;

public class Attack : MonoBehaviour
{
    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }

}
