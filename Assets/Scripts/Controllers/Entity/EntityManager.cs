using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public GameObject prefabToInstantiate;

    public GameObject prefabTowerEnemyInstantiate;
    public GameObject prefabTowerPlayerInstantiate;
    
    public GameObject prefabEnemy;
    
    public GameObject globalTarget;
    public GameObject globalTarget1;
  

    private Camera m_CurrentCamera;

    private void Awake()
    {
        m_CurrentCamera = FindObjectOfType<Camera>();
       
    }
    private void Start()
    {
        InstantaiteTower();
    }
    private void Update()
    {
        InstantiateEnemy();
       
    }
    private void InstantaiteTower()
    {
        GameObject towerinstanted = PoolManager.Instance.GetElement(prefabTowerEnemyInstantiate);
        GameObject towerPlayerInstantiated = PoolManager.Instance.GetElement(prefabTowerPlayerInstantiate);

        towerinstanted.transform.position = globalTarget.transform.position;
        towerPlayerInstantiated.transform.position = globalTarget1.transform.position;
       
        towerinstanted.SetActive(true);
        towerPlayerInstantiated.SetActive(true);


    }
    private void InstantiateEnemy()
    {
        // Creation d'un Ray à partir de la camera
        Ray ray = m_CurrentCamera.ScreenPointToRay(Input.mousePosition);
        float mult = 1000;
        Debug.DrawRay(ray.origin, ray.direction * mult, Color.green);

        // Recuperation du bouton droit de la souris.
        if (Input.GetMouseButtonDown(0))
        {
            // 
            if (Physics.Raycast(ray, out RaycastHit hit, mult, LayerMask.GetMask("Default")))
            {
                // On recupère un élement depuis le poolmanager
                GameObject instantiated = PoolManager.Instance.GetElement(prefabToInstantiate);
                instantiated.transform.position = hit.point;
                instantiated.SetActive(true);

                Entity entity = instantiated.GetComponent<Entity>();
                if (entity)
                {
                    entity.InitEntity();
                    if (entity is EntityMoveable moveable)
                    {
                        moveable.SetGlobalTarget(prefabTowerEnemyInstantiate);
                    }
                    entity.RestartEntity();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // 
            if (Physics.Raycast(ray, out RaycastHit hit, mult, LayerMask.GetMask("Default")))
            {
                // On recupère un élement depuis le poolmanager
                GameObject instantiated = PoolManager.Instance.GetElement(prefabEnemy);
                instantiated.transform.position = hit.point;
                instantiated.SetActive(true);
                Entity entity = instantiated.GetComponent<Entity>();
                if (entity)
                {
                    entity.InitEntity();
                    if (entity is EntityMoveable moveable)
                    {
                        moveable.SetGlobalTarget(prefabTowerPlayerInstantiate);
                    }
                    entity.RestartEntity();
                }
            }
        }
    }
}
