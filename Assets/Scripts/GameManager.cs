using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region カード
    [SerializeField] private GameObject _club1;
    [SerializeField] private GameObject _club2;
    [SerializeField] private GameObject _club3;
    [SerializeField] private GameObject _club4;
    [SerializeField] private GameObject _club5;
    [SerializeField] private GameObject _club6;
    [SerializeField] private GameObject _club7;
    [SerializeField] private GameObject _club8;
    [SerializeField] private GameObject _club9;
    [SerializeField] private GameObject _club10;
    [SerializeField] private GameObject _club11;
    [SerializeField] private GameObject _club12;
    [SerializeField] private GameObject _club13;

    [SerializeField] private GameObject _diamond1;
    [SerializeField] private GameObject _diamond2;
    [SerializeField] private GameObject _diamond3;
    [SerializeField] private GameObject _diamond4;
    [SerializeField] private GameObject _diamond5;
    [SerializeField] private GameObject _diamond6;
    [SerializeField] private GameObject _diamond7;
    [SerializeField] private GameObject _diamond8;
    [SerializeField] private GameObject _diamond9;
    [SerializeField] private GameObject _diamond10;
    [SerializeField] private GameObject _diamond11;
    [SerializeField] private GameObject _diamond12;
    [SerializeField] private GameObject _diamond13;

    [SerializeField] private GameObject _heart1;
    [SerializeField] private GameObject _heart2;
    [SerializeField] private GameObject _heart3;
    [SerializeField] private GameObject _heart4;
    [SerializeField] private GameObject _heart5;
    [SerializeField] private GameObject _heart6;
    [SerializeField] private GameObject _heart7;
    [SerializeField] private GameObject _heart8;
    [SerializeField] private GameObject _heart9;
    [SerializeField] private GameObject _heart10;
    [SerializeField] private GameObject _heart11;
    [SerializeField] private GameObject _heart12;
    [SerializeField] private GameObject _heart13;

    [SerializeField] private GameObject _spade1;
    [SerializeField] private GameObject _spade2;
    [SerializeField] private GameObject _spade3;
    [SerializeField] private GameObject _spade4;
    [SerializeField] private GameObject _spade5;
    [SerializeField] private GameObject _spade6;
    [SerializeField] private GameObject _spade7;
    [SerializeField] private GameObject _spade8;
    [SerializeField] private GameObject _spade9;
    [SerializeField] private GameObject _spade10;
    [SerializeField] private GameObject _spade11;
    [SerializeField] private GameObject _spade12;
    [SerializeField] private GameObject _spade13;
    #endregion カード

    #region カード置き場
    [SerializeField] public GameObject _grave;

    [SerializeField] private GameObject club;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject spade;
    [SerializeField] private GameObject diamond;

    [SerializeField] private GameObject _back1;
    [SerializeField] private GameObject _back2;
    [SerializeField] private GameObject _back3;
    [SerializeField] private GameObject _back4;
    [SerializeField] private GameObject _back5;
    [SerializeField] private GameObject _back6;
    [SerializeField] private GameObject _back7;
    #endregion　カード置き場

    /// <summary>カードデッキリスト</summary>
    public static List<GameObject> cardDeck;
    /// <summary>Deckリスト</summary>
    public static List<List<GameObject>> deckList;
    public static List<GameObject> graveDeck = new List<GameObject>();
    public static List<GameObject> clubDeck = new List<GameObject>();
    public static List<GameObject> heartDeck = new List<GameObject>();
    public static List<GameObject> spadeDeck = new List<GameObject>();
    public static List<GameObject> diamandDeck = new List<GameObject>();
    public static List<GameObject> back1Deck = new List<GameObject>();
    public static List<GameObject> back2Deck = new List<GameObject>();
    public static List<GameObject> back3Deck = new List<GameObject>();
    public static List<GameObject> back4Deck = new List<GameObject>();
    public static List<GameObject> back5Deck = new List<GameObject>();
    public static List<GameObject> back6Deck = new List<GameObject>();
    public static List<GameObject> back7Deck = new List<GameObject>();

    /// <summary>Y方向のカードのズレ</summary>
    private const float _cardGapY = 0.0001f;
    /// <summary>Z方向のカードのズレ</summary>
    private const float _cardGapZ = -0.02f;


    void Start()
    {
        // カードデッキにカードの追加
        cardDeck = new List<GameObject> { _club1, _club2, _club3, _club4, _club5, _club6, _club7, _club8, _club9, _club10, _club11, _club12, _club13,
                                           _diamond1, _diamond2, _diamond3, _diamond4, _diamond5, _diamond6, _diamond7, _diamond8, _diamond9, _diamond10, _diamond11, _diamond12, _diamond13,
                                           _heart1, _heart2, _heart3, _heart4, _heart5, _heart6, _heart7, _heart8, _heart9, _heart10, _heart11, _heart12, _heart13,
                                           _spade1, _spade2, _spade3, _spade4, _spade5, _spade6, _spade7, _spade8, _spade9, _spade10, _spade11, _spade12, _spade13};
        // Deckリストにデッキの追加
        deckList = new List<List<GameObject>> { back1Deck, back2Deck, back3Deck, back4Deck, back5Deck, back6Deck, back7Deck,
                                                clubDeck, heartDeck, spadeDeck, diamandDeck, graveDeck, cardDeck};
        Shuffle();
        CardsArrangement();
    }

    void Update()
    {
        
    }

    

    /// <summary>
    /// デッキのシャッフル
    /// </summary>
    private void Shuffle()
    {
        cardDeck = cardDeck.OrderBy(card => Guid.NewGuid()).ToList();
    }

    /// <summary>
    /// deckListの更新
    /// </summary>
    /// <param name="deletedCardDeck">削除されたカードがあったデッキ</param>
    /// <param name="addedCardDeck">追加されたカードのあるデッキ</param>
    private void ResetCardDeck(List<GameObject> deletedCardDeck, List<GameObject> addedCardDeck)
    {
        for(var i = 0; i < deckList.Count; i++)
        {
            // 削除されたカードがあったデッキの更新
            if (deckList[i] == deletedCardDeck)
            {
                deckList.Remove(deletedCardDeck);
                deckList.Insert(i, deletedCardDeck);
            }

            // 追加されたカードのあるデッキの更新
            if (deckList[i] == addedCardDeck)
            {
                deckList.Remove(addedCardDeck);
                deckList.Insert(i, addedCardDeck);
            }
        }
    }

    /// <summary>
    /// デッキのカード配り
    /// </summary>
    private void CardsArrangement()
    {
        Vector3 back1DeckPos = _back1.transform.position;
        // _back1へ移動
        for (int index = 0; index < 7; index++)
        {
            cardDeck[index].transform.position = new Vector3(back1DeckPos.x, back1DeckPos.y + index * _cardGapY, back1DeckPos.z + index * _cardGapZ);
            // 移動したカードをバックデッキリストへ追加
            back1Deck.Add(cardDeck[index]);
            // 移動したカードはデッキリストから削除
            cardDeck.RemoveAt(index);
            // deckListの更新
            ResetCardDeck(cardDeck, back1Deck);
        }
        // バックデッキリストの1番上を表向きに
        back1Deck[back1Deck.Count - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Vector3 back2DeckPos = _back2.transform.position;
        // _back2へ移動
        for (int index = 0; index < 6; index++)
        {
            cardDeck[index].transform.position = new Vector3(back2DeckPos.x, back2DeckPos.y + index * _cardGapY, back2DeckPos.z + index * _cardGapZ);
            back2Deck.Add(cardDeck[index]);
            cardDeck.RemoveAt(index);
            ResetCardDeck(cardDeck, back2Deck);
        }
        back2Deck[back2Deck.Count - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);


        Vector3 back3DeckPos = _back3.transform.position;

        // _back3へ移動
        for (int index = 0; index < 5; index++)
        {
            cardDeck[index].transform.position = new Vector3(back3DeckPos.x, back3DeckPos.y + index * _cardGapY, back3DeckPos.z + index * _cardGapZ);
            back3Deck.Add(cardDeck[index]);
            cardDeck.RemoveAt(index);
            ResetCardDeck(cardDeck, back3Deck);
        }
        back3Deck[back3Deck.Count - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Vector3 back4DeckPos = _back4.transform.position;
        // _back4へ移動
        for (int index = 0; index < 4; index++)
        {
            cardDeck[index].transform.position = new Vector3(back4DeckPos.x, back4DeckPos.y + index * _cardGapY, back4DeckPos.z + index * _cardGapZ);
            back4Deck.Add(cardDeck[index]);
            cardDeck.RemoveAt(index);
            ResetCardDeck(cardDeck, back4Deck);
        }
        back4Deck[back4Deck.Count - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Vector3 back5DeckPos = _back5.transform.position;
        // _back5へ移動
        for (int index = 0; index < 3; index++)
        {
            cardDeck[index].transform.position = new Vector3(back5DeckPos.x, back5DeckPos.y + index * _cardGapY, back5DeckPos.z + index * _cardGapZ);
            back5Deck.Add(cardDeck[index]);
            cardDeck.RemoveAt(index);
            ResetCardDeck(cardDeck, back5Deck);
        }
        back5Deck[back5Deck.Count - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Vector3 back6DeckPos = _back6.transform.position;
        // _back6へ移動
        for (int index = 0; index < 2; index++)
        {
            cardDeck[index].transform.position = new Vector3(back6DeckPos.x, back6DeckPos.y + index * _cardGapY, back6DeckPos.z + index * _cardGapZ);
            back6Deck.Add(cardDeck[index]);
            cardDeck.RemoveAt(index);
            ResetCardDeck(cardDeck, back6Deck);
        }
        back6Deck[back6Deck.Count - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Vector3 back7DeckPos = _back7.transform.position;
        // _back7へ移動
        cardDeck[0].transform.position = new Vector3(back7DeckPos.x, back7DeckPos.y, back7DeckPos.z);
        back7Deck.Add(cardDeck[0]);
        cardDeck.RemoveAt(0);
        ResetCardDeck(cardDeck, back7Deck);
        back7Deck[back7Deck.Count - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);

    }

    
}
