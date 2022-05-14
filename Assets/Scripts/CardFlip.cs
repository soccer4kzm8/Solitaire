using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private GameObject _cardDeckImage;
    public static GameManager gameManager;

    /// <summary>Y方向のカードのズレ</summary>
    private const float _cardGapY = 0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        // 参照 : https://gamefbb.com/%E3%80%90unity%E3%80%91%E5%88%A5%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%97%E3%83%88%E3%81%AE%E5%A4%89%E6%95%B0%E3%81%AE%E5%80%A4%E3%82%92%E5%8F%96%E5%BE%97%E3%83%BB%E5%A4%89%E6%9B%B4%E3%81%99%E3%82%8B/
        GameObject script = GameObject.FindWithTag("script");
        gameManager = script.GetComponent<GameManager>();
    }

    void Update()
    {
        ImageEnabled();
    }

    /// <summary>
    /// カードデッキクリックの挙動
    /// </summary>
    public void OnClick_CardDeck()
    {
        int cardDeckIndex = GetDeckIndex("cardDeck");
        int graveDeckIndex = GetDeckIndex("graveDeck");

        // CardDeck内にカードがあるとき
        if (GameManager.deckList[cardDeckIndex].Value.Count > 0)
        {
            Vector3 _graveDeckPos = gameManager._grave.transform.position;
            // カードをデッキから墓地へ移動
            GameManager.deckList[cardDeckIndex].Value[0].transform.position = new Vector3(_graveDeckPos.x, _graveDeckPos.y + (GameManager.graveDeck.Count + 1) * _cardGapY, _graveDeckPos.z);
            // 移動したカードを墓地デッキリストへ追加
            GameManager.deckList[graveDeckIndex].Value.Add(GameManager.deckList[cardDeckIndex].Value[0]);
            // 移動したカードはデッキリストから削除
            GameManager.deckList[cardDeckIndex].Value.RemoveAt(0);
            // カードの回転
            GameManager.OpenCard(graveDeckIndex);
        }
        else
        {
            Vector3 _cardDeckPos = GameObject.Find("cardDeck").transform.position;
            var buckupGraveDeckCount = GameManager.deckList[graveDeckIndex].Value.Count;
            for (var index = 0; index < buckupGraveDeckCount; index++)
            {
                // カードを墓地からデッキへ移動
                GameManager.deckList[graveDeckIndex].Value[index].transform.position = new Vector3(_cardDeckPos.x, _cardDeckPos.y, _cardDeckPos.z);
                // カードの回転
                GameManager.deckList[graveDeckIndex].Value[index].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
                // 移動したカードをデッキリストへ追加
                GameManager.deckList[cardDeckIndex].Value.Add(GameManager.deckList[graveDeckIndex].Value[index]);
            }
            // 移動したカードは墓地リストから削除
            GameManager.deckList[graveDeckIndex].Value.Clear();
        }
    }

    /// <summary>
    /// Imageの有効化、無効化
    /// </summary>
    private void ImageEnabled()
    {
        int cardDeckIndex = GetDeckIndex("cardDeck");

        if (GameManager.deckList[cardDeckIndex].Value.Count == 0)
        {
            // デッキの枚数が0枚になったらボタンの可視化
            _cardDeckImage.GetComponent<Image>().enabled = true;
        }
        else
        {
            _cardDeckImage.GetComponent<Image>().enabled = false;
        }
    } 

    /// <summary>
    /// deckList内の指定DeckのIndex取得
    /// </summary>
    /// <param name="deckName">指定Deck名</param>
    /// <returns></returns>
    private static int GetDeckIndex(string deckName)
    {
        int deckIndex = 0;
        int index = 0;
        foreach (var deck in GameManager.deckList)
        {
            if (deck.Key == deckName)
            {
                deckIndex = index;
            }
            index++;
        }

        return deckIndex;
    }
}
