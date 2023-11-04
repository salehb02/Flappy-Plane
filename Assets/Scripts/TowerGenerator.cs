using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private float towerSpeed = 2f;

    private bool generate = true;
    private List<Tower> towers = new List<Tower>();
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private IEnumerator TowerCreatorCoroutine()
    {
        while (generate)
        {
            var pos = gameManager.Cam.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, 0));
            pos.z = 0;

            var tower = Instantiate(towerPrefab, pos + (Vector3.up * Random.Range(minHeight, maxHeight) * (Random.value > 0.5f ? 1 : -1)), Quaternion.identity, null);
            tower.SetSpeed(towerSpeed);

            towers.Add(tower);

            yield return new WaitForSeconds(delay);
        }
    }

    public void StartGenerator()
    {
        generate = true;
        StartCoroutine(TowerCreatorCoroutine());
    }

    public void StopGenerator()
    {
        generate = false;

        foreach (var tower in towers)
            tower.SetSpeed(0);
    }

    public void DestroyTower(Tower tower)
    {
        towers.Remove(tower);
    }
}