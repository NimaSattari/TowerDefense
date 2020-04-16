using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TowerManager : Singleton<TowerManager>
{
    public TowerButton TowerBtnPressed { get; set; }
    SpriteRenderer spriteRenderer;
    private List<Tower> TowerList = new List<Tower>();
    List<Collider2D> BuildList = new List<Collider2D>();
    Collider2D buildTile;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero);
            if(hit2D.collider.tag== "BuildSite")
            {
                buildTile = hit2D.collider;
                buildTile.tag = "BuildSiteFull";
                RegisterBuildSite(buildTile);
                PlaceTower(hit2D);
            }

        }
        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }
    public void RegisterBuildSite(Collider2D buildeTag)
    {
        BuildList.Add(buildeTag);
    }
    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }
    public void RenameTagsBuildSite()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildSite";
        }
        BuildList.Clear();
    }
    public void DestroyAllTower()
    {
        foreach(Tower tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }
    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && TowerBtnPressed != null)
        {
            Tower newTower = Instantiate(TowerBtnPressed.towerobject);
            newTower.transform.position = hit.transform.position;
            BuyTower(TowerBtnPressed.TowerPrice);
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
            RegisterTower(newTower);
            disableDragSprite();
            TowerBtnPressed = null;
        }
    }
    public void BuyTower(int price)
    {
        GameManager.Instance.subtractMoney(price);
    }

    public void selectedTower(TowerButton towerSelected)
    {
        if(towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            TowerBtnPressed = towerSelected;
            enableDragSprite(TowerBtnPressed.DragSprite);
        }
    }
    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }
    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
    }
}
