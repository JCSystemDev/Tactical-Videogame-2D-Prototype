using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int unitsRed, unitsBlue;
    public AudioSource changeTurnSFX;
    public Unit selectedUnit;
    public int playerTurn = 1;
    public GameObject selectedUnitSquare1;
    public GameObject selectedUnitSquare2;
    public bool movePiece;
    public GameObject game, victoryBlue, victoryRed, ui, rules;
    public bool rulesOn;
    
    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (rulesOn)
        {
            rules.SetActive(true);
        }
        else if (!rulesOn)
        {
            rules.SetActive(false);
        }
        
        
        if (Input.GetButtonDown("Jump"))
        {
            if (rulesOn)
            {
                rulesOn = false;
            }
            
            else if (!rulesOn)
            {
                rulesOn = true;
            }
            
        }    
        
        
        if (unitsRed <= 0)
        {
            game.SetActive(false);
            victoryBlue.SetActive(true);
            ui.SetActive(false);
        }
        
        else if (unitsBlue <= 0)
        {
            game.SetActive(false);
            victoryRed.SetActive(true);
            ui.SetActive(false);
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

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    
}
