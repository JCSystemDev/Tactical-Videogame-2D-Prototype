using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] damageSprites;
    public float lifeTime;

    private void Start()
    {
        Invoke("Destruction", lifeTime);
    }

    public void Setup(int damage)
    {
        GetComponent<SpriteRenderer>().sprite = damageSprites[damage];
    }

    void Destruction()
    {
        Destroy(gameObject);
    }


}
