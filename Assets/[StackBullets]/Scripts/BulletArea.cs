using HCB.Core;
using HCB.SplineMovementSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace AutoLayout3D
{
    public class BulletArea : MonoBehaviour
    {
        /*private SplineCharacter _splineCharacter;*/ //burada tanimlarsam hafizada bosuna yer tutar
        private bool _isEnteredBulletArea;
        private bool _isExitedBulletArea;

        public GameObject dummyPrefab;


        private bool _isCollided;

        [SerializeField] private Transform _scraper;
        [SerializeField] private GameObject _spiralGeneratorPrefab;

        
        GameObject _spiralGenerator;

        SpiralMeshChanger _spiralMeshChanger;



        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name); // bu degdigi seylerin ismini donecek.

            SplineCharacterClampController _splineCharacter = other.GetComponent<SplineCharacterClampController>();


            if (_splineCharacter != null && !_isCollided)
            {
                _isCollided = true;

                Debug.Log("Boom");

                //EventManager.OnBulletTake.Invoke();

                _spiralGenerator = Instantiate(_spiralGeneratorPrefab, _scraper.position, Quaternion.identity);

                _spiralGenerator.transform.SetParent(_scraper);

                _isEnteredBulletArea = true;

                
            }


        }

        private void OnTriggerExit(Collider other)
        {
            SplineCharacterClampController _splineCharacter = other.GetComponent<SplineCharacterClampController>();
            LayoutElement3D layoutElement3D = GetComponentInParent<LayoutElement3D>();
    

        if (_splineCharacter != null && _isCollided)
            {
                _isCollided = false;
                Debug.Log("Exited");

                //for Tank Arm Animation 
                EventManager.OnBulletTakeExit.Invoke();

                /*_spiralGenerator.transform.SetParent(_splineCharacter.TankStorage);*/ //burasi degisecek!!
                _spiralGenerator.GetComponent<MeshGenerator>().StopScraping();

                //Tank sepetinde biriktirmek icin pos
                Vector3 spawnPos = Vector3.zero;

                //spawnPos = _tankStorage.transform.position;

                

                

                //Componenti burada aliyoruz cunku OnTriggerEnter'da olusturuluyor prefab'i
                _spiralMeshChanger = _spiralGenerator.GetComponentInParent<SpiralMeshChanger>();

                GameObject dummy = Instantiate(dummyPrefab, _splineCharacter.TankStorage);

                _splineCharacter.TankStorage.GetComponent<GridLayoutGroup3D>().UpdateLayout();

                _spiralGenerator.transform.SetParent(dummy.transform);

                _spiralGenerator.transform.DOLocalJump(Vector3.zero, 3, 1, 2f).SetEase(Ease.OutExpo).OnComplete(()=> EventManager.BulletIncrease.Invoke());
                _spiralGenerator.transform.DOLocalRotate(Vector3.zero, 2f);




                //changing Mesh
                _spiralMeshChanger.MeshChanger();

                

                _isEnteredBulletArea = false;

            }


        }

    }
}

