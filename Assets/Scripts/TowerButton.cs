using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private Tower TowerObject;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] int towerPrice;
    public Tower towerobject
    {
        get
        {
            return TowerObject;
        }
    }
    public Sprite DragSprite
    {
        get
        {
            return dragSprite;
        }
    }
    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
