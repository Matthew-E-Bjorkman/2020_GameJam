using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public List<GenericEnemy> EnemiesInGame = new List<GenericEnemy>();
    // Start is called before the first frame update
    void Start()
    {
        EnemiesInGame = ((GenericEnemy[]) FindObjectsOfType(typeof(GenericEnemy))).ToList();
    }

    public void ResetAllEnemies()
    {
        foreach (var enemy in EnemiesInGame)
        {
            enemy.gameObject.SetActive(true);
            enemy.Reset();
        }
    }

    public void RemoveAllEnemies()
    {
        foreach (var enemy in EnemiesInGame)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
