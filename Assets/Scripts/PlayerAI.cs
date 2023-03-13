using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    Player player;
    int[] weights;
    int weightTotal;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        
        //Weights of the actions, higher number = more likely
        weights = new int[5];
        weights[(int)ACTIONS.MOVELEFT] = 30; //MOVELEFT
        weights[(int)ACTIONS.MOVERIGHT] = 10; //MOVERIGHT
        weights[(int)ACTIONS.WAIT] = 5; //WAIT
        weights[(int)ACTIONS.PUNCH] = 30; //PUNCH
        weights[(int)ACTIONS.BLOCK] = 10; //BLOCK

        foreach (int w in weights)
        {
            weightTotal += w;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generates list of actions for AI
    public List<ACTIONS> genActions()
    {
        List<ACTIONS> list = new List<ACTIONS>();

        while (player.actionPoints > 0) {
            ACTIONS a = rollAction();
            if (player.canAction(a))
            {
                if(a == ACTIONS.PUNCH)
                {
                    list.Add(ACTIONS.WINDUP);
                }
                player.useAP(a);
                list.Add(a);
            }
        }
        return list;
    }

    //Select action using weighted randomness
    ACTIONS rollAction()
    {
        //Weighted random
        int total = 0;
        int i;
        int randomValue = Random.Range(0, weightTotal + 1);
        for(i = 0; i < weights.Length - 1; i++)
        {
            total += weights[i];
            if(randomValue <= total)
            {
                break;
            }
        }
        
        return (ACTIONS)i;
    }
}
