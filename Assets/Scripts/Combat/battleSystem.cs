using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//type de variable indiquant quel état est le combat
public enum BattleState { START, PLAYERTURN, FUCHOICE, ENEMYTURN, WON, LOST }

//Type de team
public enum TeamType { PLAYERTEAM, ENEMYTEAM }

//Pour des raisons de prototypage, les skills seront tous répertoriés dans cette enumération. Plus tard il est possible que le foncitonnement change.
public enum UnitSkills { NONE, ATKMONO, ATKAOE, ATKDOT, HEAL, BUFF, DEBUFF, ATKCON }

//public enum skillType { ACTION, FOLLOWUP }

//Enumération afin de savoir ou en est le joueur dans une suite action/follow up complète. FU = Follow Up
public enum actionStep { NONE, SKILLCHOICE, SKILLTARGET, FUCASTER, FUCHOICE, FUTARGET, SKILLRESOLVE, FURESOLVE, RESET }



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

    //variable pour garder en mémoire quelle attaque va être utilisée & quel 
    //public UnitSkills currentSkill; old, à supprimer later
    public UnitSkills currentAction;
    public UnitSkills currentFollowUp;

    public int skillTarget;
    public int followupTarget;
    public int followupCaster;

    public actionStep playerTSU; //TSU = Turn Set Up, utilisé pour savoir ou on en est entre le choix d'un action, d'un FU ou d'une target par ex
    //Tableau de tous les skills
    public UnitSkills[] allSkills;

    // Start is called before the first frame update
    void Start()
    {
        //on set le combat en start puis on lance la fonciton setup du combat
        currentBattleState = BattleState.START;
        battleStart();
        //On génere l'ordre des tours. Plus tard pourra être croisé avec une eventuelle stats de vitesse
        turnGeneration();
        currentBattleState = BattleState.PLAYERTURN;
        playerTSU = actionStep.NONE;
        currentTurnOrder = -1;
        currentAction = UnitSkills.NONE;
        currentFollowUp = UnitSkills.NONE;
        skillTarget = -1;
        followupTarget = -1;
        followupCaster = -1;
        skillsSetup();
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

    //Setup les différentes compétences dans le tableau de compétences
    void skillsSetup()
    {
        allSkills = new UnitSkills[8];
        allSkills[0] = UnitSkills.NONE;
        allSkills[1] = UnitSkills.ATKMONO;
        allSkills[2] = UnitSkills.ATKAOE;
        allSkills[3] = UnitSkills.ATKDOT;
        allSkills[4] = UnitSkills.HEAL;
        allSkills[5] = UnitSkills.BUFF;
        allSkills[6] = UnitSkills.DEBUFF;
        allSkills[7] = UnitSkills.ATKCON;
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

    //Routine quand un nouveau tour commence, gere aussi le pointeur indiquant à qui s'est le tour et skip en cas de unité morte
    void newTurn()
    {
        //Test victoires
        if (isPlayersDead()) { 
            sceneChanger(TeamType.ENEMYTEAM);
            return;
        }

        //Test defaite
        if (isEnemiesDead())
            sceneChanger(TeamType.PLAYERTEAM);

        //déselectionne le personnage qui était au commande avant, sauf au premier tour
        if (currentTurnOrder >= 0)
            turnOrderGOList[currentTurnOrder].GetComponent<battleHud>().turnEnd();

        //On avance d'1 tour
        currentTurnOrder++;

        //Reset le tour en haut de la liste si le dernier perso à joyé
        if (currentTurnOrder >= nbPlayer + nbEnemy)
            currentTurnOrder = 0;

        //test si perso actuel mort, auquel cas on passera au suivant en nouveau tour
        if (turnOrderGOList[currentTurnOrder].activeSelf)
        {
            //montre le personnage actuel
            turnOrderGOList[currentTurnOrder].GetComponent<battleHud>().turnActive();
            //change l'état du combat en fonction de quel entité joue ce tour
            
            if (turnOrderGOList[currentTurnOrder].GetComponent<unit>().thisUnitType == unitType.MECHANT)
            { 
                currentBattleState = BattleState.ENEMYTURN;
                enemyTurn();
            }else
            {
                currentBattleState = BattleState.PLAYERTURN;
                playerTSU = actionStep.SKILLCHOICE;
            }
        }
        else 
        {
            newTurn();
        }
    }

    //Routine de test afin de savoir si il y a une team victorieuse ou non
    bool isPlayersDead()
    {
        count = 0;
        foreach (GameObject playerGO in playerGOList)
        {
            if (!playerGO.activeSelf)
                count++;
        }
        if (count >= nbPlayer)
            return true;
        return false;
    }
    bool isEnemiesDead()
    {
        count = 0;
        foreach (GameObject enemyGO in enemyGOList)
        {
            if (!enemyGO.activeSelf)
                count++;
        }
        if (count >= nbEnemy)
            return true;
        return false;

    }

    //IA simple des méchants, qui attaquera le joueur le plus en haut de l'écran encore en vie
    void enemyTurn()
    {
        bool didntAtk = true;
        foreach(unit targetedPlayer in playerUnitList)
        {
            if (didntAtk && targetedPlayer.isAlive())
            {
                targetedPlayer.takeDamage(turnOrderGOList[currentTurnOrder].GetComponent<unit>().power);
                didntAtk = false;
            }
        }
        newTurn();
    }

    //Changement de scene et actions en fonciton de qui qui a gagné.
    public void sceneChanger(TeamType winnerTeam)
    {
        if (winnerTeam == TeamType.PLAYERTEAM)
            SceneManager.LoadScene(0);//reprise du monde
            

        if (winnerTeam == TeamType.ENEMYTEAM)
            SceneManager.LoadScene(3);//c'est pour le moment la scene game over

    }


    
    //Les 4 fonctions serves d'UI.
    public void showSelectPanelEnemy()
    {
        //unHighlight tous les enemies clickables
        count = 0;
        foreach (battleHud currentEnemyHud in enemyHudsList)
        {
            if (enemyGOList[count].activeSelf)
            {
                currentEnemyHud.selectableActive();
            }
            count++;
        }
    }
    public void hideSelectPanelEnemy()
    {
        //unHighlight tous les enemies clickables
        count = 0;
        foreach (battleHud currentEnemyHud in enemyHudsList)
        {
            if (enemyGOList[count].activeSelf)
            {
                currentEnemyHud.selectableDesactive();
            }
            count++;
        }
    }

    public void showSelectPanelPlayer()
    {
        //unHighlight tous les enemies clickables
        count = 0;
        foreach (battleHud currentPlayerHud in playerHudsList)
        {
            if (enemyGOList[count].activeSelf)
            {
                currentPlayerHud.selectableActive();
            }
            count++;
        }
    }
    public void hideSelectPanelPlayer()
    {
        //unHighlight tous les enemies clickables
        count = 0;
        foreach (battleHud currentPlayerHud in playerHudsList)
        {
            if (enemyGOList[count].activeSelf)
            {
                currentPlayerHud.selectableDesactive();
            }
            count++;
        }
    }
    //Fin des 4 fonctions


    //Quand joueur appuis sur un btn compétence, 
    public void selectSkill(int selectedSkillInput)
    {
        if(playerTSU == actionStep.SKILLCHOICE)
        {
            currentAction = allSkills[selectedSkillInput];
            playerTSU = actionStep.SKILLTARGET;

            showSelectPanelEnemy();
            return;
        }
        if (playerTSU == actionStep.FUCHOICE)
        {
            currentFollowUp = allSkills[selectedSkillInput];
            playerTSU = actionStep.FUTARGET;
            
            hideSelectPanelPlayer();
            showSelectPanelEnemy();
            return;
        }
        Debug.Log("NON! - selectSkill");
    }

    public void selectTarget(int selectedTargetInput)
    {
        if (playerTSU == actionStep.SKILLTARGET)
        {
            skillTarget = selectedTargetInput;
            playerTSU = actionStep.FUCASTER;

            hideSelectPanelEnemy();
            showSelectPanelPlayer();
            return;
        }
        if (playerTSU == actionStep.FUCASTER)
        {
            followupCaster = selectedTargetInput;
            playerTSU = actionStep.FUCHOICE;

            hideSelectPanelPlayer();
            turnOrderGOList[currentTurnOrder].GetComponent<battleHud>().unHighlightPanel();
            playerHudsList[followupCaster].doHighlightPanel();
            return;
        }
        if (playerTSU == actionStep.FUTARGET)
        {
            followupTarget = selectedTargetInput;
            playerTSU = actionStep.SKILLRESOLVE;
            
            hideSelectPanelEnemy();
            actionResolve();
            return;
        }
        Debug.Log("NON! - selectTarget");
    }

    void actionResolve()
    {   
        myTestAction();
        playerTSU = actionStep.FURESOLVE;
        myTestAction();
        playerTSU = actionStep.NONE;
        actionEnding();

    }

    void myTestAction()
    {
        if (playerTSU == actionStep.SKILLRESOLVE)
        {
            Debug.Log("Bonjour! Tu as lancé la ctps!");
            if (currentAction == UnitSkills.ATKMONO)
                enemyUnitList[skillTarget].takeDamage(turnOrderGOList[currentTurnOrder].GetComponent<hero>().skill1(playerTSU)); 
            return;
        }
        if (playerTSU == actionStep.FURESOLVE)
        {
            Debug.Log("Aurevoir! Tu as lancé le FU!");
            if (currentAction == UnitSkills.ATKMONO)
                enemyUnitList[followupTarget].takeDamage(playerGOList[followupCaster].GetComponent<hero>().skill1(playerTSU));
            return;
        }

    }

    void actionEnding()
    {
        Debug.Log("Fin de l'action.");
        playerHudsList[followupCaster].unHighlightPanel();
        currentAction = UnitSkills.NONE;
        currentFollowUp = UnitSkills.NONE;
        skillTarget = -1;
        followupCaster = -1;
        followupTarget = -1;
        newTurn();
    }

    /*
    public void atkAoe()
    {

        if (currentBattleState == BattleState.PLAYERTURN)
        {
            count = 0;
            foreach (unit currentEnemyUnit in enemyUnitList)
            {
                currentEnemyUnit.takeDamage(playerUnitList[1].power);
                count++;
            }
            newTurn();
        }
    }
    */
}
