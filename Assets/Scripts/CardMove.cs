using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMove : MonoBehaviour
{
    /// <summary>rayとの交点を求めるためのplane</summary>
    private Plane plane;
    /// <summary>cardを掴んでいるかのFlg</summary>
    private bool isGrabbing;
    /// <summary>掴んだcardのtransform情報</summary>
    private Transform card;
    /// <summary>hitしたcardの名前</summary>
    private string hitCardName;
    /// <summary>カードの元の位置</summary>
    private Vector3 buckupCardPos;
    /// <summary>rayがhitしたカードがデッキに入っているかのFlg</summary>
    //private bool inBack1Deck = false;
    //private bool inBack2Deck = false;
    //private bool inBack3Deck = false;
    //private bool inBack4Deck = false;
    //private bool inBack5Deck = false;
    //private bool inBack6Deck = false;
    //private bool inBack7Deck = false;
    //private bool inClubDeck = false;
    //private bool inHeartDeck = false;
    //private bool inSpadeDeck = false;
    //private bool inDiamondDeck = false;
    //private bool inGraveDeck = false;
    //private bool inCardDeck = false;
    /// <summary>rayがhitしたカードがデッキに入っているかのFlgのリスト</summary>
    //private List<bool> inDeck;

    private int _deckListIndex = 0;
    private int _cardListIndex = 0;

    // Rayが当たったオブジェクトの情報を入れる箱
    private RaycastHit hitCard;

    [SerializeField] private CustomTag _customTag;

    private void Start()
    {
        plane = new Plane(Vector3.up, Vector3.up);
        //inDeck = new List<bool> { inBack1Deck, inBack2Deck, inBack3Deck, inBack4Deck, inBack5Deck, inBack6Deck, inBack7Deck,
        //                          inClubDeck, inHeartDeck, inSpadeDeck, inDiamondDeck, inGraveDeck, inCardDeck};
    }

    private void Update()
    {
        MouseButtonDownAction();

        GrabbingAction();
    }

    /// <summary>
    /// 左クリック押したとき
    /// </summary>
    private void MouseButtonDownAction()
    {
        // 左クリックを押したとき
        if (Input.GetMouseButtonDown(0))
        {
            // カーソルからのRay
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ray = rayの開始地点
            // hit = rayが当たったオブジェクトを格納
            // Mathf.Infinity = rayの発射距離
            if (Physics.Raycast(ray, out hitCard, Mathf.Infinity))
            {
                // クリックしたObjがカードの場合、かつ、表向きの場合
                if (hitCard.collider.tag == "card" && hitCard.transform.rotation.x == 0)
                {
                    hitCardName = hitCard.collider.gameObject.name;
                    //Debug.LogError(hitCardName);
                    SearchCard();
                    isGrabbing = true;
                    // 掴んだcardのTransformの情報を代入
                    card = hitCard.transform;
                }
            }

            // カードの最初の位置をバックアップ取っておく
            buckupCardPos = hitCard.transform.position;
        }
    }

    /// <summary>
    /// Rayがhitしたカードがどのデッキに入っているかを検索
    /// </summary>
    private void SearchCard()
    {
        //Debug.Log(GameManager.cardDeck.Count);
        for (var deckListIndex = 0; deckListIndex < GameManager.deckList.Count; deckListIndex++)
        {
            for (var cardListIndex = 0; cardListIndex < GameManager.deckList[deckListIndex].Count; cardListIndex++)
            {
                if (hitCardName == GameManager.deckList[deckListIndex][cardListIndex].name)
                {
                    //Debug.Log(GameManager.cardDeck.Count);
                    foreach(var i in GameManager.deckList[deckListIndex])
                    {
                        Debug.Log(i);
                    }

                    Debug.Log($"掴んだカードがあるデッキ番号：{deckListIndex}");
                    Debug.Log($"掴んだカードがあるデッキ内のカード番号：{cardListIndex}");

                    Debug.Log($"掴んだカード：{GameManager.deckList[deckListIndex][cardListIndex].name}");

                    this._deckListIndex = deckListIndex;
                    this._cardListIndex = cardListIndex;
                }
            }
        }

        //Debug.LogError("back1Deck");
        //foreach (var i in GameManager.back1Deck)
        //{
        //    if(hitCardName == i.name)
        //    {
        //        inBack1Deck = true;
        //        Debug.LogError(inBack1Deck);
        //    }
        //}

        //Debug.LogError("back2Deck");
        //foreach (var i in GameManager.back2Deck)
        //{
        //    if (hitCardName == i.name)
        //    {
        //        inBack2Deck = true;
        //        Debug.LogError(inBack2Deck);
        //    }
        //}

        //Debug.LogError("back3Deck");
        //foreach (var i in GameManager.back3Deck)
        //{
        //    if (hitCardName == i.name)
        //    {
        //        inBack3Deck = true;
        //        Debug.LogError(inBack3Deck);
        //    }
        //}

        //Debug.LogError("back4Deck");
        //foreach (var i in GameManager.back4Deck)
        //{
        //    if (hitCardName == i.name)
        //    {
        //        inBack4Deck = true;
        //        Debug.LogError(inBack4Deck);
        //    }
        //}

        //Debug.LogError("back5Deck");
        //foreach (var i in GameManager.back5Deck)
        //{
        //    if (hitCardName == i.name)
        //    {
        //        inBack5Deck = true;
        //        Debug.LogError(inBack5Deck);
        //    }
        //}

        //Debug.LogError("back6Deck");
        //foreach (var i in GameManager.back6Deck)
        //{
        //    if (hitCardName == i.name)
        //    {
        //        inBack6Deck = true;
        //        Debug.LogError(inBack6Deck);
        //    }
        //}

        //Debug.LogError("back7Deck");
        //foreach (var i in GameManager.back7Deck)
        //{
        //    if (hitCardName == i.name)
        //    {
        //        inBack7Deck = true;
        //        Debug.LogError(inBack7Deck);
        //    }
        //}
    }

    /// <summary>
    /// カードを掴んでいるとき
    /// </summary>
    private void GrabbingAction()
    {
        // カードを掴んでいるとき
        if (isGrabbing == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // 一時的にrayの可視化
            Debug.DrawRay(ray.origin, ray.direction * 15.0f, Color.blue, 20, false);

            float rayDistance;
            // PlaneにRayを飛ばす
            plane.Raycast(ray, out rayDistance);

            // cardをRayが当たった位置へ移動
            card.position = ray.GetPoint(rayDistance);

            // 左クリックを放したとき
            if (Input.GetMouseButtonUp(0))
            {
                isGrabbing = false;
                // rayがhitしたobjが入っている配列
                var hitObj = Physics.RaycastAll(ray);

                //foreach(var i in hitObj)
                //{
                //    Debug.LogError(i.collider.tag);
                //}

                // club
                // hitObj配列の何番目にFrontデッキの情報が入っているかのIndex
                int clubFrontIndex = 0;
                // hitObj配列の中にFrontデッキが入っているかのFlag。true = 入っている。
                bool clubFrontExistFlg = false;
                // heart
                int heartFrontIndex = 0;
                bool heartFrontExistFlg = false;
                // spade
                int spadeFrontIndex = 0;
                bool spadeFrontExistFlg = false;
                // diamond
                int diamondFrontIndex = 0;
                bool diamondFrontExistFlg = false;

                // rayがhitしたobjの中からtagが一致するindexを取得
                for (int index = 0; index < hitObj.Length; index++)
                {
                    if (hitObj[index].collider.tag == "clubFront")
                    {
                        clubFrontIndex = index;
                        clubFrontExistFlg = true;
                    }
                    else if (hitObj[index].collider.tag == "heartFront")
                    {
                        heartFrontIndex = index;
                        heartFrontExistFlg = true;
                    }
                    else if (hitObj[index].collider.tag == "spadeFront")
                    {
                        spadeFrontIndex = index;
                        spadeFrontExistFlg = true;
                    }
                    else if (hitObj[index].collider.tag == "diamondFront")
                    {
                        diamondFrontIndex = index;
                        diamondFrontExistFlg = true;
                    }
                }

                // 掴んでいるカードのtag
                var holdMultiTag = hitCard.collider.GetComponent<CustomTag>();
                // 持っているカードマークがFrontのマークと同じデッキにRayが当たっているとき、掴んでいるカードをrayが当たっているObjに移動
                if (holdMultiTag.HasTag("club") && clubFrontExistFlg)
                {
                    card = hitObj[clubFrontIndex].transform;
                    // 移動したら元のデッキから削除し、新しいデッキに入れる
                }
                else if (holdMultiTag.HasTag("heart") && heartFrontExistFlg)
                {
                    card = hitObj[heartFrontIndex].transform;
                }
                else if (holdMultiTag.HasTag("spade") && spadeFrontExistFlg)
                {
                    card = hitObj[spadeFrontIndex].transform;
                }
                else if (holdMultiTag.HasTag("diamond") && diamondFrontExistFlg)
                {
                    card = hitObj[diamondFrontIndex].transform;
                }
                else
                {
                    // 元の位置に戻す
                    card.position = buckupCardPos;
                }
            }
        }
    }

    private void Test()
    {
        //// どのBackデッキからカードを掴んだのかを検索
        //for (var index = 0; index < inDeck.Count; index++)
        //{
        //    if (inDeck[index] == true)
        //    {
        //        // 元のデッキからカードを削除
        //        GameManager.deckList[index].RemoveAt(0);
        //        // 移動先のデッキにカードを追加
        //    }
        //}

        for(var deckListIndex = 0; deckListIndex < GameManager.deckList.Count; deckListIndex++)
        {
            if(deckListIndex == this._deckListIndex)
            {
                for(var cardListIndex = 0; cardListIndex < GameManager.deckList[deckListIndex].Count; deckListIndex++)
                {
                    if(cardListIndex == this._cardListIndex)
                    {
                        // 移動させたカードを元デッキから削除
                        GameManager.deckList[deckListIndex].RemoveAt(0);
                        // 移動させたカードを移動先のデッキに追加
                    }
                }
            }
        }
    }
}