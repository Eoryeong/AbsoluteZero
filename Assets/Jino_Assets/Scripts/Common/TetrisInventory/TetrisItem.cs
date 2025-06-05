using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Tetris/Item")]
public class TetrisItem : ScriptableObject
{
    public Sprite itemIcon;
    public Vector2 itemSize;
}
