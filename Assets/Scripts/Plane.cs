using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void Jump()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}