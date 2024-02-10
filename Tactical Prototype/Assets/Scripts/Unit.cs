using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;
    GameManager gm;
    public int attackRange;
    private List<Unit> _enemiesInRange = new List<Unit>();
    public bool hasAttacked;
    public GameObject attackIcon;
    public GameObject lifeBar;
    public Transform bar;
    public int tileSpeed;
    public bool hasMoved;
    public float moveSpeed;
    public int playerNumber;
    public int health, healthMax;
    public int attackDamage;
    public int defenseDamage;
    public int armor;
    public Animator anim;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();

    }
    
    private void OnMouseDown()
    {
        ResetAttackIcons();
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
                GetEnemies();
                GetWalkableTiles();
                
            }
     
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        Unit unit = col.GetComponent<Unit>();
        if(gm.selectedUnit != null)
        {
            if (gm.selectedUnit._enemiesInRange.Contains(unit) && !gm.selectedUnit.hasAttacked)
            {
                gm.selectedUnit.Attack(unit);
            }
        }
    }

    void Attack(Unit enemy)
    {
        hasAttacked = true;
        int enemyDamage = attackDamage - enemy.armor;
        int myDamage = enemy.defenseDamage - armor;

        if (enemyDamage >= 1)
        {
            enemy.health -= enemyDamage;
            anim.Play("Attack");
            enemy.anim.Play("Hurt");
        }

        if (myDamage >= 1)
        {
            health -= myDamage;
            anim.Play("Hurt");
            enemy.anim.Play("Attack");
        }

        if (enemy.health <= 0)
        {
            Destroy(enemy.gameObject, 1);
            GetWalkableTiles();
        }

        if (health <= 0)
        {
            gm.ResetTiles();
            Destroy(this.gameObject, 1);
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

    void GetEnemies()
    {
        _enemiesInRange.Clear();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) +
                Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange)
            {
                if (unit.playerNumber != gm.playerTurn && !hasAttacked)
                {
                    _enemiesInRange.Add(unit);
                    unit.attackIcon.SetActive(true);
                }
                
            }
            
        }
    }

    public void ResetAttackIcons()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.attackIcon.SetActive(false);
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
        ResetAttackIcons();
        GetEnemies();
    }
}
