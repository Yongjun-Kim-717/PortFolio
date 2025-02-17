using UnityEngine;

public class WarningLine : MonoBehaviour
{
    public Color WarningColor;

    private void Start()
    {
        WarningColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if(WarningColor.a <=255)
        {
            WarningColor.a += 10;
        }
        else
        {
            WarningColor.a = 0;
        }

        this.gameObject.GetComponent<SpriteRenderer>().color = WarningColor;
        gameObject.transform.position = GameManager.Instance.PlayerPos + new Vector3(0,3,0);
    }
}
