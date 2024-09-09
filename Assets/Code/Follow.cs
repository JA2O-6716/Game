using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager._instance.player.transform.position);
    }
}
