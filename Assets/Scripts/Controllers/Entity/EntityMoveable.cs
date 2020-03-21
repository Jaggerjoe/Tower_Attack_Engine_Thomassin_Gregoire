using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EntityMoveable : Entity
{
    [Range(1, 50)]
    public float moveSpeed = 1;
  

    [Header("Target")]
    // Variable target
    private GameObject m_GlobalTarget;
    public GameObject target;
    public GameObject prefabBulletPlayer;
    

    [Header("Stop Time")]
    // Variable de temps d'arret
    public float timeWaitBeforeMove = 1;
    private float m_CurrentTimeBeforeNextMove = 0;
    
    private NavMeshAgent m_NavMeshAgent;


    public override void RangeToDoAttack(Entity target)
    {
        base.RangeToDoAttack(target);
        if(target)
        {
            m_NavMeshAgent.SetDestination(target.transform.position);
            Debug.Log("coucou");
        }
        if(!target)
        {
            SetDestination();
        }
        
    }
    public override void InitEntity()
    {
        base.InitEntity();
       //Initialisation _ construction
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }
    public override void RestartEntity()
    {
        base.RestartEntity();
        m_NavMeshAgent.speed = moveSpeed;
        SetDestination();
    }
    public void SetGlobalTarget(GameObject target)
    {
        m_GlobalTarget = target;
        SetDestination();
    }

    private void SetDestination()
    {
        if (m_GlobalTarget)
        {
            m_NavMeshAgent.SetDestination(m_GlobalTarget.transform.position);
        }
    }

    public override void Update()
    {
        base.Update();
        if(m_NavMeshAgent.isStopped)
        {
            if (m_CurrentTimeBeforeNextMove < timeWaitBeforeMove)
            {
                m_CurrentTimeBeforeNextMove += Time.deltaTime;
            }
            else
            {
                m_NavMeshAgent.isStopped = false;
                SetDestination();
                
            }
        }
    }

    protected override bool DoAttack(Entity targetEntity)
    {
        if(base.DoAttack(targetEntity))
        {
            m_NavMeshAgent.isStopped = true;
            //On instancie la bullet          
            GameObject bullet = PoolManager.Instance.GetElement(prefabBulletPlayer);
            bullet.transform.position = target.transform.position;
            bullet.GetComponent<Rigidbody>();
            bullet.GetComponent<ProjectilEntityPlayer>().GetComponent<Rigidbody>();       
            bullet.SetActive(true);         
            m_CurrentTimeBeforeNextMove = 0;
            return true;
        }
        return false;
    }

}
