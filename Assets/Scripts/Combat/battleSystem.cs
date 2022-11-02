using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//type de variable indiquant quel état est le combat
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class battleSystem : MonoBehaviour
{
    //etat actuel du combat
    public BattleState currentBattleState;

    //nombre d'enemie pendant le combat, max 5 
    public int nbEnemy;

    //variables qui serviront pour créer les tableaux plus bas
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    //Plus tard, les prefabs dans les 5 var du dessous devront etre changeable dans un script dans la transition monde/combat
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;
    public GameObject enemyPrefab5;

    public Transform playerBattleStation;
    public Transform playerBattleStation2;
    public Transform playerBattleStation3;
    public Transform enemyBattleStation;
    public Transform enemyBattleStation2;
    public Transform enemyBattleStation3;
    public Transform enemyBattleStation4;
    public Transform enemyBattleStation5;

    //Tableaux de toutes les entités du combat
    public GameObject[] playerGOList;
    public units[] playerUnitsList;
    public battleHud[] playerHudsList;

    public GameObject[] enemyGOList;
    public units[] enemyUnitsList;
    public battleHud[] enemyHudsList;

    //compteur pour les tableaux 
    int count;


    // Start is called before the first frame update
    void Start()
    {
        //on set le combat en start puis on lance la fonciton setup du combat
        currentBattleState = BattleState.START;
        battleStart();
        //placeholder
        currentBattleState = BattleState.PLAYERTURN;
    }

    //setup du combat, sert à créer tous nos tableaux et à les remplires des valeurs de chaques entités
    void battleStart()
    {
        //Spawing des joueur et on remplie l'array des GO représentant chaque joueur
        playerGOList = new GameObject[3];
        playerGOList[0] = Instantiate(playerPrefab1, playerBattleStation);
        playerGOList[1] = Instantiate(playerPrefab2, playerBattleStation2);
        playerGOList[2] = Instantiate(playerPrefab3, playerBattleStation3);

        //Pareil avec les enemies, adaptatif en fonction du nb d'enemy qui pop
        enemyGOList = new GameObject[nbEnemy];
        enemyGOList[0] = Instantiate(enemyPrefab1, enemyBattleStation);
        if (nbEnemy >= 2)
            enemyGOList[1] = Instantiate(enemyPrefab2, enemyBattleStation2);
        if (nbEnemy >= 3)
            enemyGOList[2] = Instantiate(enemyPrefab3, enemyBattleStation3);
        if (nbEnemy >= 4)
            enemyGOList[3] = Instantiate(enemyPrefab4, enemyBattleStation4);
        if (nbEnemy >= 5)
            enemyGOList[4] = Instantiate(enemyPrefab5, enemyBattleStation5);

        //On créer la liste des stats liés aux joueurs dans l'array prévu, également on initie leur huds
        playerUnitsList = new units[3];
        playerHudsList = new battleHud[3];
        count = 0;
        foreach (GameObject currentPlayerGO in playerGOList)
        {
            playerUnitsList[count] = currentPlayerGO.GetComponent<units>();
            playerHudsList[count] = currentPlayerGO.GetComponent<battleHud>();
            playerHudsList[count].battleHudSetup(playerUnitsList[count]);
            count++;
        }

        //pareil pour les méchants
        enemyUnitsList = new units[nbEnemy];
        enemyHudsList = new battleHud[nbEnemy];
        count = 0;
        foreach (GameObject currentEnemyGO in enemyGOList)
        {
            enemyUnitsList[count] = currentEnemyGO.GetComponent<units>();
            enemyHudsList[count] = currentEnemyGO.GetComponent<battleHud>();
            enemyHudsList[count].battleHudSetup(enemyUnitsList[count]);
            count++;
        }
    }

    //Test pour btn atk simple, elle attaque tous les méchants 
    public void seFaitClicker()
    {
        count = 0;
        foreach (units currentEnemyUnits in enemyUnitsList)
        {
            currentEnemyUnits.takeDamage(playerUnitsList[1].power);
            enemyHudsList[count].changeHP(currentEnemyUnits.currentHP);
            count++;
        }
    }
}
