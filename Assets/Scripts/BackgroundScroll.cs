using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Vector2 speed;

    private Vector2 currentSpeed;

    private void Update()
    {
        currentSpeed += speed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", currentSpeed);
    }
}