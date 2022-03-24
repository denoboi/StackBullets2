using HCB.PoolingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPos;

    [SerializeField] private float _spawnRate = 5f;
    [SerializeField] private float _bulletDeleteTime = 5f;
    private bool _isCollided;

    private void Start()
    {
        StartCoroutine(SpawnRate());
    }

    void SpawnBullets()
    {
        GameObject gO = PoolingSystem.Instance.InstantiateAPS("Bullet", bulletSpawnPos.position + Random.insideUnitSphere * 0.3f);
        PoolingSystem.Instance.DestroyAPS(gO, _bulletDeleteTime);
    }

    IEnumerator SpawnRate()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(_spawnRate);
            SpawnBullets();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(bulletSpawnPos.position, .3f);
    }

}