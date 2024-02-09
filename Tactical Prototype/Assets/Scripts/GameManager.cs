using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Unit selectedUnit;
    public int playerTurn = 1;
    public GameObject selectedUnitSquare;

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            EndTurn();
        }

        if (selectedUnit != null)
        {
            selectedUnitSquare.SetActive(true);
        }
    }

    void EndTurn()
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
        }
        else if (playerTurn == 2)
        {
            playerTurn = 1;
        }

        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }
        
        ResetTiles();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
        }

    }
    
}
