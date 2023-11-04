using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Vector2 speed;

    private Vector2 currentSpeed;
    private bool isStopped;

    private void Update()
    {
        if (!isStopped)
            currentSpeed += speed * Time.deltaTime;

        mat.SetTextureOffset("_MainTex", currentSpeed);
    }

    public void Stop()
    {
        isStopped = true;
    }
}