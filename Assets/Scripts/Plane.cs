using UnityEngine;

public class Plane : MonoBehaviour
{

    [SerializeField] private GameObject explosion;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxRotation;
    [SerializeField] private float minRotation;
    [SerializeField] private float rotationLerp;

    private float currentRotation;

    private Rigidbody2D rigid;
    private GameManager gameManager;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!gameManager.IsGameStarted)
        {
            rigid.isKinematic = true;
            rigid.velocity = Vector3.zero;
            return;
        }

        if (rigid.isKinematic == true)
            rigid.isKinematic = false;

        currentRotation = Mathf.Lerp(currentRotation, minRotation, Time.deltaTime * rotationLerp);
        transform.eulerAngles = new Vector3(0, 0, currentRotation);

        if (transform.position.y < gameManager.Cam.ViewportToWorldPoint(new Vector3(0, -0.2f, 0)).y)
            Destroy(GameManager.LoseReason.Crash);

        if (transform.position.y > gameManager.Cam.ViewportToWorldPoint(new Vector3(0, 1.2f, 0)).y)
            Destroy(GameManager.LoseReason.GoToSpace);
    }

    public void Jump()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        currentRotation = maxRotation;

        AudioManager.Instance.JumpSFX();
    }

    public void Destroy(GameManager.LoseReason reason)
    {
        gameManager.Lose(reason);
        //Instantiate(explosion,transform.position, Quaternion.identity, null);
        gameObject.SetActive(false);
    }
}