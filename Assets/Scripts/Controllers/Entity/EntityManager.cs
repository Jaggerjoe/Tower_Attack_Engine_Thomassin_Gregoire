using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : SingletonMono<EntityManager>
{
    // Ref vers la global target des entités Player
    public GameObject towerIA;
    public GameObject outpostIA;
    public GameObject outpost2IA;
    
    // Ref vers la global target des entités IA
    public GameObject towerPlayer;
    public GameObject outspotPlayer;
    public GameObject outspotPlayer2;

    public Action<Alignment> OnTowerDestroy;

    public void PopElementFromData(EntityData entityData, Vector3 position)
    {
        GameObject newInstantiate = PoolManager.Instance.GetElement(entityData);
        
        if (newInstantiate != null)
        {
            SetPopElement(newInstantiate, position);
        }
        else
        {
            Debug.LogError("NO POOLED DATA PREFAB : " + entityData.name);
        }
    }

    public void PopElementFromPrefab(GameObject prefabToPop, Vector3 position)
    {
        GameObject newInstantiate = PoolManager.Instance.GetElement(prefabToPop);
        if (newInstantiate != null)
        {
            SetPopElement(newInstantiate, position);
        }
        else
        {
            Debug.LogError("NO POOLED PREFAB : " + prefabToPop.name);
        }
    }
    public void PoolElement(GameObject toPool)
    {
        if (towerPlayer == toPool)
        {
            OnTowerDestroy?.Invoke(Alignment.Player);
        }
        else if (towerIA == toPool)
        {
            OnTowerDestroy?.Invoke(Alignment.IA);
        }

        PoolManager.Instance.PoolElement(toPool);
    }

    // Fonction centrale.
    // Toute instantiation d'entité doit passer par cette fonction.
    // Elle centralise l'initialisation de l'entité.
    private void SetPopElement(GameObject newInstantiate, Vector3 position)
    {
        newInstantiate.transform.position = position;
        newInstantiate.SetActive(true);
        Entity entity = newInstantiate.GetComponent<Entity>();
        if (entity is EntityMoveable moveable)
        {
            if (moveable.entityData.alignment == Alignment.IA)
            {
                if ((outspotPlayer != null) && (outspotPlayer2 != null))
                {
                    if (Vector3.Distance(moveable.transform.position, outspotPlayer.transform.position) <= Vector3.Distance(moveable.transform.position, outspotPlayer2.transform.position))
                    {
                        moveable.SetGlobalTarget(outspotPlayer);
                    }
                }
                if ((outspotPlayer2 != null) && (outspotPlayer != null))
                {
                    if (Vector3.Distance(moveable.transform.position, outspotPlayer2.transform.position) <= Vector3.Distance(moveable.transform.position, outspotPlayer.transform.position))
                    {
                        moveable.SetGlobalTarget(outspotPlayer2);
                    }
                }
                if((outspotPlayer != null) && (towerPlayer != null))
                {
                    if (Vector3.Distance(moveable.transform.position, outspotPlayer.transform.position) <= Vector3.Distance(moveable.transform.position, towerPlayer.transform.position))
                    {
                        moveable.SetGlobalTarget(outspotPlayer);
                    }
                    else
                    {
                        moveable.SetGlobalTarget(towerPlayer);
                    }
                }
                if ((outspotPlayer2 != null) && (towerPlayer != null))
                {
                    if (Vector3.Distance(moveable.transform.position, outspotPlayer2.transform.position) <= Vector3.Distance(moveable.transform.position, towerPlayer.transform.position))
                    {
                        moveable.SetGlobalTarget(outspotPlayer2);
                    }
                    else
                    {
                        moveable.SetGlobalTarget(towerPlayer);
                    }
                }               
                if ((outspotPlayer == null) && (outspotPlayer2 == null))
                {
                    moveable.SetGlobalTarget(towerPlayer);
                }
                if(outspotPlayer == null)
                {
                    entity.RestartEntity();
                }
                if(outspotPlayer2 == null)
                {
                    entity.RestartEntity();
                }
                entity.RestartEntity();
            }
            if (moveable.entityData.alignment == Alignment.Player)
            {
                if((outpostIA != null) && (outpost2IA != null))
                {
                    if (Vector3.Distance(moveable.transform.position, outpostIA.transform.position) <= Vector3.Distance(moveable.transform.position, outpost2IA.transform.position))
                    {
                        moveable.SetGlobalTarget(outpostIA);                        
                    }   
                    else
                    {
                        moveable.SetGlobalTarget(outpost2IA);
                    }
                }                
                if(outpost2IA == null)
                {
                    entity.RestartEntity();
                }
                if(outpostIA == null)
                {
                    entity.RestartEntity();
                }
                if ((outpostIA != null) && (towerIA != null) && (outpost2IA == null))
                {
                    if (Vector3.Distance(moveable.transform.position, outpostIA.transform.position) <= Vector3.Distance(moveable.transform.position, towerIA.transform.position))
                    {
                        moveable.SetGlobalTarget(outpostIA);
                    }
                    else
                    {
                        moveable.SetGlobalTarget(towerIA);
                    }
                    entity.RestartEntity();
                }
                if ((outpost2IA != null) && (towerIA != null) && (outpostIA == null))
                {
                    if (Vector3.Distance(moveable.transform.position, outpost2IA.transform.position) <= Vector3.Distance(moveable.transform.position, towerIA.transform.position))
                    {
                        moveable.SetGlobalTarget(outpost2IA);
                    }
                    else
                    {
                        moveable.SetGlobalTarget(towerIA);
                    }
                    entity.RestartEntity();
                }
                
                if ((outpostIA == null) && (outpost2IA == null))
                {
                    moveable.SetGlobalTarget(towerIA);
                }
                entity.RestartEntity();
            }
                
            entity.RestartEntity();
        }
    }

   
}
