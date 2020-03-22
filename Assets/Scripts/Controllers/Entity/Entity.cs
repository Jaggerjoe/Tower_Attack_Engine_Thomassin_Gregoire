using UnityEngine;

// On force le fait que l'entity ai un collider
[RequireComponent(typeof(CapsuleCollider))]
public class Entity : MonoBehaviour
{
    [Header("Props")]
    public Alignment alignment;
    [SerializeField]
    private int m_CurrentLife = 1;

    public int startLife = 1;
    public int popAmount = 1;

    [Header("AttackProps")]
    public GameObject attackContainer;


    public int damageAttack = 1;       
    public int rangeDetect = 1;
    public int rangeToDoAttack = 5;

    [Header("Time Next Attack")]
    [Range(0, 10)]
    public float timeWaitNextAttack = 1;
    private float m_CurrentTimeBeforeNextAttack = 0;
    private bool m_CanAttack = true;
    public GameObject prefabBulletPlayer;
    public static Vector3 myPoint = Vector3.zero;
    // temps entre chaque spawn de monstre 


    public  void Awake()
    {
        InitEntity();
          
    }
   

    public virtual void InitEntity()
    {
        
        
    }
    public void OnEnable()
    {
        
    }
    public void OnDisable()
    {
        

    }
  
    //Set de l'entité lorsqu'elle est activé
    //elle est reset a ses valeurs de depart
    public virtual void RestartEntity()
    {
        CapsuleCollider colliderAttack;     
        colliderAttack = attackContainer.GetComponent<CapsuleCollider>();
        colliderAttack.radius = rangeToDoAttack;

        m_CurrentLife = startLife;
     }
    
   
    public virtual void Update()
    {        
        if(!m_CanAttack)
        {
            if(m_CurrentTimeBeforeNextAttack < timeWaitNextAttack)
            {
                m_CurrentTimeBeforeNextAttack += Time.deltaTime;
            }
            else
            {
                m_CanAttack = true;
            }
        }
       
    }

    

    // Life
    private void SetLife(int amountLife)
    {
        m_CurrentLife = amountLife;
    }

    private void DamageEntity(int damage)
    {
        m_CurrentLife -= damage;
        if(m_CurrentLife <= 0)
        {
            // Entity Die
            //GameObject.Destroy(gameObject);
            PoolManager.Instance.PoolElement(gameObject);
        }
    }

    private bool IsValidEntity()
    {
        return gameObject.activeSelf && m_CurrentLife > 0;
    }

    // Attack
    private void OnTriggerStay(Collider other)
    {
        if (m_CanAttack)
        {
            //Debug.Log($"Ontrigger {name}: ", other.gameObject);
            DetectTarget(other.gameObject);
        }
    }

    private void DetectTarget(GameObject target)
    {
        // Verification si bon layer
        if(target.gameObject.layer == LayerMask.NameToLayer("Damageable"))
        {
            // Recuperation de l'entity pour tester l'alignement
            Entity entity = target.GetComponent<Entity>();
            if (entity && entity.alignment != alignment)
            {
                RangeToDoAttack(entity);
            }
        }
    }
    public virtual void RangeToDoAttack(Entity target)
    {
        float dist = Vector3.Distance(target.transform.position, transform.position);

        if (dist <= rangeDetect)
        {
            DoAttack(target);
        }
      
    }

    
    protected virtual bool DoAttack(Entity targetEntity)
    {
        // On verifie si l'entity est valide
        if(targetEntity.IsValidEntity())
        {           
            // On applique les degats
            targetEntity.DamageEntity(damageAttack);

            //On instancie la bullet          
            GameObject bullet = PoolManager.Instance.GetElement(prefabBulletPlayer);          
            bullet.GetComponent<Rigidbody>();
           
            bullet.transform.position = transform.position;
            bullet.SetActive(true);
            m_CanAttack = false;
            m_CurrentTimeBeforeNextAttack = 0;

            SoundManager.Instance.PlayOneShotGlobalSound();
            return true;
        }
        return false;
    }
}
