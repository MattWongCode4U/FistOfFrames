using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum USER {PLAYER1, PLAYER2 };
public enum ACTIONS { MOVELEFT, MOVERIGHT, WAIT, PUNCH, BLOCK, STUN, WINDUP };

public class Player : MonoBehaviour
{
    public USER user;
    public bool inAir = false;
    public int pos;
    public int fullHealth = 3;
    public int health;
    public int punchDamage = 1;
    public bool blocking = false;
    public int actionLimit = 3;
    public int actionPoints;
    public bool stunned = false;
    public float moveSpeed = 5f;

    public bool superPunch = false;
    public GameObject SuperPunchLight;
    public GameObject DamageLight;

    public Dictionary<ACTIONS, int> ActionCost;
    Vector3 startPos, targetPos;
    float elapsedTime;
    float desiredDuration = 1.0f;

    Animator anim;
    Animator dmgLightAnim;

    // Start is called before the first frame update
    void Start()
    {
        health = fullHealth;
        actionPoints = actionLimit;

        anim = GetComponent<Animator>();
        dmgLightAnim = DamageLight.GetComponent<Animator>();

        ActionCost = new Dictionary<ACTIONS, int>();
        ActionCost.Add(ACTIONS.MOVELEFT, 1);
        ActionCost.Add(ACTIONS.MOVERIGHT, 1);
        ActionCost.Add(ACTIONS.WAIT, 0);
        ActionCost.Add(ACTIONS.WINDUP, 0);
        ActionCost.Add(ACTIONS.PUNCH, 1);
        ActionCost.Add(ACTIONS.BLOCK, 1);
        ActionCost.Add(ACTIONS.STUN, 0);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, targetPos, moveSpeed * elapsedTime / desiredDuration);
    }

    //Moves gameobject to the position of to
    public void moveTo(Transform to)
    {
        elapsedTime = 0f;
        startPos = transform.position;
        targetPos = new Vector3(to.position.x, transform.position.y, transform.position.z);
    }

    //Sets player to the given position 
    public void snapTo(Transform to)
    {
        transform.position = new Vector3(to.position.x, transform.position.y, transform.position.z); ;
        startPos = transform.position;
        targetPos = new Vector3(to.position.x, transform.position.y, transform.position.z); ;
    }

    //Damage taken, blocking check
    public void takeDamage(int dmg)
    {
        int d = dmg;
        
        if (blocking)
        {
            d = 0;
        }
        Debug.Log(user.ToString() + " took damage");
        health -= d;
        Debug.Log(user.ToString() + " Health: " + health);
    }

    //Resets variables for new round
    public void reset()
    {
        health = fullHealth;
        blocking = false;
        disableSuperPunch();
        resetAP();
    }

    //Resets action points (AP)
    public void resetAP()
    {
        actionPoints = actionLimit;
    }

    //Uses action points amt for an action
    public void useAP(ACTIONS a)
    {
        actionPoints -= ActionCost[a];
    }

    //Refunds action points amt for an undo
    public void refundAP(ACTIONS a)
    {
        actionPoints += ActionCost[a];
    }

    //Has enough points to perform action given
    public bool canAction(ACTIONS a)
    {
        if(actionPoints >= ActionCost[a])
        {
            return true;
        } else
        {
            return false;
        }
    }

    //Enable super punch and visual
    public void enableSuperPunch()
    {
        superPunch = true;
        SuperPunchLight.SetActive(true);
    }

    //Disable super punch and visual
    public void disableSuperPunch()
    {
        superPunch = false;
        SuperPunchLight.SetActive(false);
    }

    //Animations
    //Reset idle animation from the start
    public void idleAnimReset()
    {
        anim.Play("P_Idle", -1, 0f);
    }

    //Play running animation
    public void runAnim()
    {
        anim.SetTrigger("Run");
    }

    //Play windup animation
    public void windupAnim()
    {
        anim.SetTrigger("Windup");
    }

    //Play punch animation
    public void punchAnim()
    {
        anim.SetTrigger("Punch");
    }

    //Play super punch animation
    public void superPunchAnim()
    {
        anim.SetTrigger("SuperPunch");
    }

    //Play punch animation
    public void blockAnim()
    {
        anim.SetTrigger("Block");
    }

    //Play death animation
    public void deathAnim()
    {
        anim.SetTrigger("Death");
    }

    //Play stun animation
    public void stunAnim()
    {
        anim.SetTrigger("Stun");
    }

    //Play damage light animation
    public void damageLight()
    {
        dmgLightAnim.SetTrigger("Flash");
    }

    //Reset all animation triggers
    public void resetAnimTriggers()
    {
        anim.ResetTrigger("Run");
        anim.ResetTrigger("Punch");
        anim.ResetTrigger("Block");
        anim.ResetTrigger("Death");
        anim.ResetTrigger("Stun");
        anim.ResetTrigger("SuperPunch");
        dmgLightAnim.ResetTrigger("Flash");
    }
}
