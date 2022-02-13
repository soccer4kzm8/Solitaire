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

    /// <summary>
    /// カードデッキリスト
    /// </summary>
    public static List<GameObject> cardDeck;
    /// <summary>
    /// Deckリスト
    /// </summary>
    public static List<KeyValuePair<string, List<GameObject>>> deckList;

    public static List<GameObject> graveDeck;
    public static List<GameObject> clubDeck;
    public static List<GameObject> heartDeck;
    public static List<GameObject> spadeDeck;
    public static List<GameObject> diamondDeck;
    public static List<GameObject> back1Deck;
    public static List<GameObject> back2Deck;
    public static List<GameObject> back3Deck;
    public static List<GameObject> back4Deck;
    public static List<GameObject> back5Deck;
    public static List<GameObject> back6Deck;
    public static List<GameObject> back7Deck;

    /// <summary>
    /// Y方向のカードのズレ
    /// </summary>
    public static float cardGapY = 0.0001f;

    /// <summary>
    /// Z方向のカードのズレ
    /// </summary>
    public static float cardGapZ = -0.02f;

    /// <summary>
    /// deckList内のcardDeckのIndex
    /// </summary>
    private const int _cardDeckIndex = 12;

    /// <summary>
    /// Z方向のStartPosの調整定数
    /// </summary>
    public const float cardStartPosZ = 0.15f;


    void Start()
    {
        graveDeck = new List<GameObject>();
        clubDeck = new List<GameObject>();
        heartDeck = new List<GameObject>();
        spadeDeck = new List<GameObject>();
        diamondDeck = new List<GameObject>();
        back1Deck = new List<GameObject>();
        back2Deck = new List<GameObject>();
        back3Deck = new List<GameObject>();
        back4Deck = new List<GameObject>();
        back5Deck = new List<GameObject>();
        back6Deck = new List<GameObject>();
        back7Deck = new List<GameObject>();

        // カードデッキにカードの追加
        cardDeck = new List<GameObject>
        { 
            _club1, _club2, _club3, _club4, _club5, _club6, _club7, _club8, _club9, _club10, _club11, _club12, _club13,
            _diamond1, _diamond2, _diamond3, _diamond4, _diamond5, _diamond6, _diamond7, _diamond8, _diamond9, _diamond10, _diamond11, _diamond12, _diamond13,
            _heart1, _heart2, _heart3, _heart4, _heart5, _heart6, _heart7, _heart8, _heart9, _heart10, _heart11, _heart12, _heart13,
            _spade1, _spade2, _spade3, _spade4, _spade5, _spade6, _spade7, _spade8, _spade9, _spade10, _spade11, _spade12, _spade13
        };

        deckList = new List<KeyValuePair<string, List<GameObject>>>()
        {
            new KeyValuePair<string, List<GameObject>>("back1Deck", back1Deck),
            new KeyValuePair<string, List<GameObject>>("back2Deck", back2Deck),
            new KeyValuePair<string, List<GameObject>>("back3Deck", back3Deck),
            new KeyValuePair<string, List<GameObject>>("back4Deck", back4Deck),
            new KeyValuePair<string, List<GameObject>>("back5Deck", back5Deck),
            new KeyValuePair<string, List<GameObject>>("back6Deck", back6Deck),
            new KeyValuePair<string, List<GameObject>>("back7Deck", back7Deck),
            new KeyValuePair<string, List<GameObject>>("clubDeck", clubDeck),
            new KeyValuePair<string, List<GameObject>>("heartDeck", heartDeck),
            new KeyValuePair<string, List<GameObject>>("spadeDeck", spadeDeck),
            new KeyValuePair<string, List<GameObject>>("diamondDeck", diamondDeck),
            new KeyValuePair<string, List<GameObject>>("graveDeck", graveDeck),
            new KeyValuePair<string, List<GameObject>>("cardDeck", cardDeck),
        };

        Shuffle();
        CardsArrangement();
    }

    /// <summary>
    /// デッキのシャッフル
    /// </summary>
    private void Shuffle()
    {
        deckList[_cardDeckIndex].Value = cardDeck.OrderBy(card => Guid.NewGuid()).ToList();
    }

    /// <summary>
    /// カード移動手順
    /// </summary>
    /// <param name="deckPos">移動先Deckの位置</param>
    /// <param name="deckCapa">移動先デッキのキャパ</param>
    /// <param name="deckName">移動先デッキの名前</param>
    private void CardPositionMove(Vector3 deckPos, int deckCapa, string deckName)
    {
        // ①カードの移動
        for (int index = 0; index < deckCapa; index++)
        {
            // 移動させるカード
            var moveCard = deckList[_cardDeckIndex].Value[index];
            moveCard.transform.position = new Vector3(deckPos.x, deckPos.y + (index + 1) * cardGapY, deckPos.z + cardStartPosZ + index * cardGapZ);
        }

        // ➁移動したカードをバックデッキリストへ追加し、元のデッキから削除
        // deckList内の移動先デッキのIndex
        int backDeckIndex = 0;
        for (int index = 0; index < deckCapa; index++)
        {
            for (int deckIndex = 0; deckIndex < deckList.Count; deckIndex++)
            {
                if (deckList[deckIndex].Key == deckName)
                {
                    backDeckIndex = deckIndex;
                    deckList[deckIndex].Value.Add(deckList[_cardDeckIndex].Value[0]);
                    deckList[_cardDeckIndex].Value.RemoveAt(0);
                }
            }
        }

        // ③移動したカードの回転
        OpenCard(backDeckIndex);
    }

    /// <summary>
    /// デッキ一番上のカードを表向きにする
    /// </summary>
    /// <param name="deckIndex">表向きにするカードが所属するデッキ</param>
    public static void OpenCard(int deckIndex)
    {
        int deckLength = deckList[deckIndex].Value.Count;
        if(deckLength > 0)
        {
            deckList[deckIndex].Value[deckLength - 1].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    /// <summary>
    /// デッキのカード配り
    /// </summary>
    private void CardsArrangement()
    {
        List<BackDeck> backDeckList = new List<BackDeck>()
        {
            new BackDeck(){deckName = nameof(back1Deck), deckPos = _back1.transform.position, deckCapa =7},
            new BackDeck(){deckName = nameof(back2Deck), deckPos = _back2.transform.position, deckCapa =6},
            new BackDeck(){deckName = nameof(back3Deck), deckPos = _back3.transform.position, deckCapa =5},
            new BackDeck(){deckName = nameof(back4Deck), deckPos = _back4.transform.position, deckCapa =4},
            new BackDeck(){deckName = nameof(back5Deck), deckPos = _back5.transform.position, deckCapa =3},
            new BackDeck(){deckName = nameof(back6Deck), deckPos = _back6.transform.position, deckCapa =2},
            new BackDeck(){deckName = nameof(back7Deck), deckPos = _back7.transform.position, deckCapa =1},
        };

        foreach (var backDeck in backDeckList)
        {
            CardPositionMove(backDeck.deckPos, backDeck.deckCapa, backDeck.deckName);
        }
    }
}
