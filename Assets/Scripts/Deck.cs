using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    /// <summary>デッキ名</summary>
    public string deckName;

    /// <summary>デッキの種類。0 = backDeck. 1 = frontDeck. 2 = others</summary>
    public int deckIndex;
}
