using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//type de variable indiquant quel état est le combat
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class battleSystem : MonoBehaviour
{
    //etat actuel du combat
    public BattleState currentBattleState;

    public int nbPlayer;
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

    //données positionnel pour spawn les entités aux bons endroits
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
    public unit[] playerUnitList;
    public battleHud[] playerHudsList;

    public GameObject[] enemyGOList;
    public unit[] enemyUnitList;
    public battleHud[] enemyHudsList;

    //Tableau de toutes les unités dans le cadre du turn order
    public GameObject[] turnOrderGOList;

    //compteur pour les tableaux 
    int count;

    //compteur pour le turn order
    public int currentTurnOrder;

    // Start is called before the first frame update
    void Start()
    {
        //on set le combat en start puis on lance la fonciton setup du combat
        currentBattleState = BattleState.START;
        battleStart();
        //On génere l'ordre des tours. Plus tard pourra être croisé avec une eventuelle stats de vitesse
        turnGeneration();
        currentBattleState = BattleState.PLAYERTURN;
        currentTurnOrder = -1;
        newTurn();
    }

    //setup du combat, sert à créer tous nos tableaux et à les remplires des valeurs de chaques entités
    void battleStart()
    {
        //Spawing des joueur et on remplie l'array des GO représentant chaque joueur
        playerGOList = new GameObject[nbPlayer];
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
        playerUnitList = new unit[nbPlayer];
        playerHudsList = new battleHud[nbPlayer];
        count = 0;
        foreach (GameObject currentPlayerGO in playerGOList)
        {
            playerUnitList[count] = currentPlayerGO.GetComponent<unit>();
            playerHudsList[count] = currentPlayerGO.GetComponent<battleHud>();
            playerHudsList[count].battleHudSetup(playerUnitList[count]);
            count++;
        }

        //pareil pour les méchants
        enemyUnitList = new unit[nbEnemy];
        enemyHudsList = new battleHud[nbEnemy];
        count = 0;
        foreach (GameObject currentEnemyGO in enemyGOList)
        {
            enemyUnitList[count] = currentEnemyGO.GetComponent<unit>();
            enemyHudsList[count] = currentEnemyGO.GetComponent<battleHud>();
            enemyHudsList[count].battleHudSetup(enemyUnitList[count]);
            count++;
        }
    }

    //Genere le turn order
    void turnGeneration()
    {
        //On genere la taille du tableau en fonction du nb d'enemies
        turnOrderGOList = new GameObject[3 + nbEnemy];
        
        //et on remplie des units joueurs
        count = 0;
        foreach (GameObject playerGO in playerGOList)
        {
            turnOrderGOList[count] = playerGO;
            count++;
        }
        //puis des enemis, besoin de ne pas reset le count
        foreach (GameObject enemyGO in enemyGOList)
        {
            turnOrderGOList[count] = enemyGO;
            count++;
        }
    }

    //Routine quand un nouveau tour commence, gere aussi le pointeur indiquant à qui s'est le tour
    void newTurn()
    {
        if (currentTurnOrder >= 0)
            turnOrderGOList[currentTurnOrder].GetComponent<battleHud>().turnEnd();
        currentTurnOrder++;

        if (currentTurnOrder >= nbPlayer + nbEnemy)
            currentTurnOrder = 0;

        turnOrderGOList[currentTurnOrder].GetComponent<battleHud>().turnActive();

        //change l'état du combat en fonction de quel entité joue ce tour
        currentBattleState = BattleState.PLAYERTURN;
        if ( turnOrderGOList[currentTurnOrder].GetComponent<unit>().thisUnitType == unitType.MECHANT )
            currentBattleState = BattleState.ENEMYTURN;

    }

    //Test pour btn atk simple, elle attaque tous les méchants 
    public void seFaitClicker()
    {
        count = 0;
        foreach (unit currentEnemyUnit in enemyUnitList)
        {
            currentEnemyUnit.takeDamage(playerUnitList[1].power);
            enemyHudsList[count].changeHP(currentEnemyUnit.currentHP);
            count++;
        }
        newTurn();
    }
}
