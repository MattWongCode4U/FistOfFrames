using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RESULT { P1WIN, P2WIN, TIE, CONTINUE };

public class GameMaster : MonoBehaviour
{
    public UIHandler ui;
    public GameAudioManager gameAudioManager;
    public GameObject gameOverGO;
    public Transform[] positions;

    public GameObject p1go, p2go;
    Player Player1, Player2;
    PlayerAI P2AI;

    List<ACTIONS> p1Actions;
    List<ACTIONS> p2Actions;

    bool p1ready;
    bool p2ready;

    int defaultPlayer1Pos = 2;
    int defaultPlayer2Pos = 4;

    public float delayBetweenFrames = 1f;

    int roundCounter;
    bool roundOver;
    int p1roundsW;
    int p2roundsW;
    int roundsToWin = 2;

    bool allowInput, gameover;

    public bool vsAI;

    private void Awake()
    {
        //See which game mode it is
        if(MainMenu.GameMode == "vsAI")
        {
            vsAI = true;
        } else if (MainMenu.GameMode == "offPVP")
        {
            vsAI = false;
        } else if (MainMenu.GameMode == "onPVP") //potential online pvp setup for future
        {
            vsAI = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverGO.SetActive(false);
        allowInput = true;
        gameover = false;

        Player1 = p1go.GetComponent<Player>();
        Player2 = p2go.GetComponent<Player>();
        P2AI = p2go.GetComponent<PlayerAI>();

        roundCounter = 1;
        p1roundsW = 0;
        p2roundsW = 0;

        roundOver = false;

        setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowInput || gameover)
        {
            return;
        }
        
        //PLAYER 1 ACTIONS
        if (!p1ready)
        {
            bool p1APChange = false;
            //MOVELEFT
            if (Input.GetKeyDown(KeyCode.A) && Player1.canAction(ACTIONS.MOVELEFT)) 
            {
                addP1Queue(ACTIONS.MOVELEFT);
                Player1.useAP(ACTIONS.MOVELEFT);
                p1APChange = true;
            }
            //MOVERIGHT
            if (Input.GetKeyDown(KeyCode.D) && Player1.canAction(ACTIONS.MOVERIGHT))
            {
                addP1Queue(ACTIONS.MOVERIGHT);
                Player1.useAP(ACTIONS.MOVERIGHT);
                p1APChange = true;
            }
            //WAIT
            if (Input.GetKeyDown(KeyCode.S) && Player1.actionPoints > 0)
            {
                addP1Queue(ACTIONS.WAIT);
            }
            //PUNCH
            if (Input.GetKeyDown(KeyCode.E) && Player1.canAction(ACTIONS.PUNCH))
            {
                addP1Queue(ACTIONS.WINDUP);
                addP1Queue(ACTIONS.PUNCH);
                Player1.useAP(ACTIONS.PUNCH);
                p1APChange = true;
            }
            //BLOCK
            if (Input.GetKeyDown(KeyCode.F) && Player1.canAction(ACTIONS.BLOCK))
            {
                addP1Queue(ACTIONS.BLOCK);
                Player1.useAP(ACTIONS.BLOCK);
                p1APChange = true;
            }
            //UNDO
            if (Input.GetKeyDown(KeyCode.R))
            {
                undo(USER.PLAYER1);
                p1APChange = true;
            }

            //Update Action limit UI only if there is a change
            if (p1APChange)
            {
                ui.updateActionLimit(Player1, Player2);
                p1APChange = false;
            }
        }
        //P1 READY
        if (Input.GetKeyDown(KeyCode.V))
        {
            p1ready = !p1ready;
            ui.updateReadys(p1ready, p2ready);
        }

        //PLAYER 2 is a person
        if (vsAI == false) 
        {
            //PLAYER 2 ACTIONS
            if (!p2ready)
            {
                bool p2APChange = false;
                //MOVELEFT
                if (Input.GetKeyDown(KeyCode.J) && Player2.canAction(ACTIONS.MOVELEFT))
                {
                    addP2Queue(ACTIONS.MOVELEFT);
                    Player2.useAP(ACTIONS.MOVELEFT);
                    p2APChange = true;
                }
                //MOVERIGHT
                if (Input.GetKeyDown(KeyCode.L) && Player2.canAction(ACTIONS.MOVERIGHT))
                {
                    addP2Queue(ACTIONS.MOVERIGHT);
                    Player2.useAP(ACTIONS.MOVERIGHT);
                    p2APChange = true;
                }
                //WAIT
                if (Input.GetKeyDown(KeyCode.K) && Player2.actionPoints > 0)
                {
                    addP2Queue(ACTIONS.WAIT);
                }
                //PUNCH
                if (Input.GetKeyDown(KeyCode.U) && Player2.canAction(ACTIONS.PUNCH))
                {
                    addP2Queue(ACTIONS.WINDUP);
                    addP2Queue(ACTIONS.PUNCH);
                    Player2.useAP(ACTIONS.PUNCH);
                    p2APChange = true;
                }
                //BLOCK
                if (Input.GetKeyDown(KeyCode.H) && Player2.canAction(ACTIONS.BLOCK))
                {
                    addP2Queue(ACTIONS.BLOCK);
                    Player2.useAP(ACTIONS.BLOCK);
                    p2APChange = true;
                }
                //UNDO
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    undo(USER.PLAYER2);
                    p2APChange = true;
                }

                //Update Action limit UI only if there is a change
                if (p2APChange)
                {
                    ui.updateActionLimit(Player1, Player2);
                    p2APChange = false;
                }
            }
            //P2 READY
            if (Input.GetKeyDown(KeyCode.B))
            {
                p2ready = !p2ready;
                ui.updateReadys(p1ready, p2ready);
            }
        } else //PLAYER 2 is AI
        {
            if (!p2ready)
            {
                p2Actions = P2AI.genActions();
                p2ready = true;
                ui.updateReadys(p1ready, p2ready);
                ui.updateActionLimit(Player1, Player2);
            }
        }

        //BOTH PLAYERS READY
        if (p1ready && p2ready)
        {
            allowInput = false;
            pruneQueues();
            eqaulizeQueues();
            StartCoroutine(parseQueues());
        }

        if (roundOver)
        {
            Debug.Log("ROUND OVER");
            setup();
        }
    }

    //Setup variables for new round
    void setup()
    {
        Debug.Log("Round: " + roundCounter + "\nSCORE P1: " + p1roundsW + " - P2: " + p2roundsW);
        p1Actions = new List<ACTIONS>();
        p2Actions = new List<ACTIONS>();
        p1ready = false;
        p2ready = false;

        //Default starting positions
        Player1.pos = defaultPlayer1Pos;
        Player2.pos = defaultPlayer2Pos;
        Player1.snapTo(positions[defaultPlayer1Pos]);
        Player2.snapTo(positions[defaultPlayer2Pos]);
        //Default player variables reset
        Player1.reset();
        Player2.reset();

        roundOver = false;
        allowInput = true;

        ui.clearActionQueues();
        ui.updateUI(Player1, Player2, roundCounter, p1roundsW, p2roundsW, p1ready, p2ready);
    }

    //Add action to P1 Queue
    public void addP1Queue(ACTIONS action)
    {
        p1Actions.Add(action);
    }

    //Add action to P2 Queue
    public void addP2Queue(ACTIONS action)
    {
        p2Actions.Add(action);
    }

    //Remove ending WAIT actions from the queues to minimize no actions
    void pruneQueues()
    {
        while(p1Actions.Count >= 1 && p1Actions[p1Actions.Count-1] == ACTIONS.WAIT)
        {
            p1Actions.RemoveAt(p1Actions.Count - 1);
        }

        while (p2Actions.Count >= 1 && p2Actions[p2Actions.Count-1] == ACTIONS.WAIT)
        {
            p2Actions.RemoveAt(p2Actions.Count - 1);
        }
    }

    //Make queues the same size by padding waits at the end
    void eqaulizeQueues()
    {
        while(p1Actions.Count > p2Actions.Count)
        {
            p2Actions.Add(ACTIONS.WAIT);
        }
        while (p2Actions.Count > p1Actions.Count)
        {
            p1Actions.Add(ACTIONS.WAIT);
        }
    }

    //Begin actioning both queues with delay between actions. Queues are pruned and equalized before this
    IEnumerator parseQueues()
    {
        //Debugging outputs
        string out1 = "";
        string out2 = "";
        foreach(ACTIONS a in p1Actions)
        {
            out1 += a.ToString() + ", ";
        }
        foreach (ACTIONS a in p2Actions)
        {
            out2 += a.ToString() + ", ";
        }
        Debug.Log("p1: " + out1);
        Debug.Log("p2: " + out2);

        //Parse all actions
        for (int i = 0; i < p1Actions.Count; i++)
        {
            if (roundOver || gameover)
            {
                break;
            }
            handleSuperPunch(i, p1Actions[i], p2Actions[i]);
            compareActions(p1Actions[i], p2Actions[i]);
            ui.displayActions(p1Actions[i], p2Actions[i]);
            handleStuns(i);
            ui.updateUILoop(Player1, Player2, p1ready, p2ready);
            clearStatuses();
            yield return new WaitForSeconds(delayBetweenFrames);
            Player1.resetAnimTriggers();
            Player2.resetAnimTriggers();
        }

        //Reset variables for new actions
        Player1.idleAnimReset();
        Player2.idleAnimReset();
        p1Actions = new List<ACTIONS>();
        p2Actions = new List<ACTIONS>();
        ui.clearActionQueues();
        Player1.resetAP();
        Player2.resetAP();
        p1ready = false;
        p2ready = false;
        ui.updateUI(Player1, Player2, roundCounter, p1roundsW, p2roundsW, p1ready, p2ready);
        allowInput = true;
    }

    //Determine which actions by which player
    public void parseAction(ACTIONS a, USER u)
    {
        if (a == ACTIONS.MOVELEFT)
        {
            moveLeft(u);
        }
        if (a == ACTIONS.MOVERIGHT)
        {
            moveRight(u);
        }
        if(a == ACTIONS.WAIT)
        {

        }
        if(a == ACTIONS.WINDUP)
        {
            windup(u);
        }
        if(a == ACTIONS.PUNCH)
        {
            punch(u);
        }
        if(a == ACTIONS.BLOCK)
        {
            block(u);
        }
        if(a == ACTIONS.STUN)
        {
            stun(u);
        }
    }

    //Handle order of actions and handle different cases
    public void compareActions(ACTIONS a1, ACTIONS a2)
    {
        //Punching vs Moving
        if(a1 == ACTIONS.PUNCH && (a2 == ACTIONS.MOVELEFT || a2 == ACTIONS.MOVERIGHT))
        {
            parseAction(a2, USER.PLAYER2);
            parseAction(a1, USER.PLAYER1);
        }
        else if (a2 == ACTIONS.PUNCH && (a1 == ACTIONS.MOVELEFT || a1 == ACTIONS.MOVERIGHT))
        {
            parseAction(a1, USER.PLAYER1);
            parseAction(a2, USER.PLAYER2);
        }

        //Moving into each other = do not move either (bump into each other then back to origin)
        else if(distanceBetween() <= 2 && a1 == ACTIONS.MOVERIGHT && a2 == ACTIONS.MOVELEFT)
        {
            parseAction(ACTIONS.WAIT, USER.PLAYER1);
            parseAction(ACTIONS.WAIT, USER.PLAYER2);
        }

        //Both going right, handle timing error
        else if(a1 == ACTIONS.MOVERIGHT && a2 == ACTIONS.MOVERIGHT)
        {
            parseAction(a2, USER.PLAYER2);
            parseAction(a1, USER.PLAYER1);
        }

        //Block vs Attack
        else if(a1 == ACTIONS.BLOCK && a2 == ACTIONS.PUNCH)
        {
            parseAction(a1, USER.PLAYER1);
            parseAction(a2, USER.PLAYER2);
        }
        else if (a2 == ACTIONS.BLOCK && a1 == ACTIONS.PUNCH)
        {
            parseAction(a2, USER.PLAYER2);
            parseAction(a1, USER.PLAYER1);
        }

        //Default order
        else
        {
            parseAction(a1, USER.PLAYER1);
            parseAction(a2, USER.PLAYER2);
        }

        //Check deaths if someone punched (Change to when someone takes damage?)
        if (a1 == ACTIONS.PUNCH || a2 == ACTIONS.PUNCH)
        {
            checkDeaths();
        }
    }

    //Get Player reference from USER u variable
    Player getPlayer(USER u)
    {
        if (u == USER.PLAYER1)
        {
            return Player1;
        } else
        {
            return Player2;
        }
    }

    //Get other Player reference from the currUser variable
    Player getOtherPlayer(USER currUser)
    {
        if (currUser == USER.PLAYER1)
        {
            return Player2;
        }
        else
        {
            return Player1;
        }
    }

    //Get other Player refs' position
    int getOtherPlayerPos(USER currUser)
    {
        return getOtherPlayer(currUser).pos;
    }

    //Move player left
    public void moveLeft(USER u)
    {
        Player p = getPlayer(u);
        Player otherP = getOtherPlayer(u);

        p.runAnim();

        if (p.pos > 0)
        {
            if(p.pos -1 == otherP.pos)
            {
                return;
            }
            p.pos--;
            p.moveTo(positions[p.pos]);
            gameAudioManager.playMoveSFX();
        }
    }

    //Move player right
    public void moveRight(USER u)
    {
        Player p = getPlayer(u);
        Player otherP = getOtherPlayer(u);

        p.runAnim();

        if (p.pos < positions.Length - 1)
        {
            if (p.pos + 1 == otherP.pos)
            {
                return;
            }
            p.pos++;
            p.moveTo(positions[p.pos]);
            gameAudioManager.playMoveSFX();
        }
    }

    //Windup action of player
    public void windup(USER u)
    {
        Player p = getPlayer(u);
        p.windupAnim();
    }

    //Punch space in front of player
    public void punch(USER u)
    {
        Player p = getPlayer(u);
        Player otherP = getOtherPlayer(u);
        if (p.superPunch)
        {
            p.superPunchAnim();
            p.disableSuperPunch();
        }
        else
        {
            p.punchAnim();
        }

        bool hitting = false;
        if (u == USER.PLAYER1)
        {
            if (p.pos + 1 == otherP.pos)
            {
                hitting = true;
            }
        }
        else
        {
            if (p.pos - 1 == otherP.pos)
            {
                hitting = true;
            }
        }
        if (hitting)
        {
            if (otherP.blocking)
            {
                p.stunned = true;
                otherP.enableSuperPunch();
                gameAudioManager.playBlockSFX();
            } else
            {
                otherP.takeDamage(p.punchDamage);
                otherP.damageLight();
                gameAudioManager.playHitSFX();
            }
        }
    }

    //Set user to blocking status
    public void block(USER u)
    {
        Player p = getPlayer(u);
        p.blocking = true;
        p.blockAnim();
    }

    //Undo action takes out the last action and refunds points
    void undo(USER u)
    {
        Player p = getPlayer(u);
        List<ACTIONS> l;
        if (u == USER.PLAYER1)
        {
            l = p1Actions;
        }
        else
        {
            l = p2Actions;
        }

        //Not empty list check
        if (l.Count - 1 < 0)
        {
            return;
        }

        //Refund Action Points based on which action was undone
        ACTIONS a = l[l.Count - 1];
        p.refundAP(a);

        //Remove the last action
        l.RemoveAt(l.Count - 1);
        if(a == ACTIONS.PUNCH) //Remove again for windup
        {
            l.RemoveAt(l.Count - 1);
        }
    }

    void stun(USER u)
    {
        Player p = getPlayer(u);
        p.stunAnim();
        gameAudioManager.playStunSFX();
    }

    //Calculate distance between the players
    public int distanceBetween()
    {
        return Player2.pos - Player1.pos;
    }

    //Check if a player has died
    void checkDeaths()
    {
        if (Player1.health <= 0 || Player2.health <= 0) //someone dead
        {
            roundOver = true;
            roundCounter++;

            //Death animations
            if (Player1.health <= 0)
            {
                Player1.deathAnim();
            }
            if (Player2.health <= 0)
            {
                Player2.deathAnim();
            }

            if (Player1.health <= 0 && Player2.health <= 0) //tie
            {
                Debug.Log("TIE");
                ui.roundEnd("TIE");
            }
            else if (Player1.health <= 0) //P1 dead
            {
                p2roundsW++;
                Debug.Log("P2 Round Win");
                if (!checkP2Win())
                {
                    ui.roundEnd("P2 Scene Win");
                }
            }
            else if (Player2.health <= 0) //P2 dead
            {
                p1roundsW++;
                Debug.Log("P1 Round Win");
                if (!checkP1Win())
                {
                    ui.roundEnd("P1 Scene Win");
                }
            }
        }
        else //no round win
        {
            return;
        }
    }

    //Check if a player 1 has won enough rounds to win game
    bool checkP1Win()
    {
        if (p1roundsW >= roundsToWin)
        {
            gameover = true;
            //P1 Wins game
            Debug.Log("P1 Wins");
            StartCoroutine(gameOver("Player 1"));
            return true;
        }
        return false;
    }

    //Check if a player 2 has won enough rounds to win game
    bool checkP2Win()
    {
        if (p2roundsW >= roundsToWin)
        {
            gameover = true;
            //P2 Wins game
            Debug.Log("P2 Wins");
            StartCoroutine(gameOver("Player 2"));
            return true;
        }
        return false;
    }

    //Clear statuses for both players
    void clearStatuses()
    {
        Player1.blocking = false;
        Player2.blocking = false;
        Player1.stunned = false;
        Player2.stunned = false;
    }

    //Inserts a STUN action after the given index in player queues
    void handleStuns(int index) 
    {
        if (Player1.stunned)
        {
            p1Actions.Insert(index +1, ACTIONS.STUN);
        }
        if (Player2.stunned)
        {
            p2Actions.Insert(index +1, ACTIONS.STUN);
        }
        eqaulizeQueues();
    }

    //Removes WINDUP action if super punch is available
    void handleSuperPunch(int index, ACTIONS a1, ACTIONS a2)
    {
        bool change = false;
        if (Player1.superPunch && a1 == ACTIONS.WINDUP)
        {
            p1Actions.RemoveAt(index);
            change = true;
        }
        if (Player2.superPunch && a2 == ACTIONS.WINDUP)
        {
            p2Actions.RemoveAt(index);
            change = true;
        }
        
        if (change)
        {
            pruneQueues();
            eqaulizeQueues();
        }
    }

    //Shows the gameover screen with the winning player after a delay 
    IEnumerator gameOver(string playerWon)
    {
        ui.updateGameOverPlayerText(playerWon);
        yield return new WaitForSeconds(1);
        gameOverGO.SetActive(true);
    }
}
