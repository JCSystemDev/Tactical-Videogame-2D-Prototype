using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject turn1, turn2;
    public GameManager gm;

    private void Start()
    {
        
    }

    void Update()
    {
        if (gm.playerTurn == 1)
        {
            turn1.SetActive(true);
            turn2.SetActive(false);
        }
        else if (gm.playerTurn == 2)
        {
            turn1.SetActive(false);
            turn2.SetActive(true);
        }
        
    }
}
