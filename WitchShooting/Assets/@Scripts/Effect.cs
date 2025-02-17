using UnityEngine;

public class Effect : MonoBehaviour
{
    // * Animation Event
    void EffectEndEvent()
    {
        Destroy(gameObject);
    }
}
