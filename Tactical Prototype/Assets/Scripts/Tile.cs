using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    public Sprite[] tileGraphics;
    [SerializeField] private float hoverAmount;
    public LayerMask obstacleLayer;
    public Color highLightedColor;
    public bool isWalkable;
    GameManager gm;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int randTile = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[randTile];

        gm = FindObjectOfType<GameManager>();

    }

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
  
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
       
    }

    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        if (obstacle != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void HighLight()
    {
        rend.color = highLightedColor;
        isWalkable = true;
    }

    public void Reset()
    {
        rend.color = Color.white;
        isWalkable = false;
    }

    private void OnMouseDown()
    {
        if (isWalkable && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(this.transform.position);
        }
    }
    
}