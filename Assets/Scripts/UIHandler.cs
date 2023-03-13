using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI p1hp;
    public TextMeshProUGUI p2hp;
    public TextMeshProUGUI roundsText;
    public TextMeshProUGUI p1WRounds;
    public TextMeshProUGUI p2WRounds;
    public TextMeshProUGUI p1Actions;
    public TextMeshProUGUI p2Actions;
    public TextMeshProUGUI p1Ready;
    public TextMeshProUGUI p2Ready;
    public TextMeshProUGUI p1ActionsQueue;
    public TextMeshProUGUI p2ActionsQueue;

    public TextMeshProUGUI gameOverPlayerWonText;
    public TextMeshProUGUI roundOverOverlayText;
    public GameObject roundOverOverlay;

    bool p1r = false;
    bool p2r = false;
    bool p1fadeIn = false;
    bool p2fadeIn = false;

    private void Update()
    {
        //Fading in and out ready text
        if (!p1r) {
            if (p1fadeIn) {
                fadeTextIn(p1Ready, 1);
                if (p1Ready.color.a >= 1)
                    p1fadeIn = false;
                
            } else
            {
                fadeTextOut(p1Ready, 1);
                if (p1Ready.color.a <= 0)
                    p1fadeIn = true;
            }
        }
        if (!p2r)
        {
            if (p2fadeIn)
            {
                fadeTextIn(p2Ready, 1);
                if (p2Ready.color.a >= 1)
                    p2fadeIn = false;

            }
            else
            {
                fadeTextOut(p2Ready, 1);
                if (p2Ready.color.a <= 0)
                    p2fadeIn = true;
            }
        }
    }

    //Update All UI
    public void updateUI(Player p1, Player p2, int roundsW, int p1W, int p2W, bool p1r, bool p2r)
    {
        updateHealths(p1.health, p2.health);
        updateRoundNumber(roundsW);
        updateRoundWins(p1W, p2W);
        updateActionLimit(p1, p2);
        updateReadys(p1r, p2r);
    }

    //Update Health amount UIs
    public void updateHealths(int p1, int p2)
    {
        //Debug.Log("DEBUG " + p1 + " " + p2);
        p1hp.text = "P1 HP: " + p1;
        p2hp.text = "P2 HP: " + p2;
    }

    //Update Round number UI
    public void updateRoundNumber(int r)
    {
        roundsText.text = "Round " + r;
    }

    //Update Round wins of both players' UIs
    public void updateRoundWins(int p1, int p2)
    {
        p1WRounds.text = "P1 Wins: " + p1;
        p2WRounds.text = "P2 Wins: " + p2;
    }

    //Update Action limit of both players' UI 
    public void updateActionLimit(Player p1, Player p2)
    {
        p1Actions.text = "P1 Actions: " + p1.actionPoints + "/" + p1.actionLimit;
        p2Actions.text = "P2 Actions: " + p2.actionPoints + "/" + p2.actionLimit;
    }

    //Update Player ready UIs 
    public void updateReadys(bool p1, bool p2)
    {
        if (p1)
        {
            p1Ready.color = Color.green;
            p1Ready.text = "P1 RDY";
            p1r = true;
        } else
        {
            p1Ready.color = Color.red;
            p1Ready.text = "P1 NOT RDY (V)";
            p1r = false;
        }
        if (p2)
        {
            p2Ready.color = Color.green;
            p2Ready.text = "P2 RDY";
            p2r = true;
        } else
        {
            p2Ready.color = Color.red;
            p2Ready.text = "P2 NOT RDY (B)";
            p2r = false;
        }
    }

    //Update Game Over text with correct player number
    public void updateGameOverPlayerText(string p)
    {
        gameOverPlayerWonText.text = "Game Over \n" + p + " Wins!";
    }

    //Update round over overlay text and call the timed show function
    public void roundEnd(string s)
    {
        roundOverOverlayText.text = s;
        StartCoroutine(showRound());
    }

    //Show the overlay for a second then turn it off
    IEnumerator showRound()
    {
        roundOverOverlay.SetActive(true);
        yield return new WaitForSeconds(1);
        roundOverOverlay.SetActive(false);
    }

    //Fade text out
    void fadeTextOut(TextMeshProUGUI text, float speed)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * speed);
    }

    //Fade text in
    void fadeTextIn(TextMeshProUGUI text, float speed)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + Time.deltaTime * speed);
    }

    //Add to action queue display UI text
    public void displayActions(ACTIONS a1, ACTIONS a2)
    {
        p1ActionsQueue.text += parseDisplayAction(a1) + " ";
        p2ActionsQueue.text += parseDisplayAction(a2) + " ";
    }

    //Determine what action to display from the given action
    string parseDisplayAction(ACTIONS a)
    {
        if (a == ACTIONS.MOVELEFT)
        {
            return "M.LEFT";
        } else if (a == ACTIONS.MOVERIGHT)
        {
            return "M.RIGHT";
        } else if (a == ACTIONS.WAIT)
        {
            return "WAIT";
        } else if(a == ACTIONS.WINDUP)
        {
            return "WINDUP";
        } else if(a == ACTIONS.PUNCH)
        {
            return "PUNCH";
        } else if(a == ACTIONS.BLOCK)
        {
            return "BLOCK";
        } else if(a == ACTIONS.STUN)
        {
            return "STUN";
        } else
        {
            return "";
        }
    }

    //Clear the action queue display UI
    public void clearActionQueues()
    {
        p1ActionsQueue.text = "";
        p2ActionsQueue.text = "";
    }
}
