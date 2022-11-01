using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;

    public Transform playerBattleStation;
    public Transform playerBattleStation2;
    public Transform playerBattleStation3;

    public Transform enemyBattleStation;
    public Transform enemyBattleStation2;
    public Transform enemyBattleStation3;


    // Start is called before the first frame update
    void Start()
    {
        battleStart();
    }

    // Update is called once per frame
    void battleStart()
    {
        GameObject playerGO1 = Instantiate(playerPrefab, playerBattleStation);
        GameObject playerGO2 = Instantiate(playerPrefab, playerBattleStation2);
        GameObject playerGO3 = Instantiate(playerPrefab, playerBattleStation3);
        GameObject enemyGO1 = Instantiate(enemyPrefab1, enemyBattleStation);
        GameObject enemyGO2 = Instantiate(enemyPrefab2, enemyBattleStation2);
        GameObject enemyGO3 = Instantiate(enemyPrefab2, enemyBattleStation3);
    }
}
