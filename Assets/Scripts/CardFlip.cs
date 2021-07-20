using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _cardDeckImage;

    /// <summary>Y方向のカードのズレ</summary>
    private const float _cardGapY = 0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        // 参照 : https://gamefbb.com/%E3%80%90unity%E3%80%91%E5%88%A5%E3%82%B9%E3%82%AF%E3%83%AA%E3%83%97%E3%83%88%E3%81%AE%E5%A4%89%E6%95%B0%E3%81%AE%E5%80%A4%E3%82%92%E5%8F%96%E5%BE%97%E3%83%BB%E5%A4%89%E6%9B%B4%E3%81%99%E3%82%8B/
        GameObject script = GameObject.FindWithTag("script");
        _gameManager = script.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.cardDeck.Count == 0)
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
    /// カードデッキクリックの挙動
    /// </summary>
    public void OnClick_CardDeck()
    {
        if (GameManager.cardDeck.Count > 0)
        {
            Vector3 _graveDeckPos = _gameManager._grave.transform.position;
            // カードをデッキから墓地へ移動
            GameManager.cardDeck[0].transform.position = new Vector3(_graveDeckPos.x, _graveDeckPos.y + GameManager.graveDeck.Count * _cardGapY, _graveDeckPos.z);
            // カードの回転
            GameManager.cardDeck[0].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            // 移動したカードを墓地デッキリストへ追加
            GameManager.graveDeck.Add(GameManager.cardDeck[0]);
            // 移動したカードはデッキリストから削除
            GameManager.cardDeck.RemoveAt(0);
        }
        else
        {
            Vector3 _cardDeckPos = GameObject.Find("CardDeck").transform.position;
            var buckupGraveDeckCount = GameManager.graveDeck.Count;
            for(var index = 0; index < buckupGraveDeckCount; index++)
            {
                // カードを墓地からデッキへ移動
                GameManager.graveDeck[index].transform.position = new Vector3(_cardDeckPos.x, _cardDeckPos.y, _cardDeckPos.z);
                // カードの回転
                GameManager.graveDeck[index].transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                // 移動したカードをデッキリストへ追加
                GameManager.cardDeck.Add(GameManager.graveDeck[index]);
            }
            // 移動したカードは墓地リストから削除
            GameManager.graveDeck.Clear();
        }
    }
}
