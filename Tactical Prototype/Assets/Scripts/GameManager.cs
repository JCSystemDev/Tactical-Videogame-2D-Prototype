using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int unitsRed, unitsBlue;
    public AudioSource changeTurnSFX;
    public Unit selectedUnit;
    public int playerTurn = 1;
    public GameObject selectedUnitSquare1;
    public GameObject selectedUnitSquare2;
    public bool movePiece; 

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
            if (playerTurn == 1)
            {
                selectedUnitSquare1.SetActive(true);
                selectedUnitSquare1.transform.position = selectedUnit.transform.position;
            }
            else if (playerTurn == 2)
            {
                selectedUnitSquare2.SetActive(true);
                selectedUnitSquare2.transform.position = selectedUnit.transform.position;
            }
            
            
        }
        else
        {
            selectedUnitSquare1.SetActive(false);
            selectedUnitSquare2.SetActive(false);
        }
        
    }

    public void EndTurn()
    {
        movePiece = false;
        changeTurnSFX.Play();
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
            unit.attackIcon.SetActive(false);
            unit.hasAttacked = false;
        }

    }
    
}
