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
    public float timer = 0;
    private float m_CurrentTimer = 3;
   

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
        SpawnEnemy();
    }
    private void InstantaiteTower()
    {
        //On récupère la tour setter dans le poolManger
        GameObject towerinstanted = PoolManager.Instance.GetElement(prefabTowerEnemyInstantiate);
        GameObject towerPlayerInstantiated = PoolManager.Instance.GetElement(prefabTowerPlayerInstantiate);
        //On set son endroit de spawn 
        towerinstanted.transform.position = globalTarget.transform.position;
        towerPlayerInstantiated.transform.position = globalTarget1.transform.position;
       //On active la tour.
        towerinstanted.SetActive(true);
        towerPlayerInstantiated.SetActive(true);


    }
    public void SpawnEnemy()
    {
        //On fais ecoulé selon le time.deltaTime(je sais pas trop comment il fonctionne)
        timer = timer + Time.deltaTime;
        //Si le timer est supérieur ou égale au timer courant alors on execute le if.
        if (timer >= m_CurrentTimer)
        {
            if (prefabTowerEnemyInstantiate != null)
            {
                // On recupère un élement depuis le poolmanager
                GameObject iaSpawn = PoolManager.Instance.GetElement(prefabEnemy);
                //On Fais spawn l'IA a l'endroit de la tour
                iaSpawn.transform.position = prefabTowerEnemyInstantiate.transform.position;
                //On active l'IA qui est desactivé dans le poolManager.
                iaSpawn.SetActive(true);
                Entity entity = iaSpawn.GetComponent<Entity>();
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
           
            timer = 0;
            Debug.Log("Coucou");

        }
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
