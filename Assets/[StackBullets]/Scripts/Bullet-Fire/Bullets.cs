using HCB.Core;
using HCB.PoolingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AutoLayout3D //dikkat
{
    public class Bullets : MonoBehaviour
    {
        [SerializeField] private Transform bulletSpawnPos;

        [SerializeField] private float _spawnRate = 5f;
        [SerializeField] private float _bulletDeleteTime = 5f;
        private bool _isCollided;
        private bool _isGameStarted;

        private int _bulletCount;
        private float _timer = Mathf.Infinity;

        public bool CanShoot = true;

        #region Listeners
        private void OnEnable()
        {
            if (Managers.Instance == null) return;

            LevelManager.Instance.OnLevelStart.AddListener(() => _isGameStarted = true);
            GameManager.Instance.OnStageEnd.AddListener(() => _isGameStarted = false);
            EventManager.BulletIncrease.AddListener(AddBullet);
        }

        private void OnDisable()
        {
            if (Managers.Instance == null) return;

            LevelManager.Instance.OnLevelStart.RemoveListener(() => _isGameStarted = true);
            GameManager.Instance.OnStageEnd.RemoveListener(() => _isGameStarted = false);
            EventManager.BulletIncrease.RemoveListener(AddBullet);
        }

        #endregion




        void SpawnBullets()
        {

        if (!CanShoot) return;
    
        if (!_isGameStarted) return;

            GameObject gO = PoolingSystem.Instance.InstantiateAPS("Bullet", bulletSpawnPos.position, bulletSpawnPos.rotation);

            

            //gO.transform.eulerAngles = new Vector3(0, -90, 0); //rotation Vector3 almiyor, eulerAngles yazilmali

            PoolingSystem.Instance.DestroyAPS(gO, _bulletDeleteTime);

            _bulletCount--;

            EventManager.BulletDecrease.Invoke();


        }

        private void Update()
        {
            if (_bulletCount <= 0) return;

            _timer += Time.deltaTime;

            if (_timer >= _spawnRate)
            {
                SpawnBullets();
                _timer = 0;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(bulletSpawnPos.position, .1f);
        }

        void AddBullet()
        {
            _bulletCount++;
        }

    }

}
