using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI roundsText;
    public TextMeshProUGUI p1Ready;
    public TextMeshProUGUI p2Ready;

    public TextMeshProUGUI gameOverPlayerWonText;
    public TextMeshProUGUI roundOverOverlayText;
    public GameObject roundOverOverlay;

    public Image p1Clapper;
    public Image p2Clapper;
    public Sprite clapperOpen;
    public Sprite clapperClosed;

    public Image[] p1Lights;
    public Image[] p2Lights;
    public Sprite lightOff;
    public Sprite lightOn;

    public GameObject[] p1Megaphones;
    public GameObject[] p2Megaphones;

    public GameObject[] p1Heart;
    public GameObject[] p2Heart;

    public Image[] p1AQ;
    public Image[] p2AQ;
    public Sprite AQLeft;
    public Sprite AQRight;
    public Sprite AQWait;
    public Sprite AQPunch;
    public Sprite AQBlock;
    public Sprite AQStun;
    public Sprite AQWindup;

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

    //Update UI in action queue loop, doesn't need to update everything
    public void updateUILoop(Player p1, Player p2, bool p1r, bool p2r)
    {
        updateHealths(p1.health, p2.health);
        updateActionLimit(p1, p2);
        updateReadys(p1r, p2r);
    }

    //Update Health amount UIs
    public void updateHealths(int p1, int p2)
    {
        refreshHearts(p1, p1Heart);
        refreshHearts(p2, p2Heart);
    }

    //Update visual images of hearts for the player HP
    void refreshHearts(int p, GameObject[] gArr)
    {
        foreach(GameObject g in gArr)
        {
            g.SetActive(false);
        }
        if (p > 0)
        {
            for (int i = 0; i < p; i++)
            {
                gArr[i].SetActive(true);
            }
        }
    }

    //Update Round number UI
    public void updateRoundNumber(int r)
    {
        roundsText.text = "Scene\n" + r;
    }

    //Update Round wins of both players' UIs
    public void updateRoundWins(int p1, int p2)
    {
        refreshRoundLights(p1Lights, p1);
        refreshRoundLights(p2Lights, p2);
    }

    //Update visual images of lights based on round wins. Initially set all to off
    void refreshRoundLights(Image[] iArr, int wins)
    {
        int count = 0;
        foreach(Image i in iArr)
        {
            i.sprite = lightOff;
        }
        foreach(Image i in iArr){
            if (count == wins)
            {
                return;
            }
            i.sprite = lightOn;
            count++;
        }
    }

    //Update Action limit of both players' UI 
    public void updateActionLimit(Player p1, Player p2)
    {
        refreshMegaphoneIcons(p1, p1Megaphones);
        refreshMegaphoneIcons(p2, p2Megaphones);
    }

    //Update visual images of action limit points available (megaphones)
    void refreshMegaphoneIcons(Player p, GameObject[] gArr)
    {
        foreach(GameObject g in gArr)
        {
            g.SetActive(false);
        }
        if(p.actionPoints > 0)
        {
            for(int i = 0; i < p.actionPoints; i++)
            {
                gArr[i].SetActive(true);
            }
        }
    }

    //Update Player ready UIs 
    public void updateReadys(bool p1, bool p2)
    {
        if (p1)
        {
            p1Clapper.sprite = clapperClosed;
            
            p1Ready.color = Color.green;
            p1Ready.text = "P1 RDY";
            p1r = true;
        } else
        {
            p1Clapper.sprite = clapperOpen;
            
            p1Ready.color = Color.red;
            p1Ready.text = "P1 NOT RDY (V)";
            p1r = false;
        }
        if (p2)
        {
            p2Clapper.sprite = clapperClosed;
            
            p2Ready.color = Color.green;
            p2Ready.text = "P2 RDY";
            p2r = true;
        } else
        {
            p2Clapper.sprite = clapperOpen;
            
            p2Ready.color = Color.red;
            p2Ready.text = "P2 NOT RDY (B)";
            p2r = false;
        }
    }

    //Update Game Over text with correct player number
    public void updateGameOverPlayerText(string p)
    {
        gameOverPlayerWonText.text = "Game Over\n" + p + " Wins the Role!";
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
        addSpriteActionQueue(p1AQ, parseDisplayAction(a1));
        addSpriteActionQueue(p2AQ, parseDisplayAction(a2));
    }

    //Add sprite to the last most empty image
    void addSpriteActionQueue(Image[] q, Sprite s)
    {
        foreach(Image i in q)
        {
            if(i.sprite == null)
            {
                i.gameObject.SetActive(true);
                i.sprite = s;

                return;
            }
        }

        //reached the end and all images being used
        //shift all images one down then add the sprite to the final spot
        shiftImagesDown(q);
        q[q.Length - 1].sprite = s;
    }

    //Move action queue Images down 1 spot
    void shiftImagesDown(Image[] q)
    {
        for(int i = 0; i < q.Length-1; i++)
        {
            q[i].sprite = q[i+1].sprite;
        }
    }

    //Determine what action to display from the given action
    Sprite parseDisplayAction(ACTIONS a)
    {
        if (a == ACTIONS.MOVELEFT)
        {
            return AQLeft;
        } else if (a == ACTIONS.MOVERIGHT)
        {
            return AQRight;
        } else if (a == ACTIONS.WAIT)
        {
            return AQWait;
        } else if(a == ACTIONS.WINDUP)
        {
            return AQWindup;
        } else if(a == ACTIONS.PUNCH)
        {
            return AQPunch;
        } else if(a == ACTIONS.BLOCK)
        {
            return AQBlock;
        } else if(a == ACTIONS.STUN)
        {
            return AQStun;
        } else
        {
            return null;
        }
    }

    //Clear the action queue display UI
    public void clearActionQueues()
    {
        foreach(Image i in p1AQ)
        {
            i.sprite = null;
            i.gameObject.SetActive(false);
        }
        foreach (Image i in p2AQ)
        {
            i.sprite = null;
            i.gameObject.SetActive(false);
        }
    }
}
