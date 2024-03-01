using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    public static Action<Tile> onTileSelected;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Tile[] selectedTiles = new Tile[2];
    [SerializeField] private Tile[] allTiles;
    [SerializeField] private Transform level;
    private bool isAllTilesOpened;
    private void Awake()
    {
        onTileSelected += SelectTile;
    }
    private void OnDisable()
    {
        onTileSelected -= SelectTile;
    }
    private void Start()
    {
        FindAllTiles();
    }
    public void FindAllTiles()
    {
        allTiles = level.GetComponentsInChildren<Tile>();
    }
    private void SelectTile(Tile newTile)
    {
            TileArraySwap(newTile);
    }
    private void TileArraySwap(Tile newTile)
    {
        Debug.Log("TileArraySwap");
        //if (newTile == selectedTiles[0])
        //{
        //    newTile.UnSelectTile();
        //    selectedTiles[0].UnSelectTile();
        //    selectedTiles[0] = null;
        //    selectedTiles[1] = null;
            
        //    return;
        //}
        //if (newTile == selectedTiles[1])
        //{
        //    selectedTiles[1].UnSelectTile();
        //    selectedTiles[0] = null;
        //    selectedTiles[1] = null;
        //    newTile.UnSelectTile();
        //    return;
        //}
        if (selectedTiles[0] == null)
        {
            selectedTiles[0] = newTile;
            return;
        }
        else if (selectedTiles[0] != null && selectedTiles[1]==null)
        {
            selectedTiles[1] = newTile;
        }
        else
        {
            selectedTiles[0] = selectedTiles[1];
            selectedTiles[1] = newTile;
        }

        if (selectedTiles[1] != null)
        {
            CheckTiles();
        }
    }
    private void MergeTile()
    {
        selectedTiles[0].ShowNeighbours();
        selectedTiles[1].ShowNeighbours();
        selectedTiles[0].gameObject.SetActive(false);
        selectedTiles[1].gameObject.SetActive(false);
        ClearArray();
    }
    private void ClearArray()
    {
        StartCoroutine(WaitForUnSelectTile(selectedTiles[0]));
        StartCoroutine(WaitForUnSelectTile(selectedTiles[1]));
        selectedTiles[0] = null;
        selectedTiles[1] = null;
    }
    private void CheckTiles()
    {
        if (selectedTiles[0].type==selectedTiles[1].type)
        {
            MergeTile();
        }
        else
        {
            ClearArray();
        }
        isAllTilesOpened = true;
        foreach(Tile tile in allTiles)
        {
            if (isAllTilesOpened)
            isAllTilesOpened = tile.isCanInteract;
        }
        if (isAllTilesOpened)
        {
            LevelCompleted();
        }
    }
    private void LevelCompleted()
    {
        Debug.Log("level completed");
        GameManager.instance.EndGame();
    }
    private IEnumerator WaitForUnSelectTile(Tile tile)
    {
        yield return new WaitForSeconds(0.3f);
        tile.UnSelectTile();
    }
}
