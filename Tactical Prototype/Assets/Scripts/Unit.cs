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
    public int tileSpeed;
    public bool hasMoved;
    public float moveSpeed;
    public int playerNumber;
    public int health;
    public int attackDamage;
    public int armor;
    public Animator anim;
    private SpriteRenderer spr;
    public DamageIcon dmgIcon;
    public AudioSource attackSfx, hitSFX, deadSFX, clickSFX, moveSFX;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();

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
            if ((playerNumber == gm.playerTurn) && !gm.movePiece)
            {
                clickSFX.Play();
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

        if (enemyDamage >= 1)
        {
            DamageIcon instance = Instantiate(dmgIcon, 
                new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z), 
                Quaternion.identity);
            instance.Setup(enemyDamage);
            enemy.health -= enemyDamage;
            spr.sortingOrder = 1;
            enemy.spr.sortingOrder = -1;
            anim.Play("Attack");
            attackSfx.Play();
            enemy.anim.Play("Hurt");
            hitSFX.Play();
        }
        
        if (enemy.health <= 0)
        {
            if (gm.playerTurn == 1)
            {
                gm.unitsRed--;
            }
            else if (gm.playerTurn == 2)
            {
                gm.unitsBlue--;
            }
            enemy.anim.Play("Dead");
            enemy.deadSFX.Play();
            Destroy(enemy.gameObject, 1f);
            GetWalkableTiles();
        }

        if (health <= 0)
        {
            if (gm.playerTurn == 1)
            {
                gm.unitsBlue--;
            }
            else if (gm.playerTurn == 2)
            {
                gm.unitsRed--;
            }
            gm.ResetTiles();
            anim.Play("Dead");
            deadSFX.Play();
            Destroy(this.gameObject, 1f);
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
        if (!gm.movePiece)
        {
            gm.movePiece = true;
            gm.ResetTiles();
            StartCoroutine(StartMovement(tilePos));
            
        }
        
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        moveSFX.Play();
        
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
