using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float timer;
    private float m_CurrentTimer = 100;
    public void Start()
    {
        EntityManager entityManager = FindObjectOfType<EntityManager>();
        if(entityManager != null)
        {
            entityManager.OnTowerDestroy += EndGame;
        }
    }
    private void Update()
    {
        UpdateTimer();
    }
    private void EndGame(Alignment alignment)
    {
        switch(alignment)
        {
            case Alignment.Player:
                Debug.Log("LOOOOOOOOOOSE ! GAME OVER !");
                break;
            case Alignment.IA:
                Debug.Log("WIN ! YOU'RE THE BEST");
                break;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public float Timer()
    {
        return timer;
    }
    public void UpdateTimer()
    {
        if(timer <= m_CurrentTimer)
        {
            timer += Time.deltaTime;
        }
        if(timer >= m_CurrentTimer)
        {
            EndGame(Alignment.Player);
        }
    }
}
