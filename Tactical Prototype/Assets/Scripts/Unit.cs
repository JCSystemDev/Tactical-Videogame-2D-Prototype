using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;
    GameManager gm;
    public int tileSpeed;
    public bool hasMoved;
    public float moveSpeed;
    public int playerNumber;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        if (selected)
        {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        }
        else 
        {
            if (playerNumber == gm.playerTurn)
            {
                if (gm.selectedUnit != null)
                {
                    gm.selectedUnit.selected = false;
                }

                selected = true;
                gm.selectedUnit = this;
                gm.ResetTiles();
                GetWalkableTiles();
                
            }
     
        }
    }

    void GetWalkableTiles()
    {
        if (hasMoved)
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) +
                Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed)
            {
                if (tile.IsClear())
                {
                    tile.HighLight();
                }
            }
        }
        
    }

    public void Move(Vector2 tilePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        while (transform.position.x != tilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
    }
}
