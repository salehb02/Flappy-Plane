using UnityEngine;

public class Tower : MonoBehaviour
{
    private float speed;
    private GameManager gameManager;
    private TowerGenerator towerGenerator;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        towerGenerator = FindObjectOfType<TowerGenerator>();
    }

    private void Update()
    {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));

        var screenX = gameManager.Cam.ViewportToWorldPoint(new Vector3(-1f, 0, 0)).x;

        if (transform.position.x < screenX)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        towerGenerator.DestroyTower(this);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.Plane.Destroy(GameManager.LoseReason.HitTower);
    }
}