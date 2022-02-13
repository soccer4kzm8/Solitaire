using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardMove : MonoBehaviour
{
    /// <summary>rayとの交点を求めるためのplane</summary>
    private Plane plane;

    /// <summary>cardを掴んでいるかのFlg</summary>
    private bool isGrabbing;

    /// <summary>掴んだ（Rayが当たった表向きの）カードの情報を入れるリスト</summary>
    private List<GameObject> grabbedCards;

    /// <summary>Rayが当たったデッキ内の表向きのカードの情報を入れるリスト</summary>
    private List<GameObject> facedUpCards;

    /// <summary>掴んでいるカードの元の位置</summary>
    private List<Vector3> backupCardsPos;

    /// <summary>掴んでいるカードが所属しているデッキのデッキリスト内の番号</summary>
    private int _deckListIndex = 0;

    /// <summary>掴んでいるカードのカードリスト内のindexリスト</summary>
    private List<int> _cardListIndexList;

    /// <summary>Rayが当たった一番奥にある表向きのカード</summary>
    private GameObject grabbingInnnermostCard;


    private void Start()
    {
        plane = new Plane(Vector3.up, Vector3.up);
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

            grabbedCards = new List<GameObject>();
            facedUpCards = new List<GameObject>();
            backupCardsPos = new List<Vector3>();
            // カーソルからのRay
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // 一時的にrayの可視化
            Debug.DrawRay(ray.origin, ray.direction * 15.0f, Color.blue, 20, false);
            var hitObjs = Physics.RaycastAll(ray);
            // rayが当たった順に並び変え
            List<RaycastHit> hitOrderedObjs = new List<RaycastHit>(hitObjs).OrderBy(h => h.distance).ToList();

            if (hitOrderedObjs.Count == 0)
                return;

            // cardDeckがクリックされたら場合
            if (hitOrderedObjs.Last().collider.name == "cardDeck")
            {
                CardFlip.OnClick_CardDeck();
            }
            else
            {
                if (!CheckExistDeck(hitOrderedObjs))
                    return;

                if (!CheckExistCard(hitOrderedObjs))
                    return;

                // rayが当たったデッキ
                GameObject rayHitDeck = hitOrderedObjs.Last().transform.gameObject;
                // rayが当たったデッキ内の一番上のカード
                GameObject topCard = hitOrderedObjs[0].transform.gameObject;

                AddFacedUpCards(rayHitDeck);

                BackUpGrabbingCards(hitOrderedObjs, rayHitDeck, topCard);
            }
        }
    }

    /// <summary>
    /// 掴んでいるカードのバックアップ
    /// </summary>
    /// <param name="hitOrderedObjs">rayが当たったObjリスト。[0]:rayが当たったデッキ内の一番上のカード。Last():rayが当たったデッキ</param>
    private void BackUpGrabbingCards(List<RaycastHit> hitOrderedObjs, GameObject rayHitDeck, GameObject topCard)
    {
        // 掴んでいるカードすべてのバックアップ
        // graveDeckから移動できるカードは1枚だけ
        if (rayHitDeck.name == "graveDeck")
        {
            grabbedCards.Add(topCard);
            backupCardsPos.Add(rayHitDeck.transform.position);
        }
        else
        {
            // rayが一番最初に当たったカードは、rayが当たったデッキ内の何番目にあるかを取得？
           
            int rayFirstHitCardIndex = 0;

            // rayHitFacedUpCards : rayがhitした表向きのカード
            // facedUpCards :       rayがhitしたデッキ内にある表向きのカード
            // facedUpCards
            foreach (var facedUpCard in facedUpCards)
            {
                if (topCard.name == facedUpCard.name)
                    break;

                rayFirstHitCardIndex++;
            }

            for (int i = rayFirstHitCardIndex; i < facedUpCards.Count; i++)
            {
                grabbedCards.Add(facedUpCards[i]);
                backupCardsPos.Add(facedUpCards[i].transform.position);
            }
        }


        if (grabbedCards.Count > 0)
            isGrabbing = true;

        // Rayが当たった一番奥のカードをバックアップ
        if (grabbedCards.Count >= 1)
        {
            grabbingInnnermostCard = grabbedCards[0];
        }

        if (grabbedCards.Count > 0)
        {
            SearchgrabbedCards();
        }
    }

    /// <summary>
    /// Rayの当たったオブジェクト内にデッキは存在するか
    /// </summary>
    /// <param name="hitOrderedObjs"></param>
    /// <returns></returns>
    private bool CheckExistDeck(List<RaycastHit> hitOrderedObjs)
    {
        foreach (var i in hitOrderedObjs)
        {
            if (i.collider.GetComponent<Deck>() != null)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Rayの当たったオブジェクト内にカードは存在するか
    /// </summary>
    /// <param name="hitOrderedObjs"></param>
    /// <returns></returns>
    private bool CheckExistCard(List<RaycastHit> hitOrderedObjs)
    {
        foreach (var i in hitOrderedObjs)
        {
            if (i.collider.GetComponent<Card>() != null)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Rayが当たったデッキ内の表向きのカードをリストに追加
    /// </summary>
    /// <param name="hitOrderedObjs"></param>
    private void AddFacedUpCards(GameObject rayHitDeck)
    {
        // Rayが当たったデッキ
        List<GameObject> targetDeck = new List<GameObject>();

        GameManager.deckList.ForEach(deck =>
        {
            if (deck.Key == rayHitDeck.name)
            {
                deck.Value.ForEach(card =>
                {
                    targetDeck.Add(card);
                });
            }
        });

        targetDeck.ForEach(card =>
        {
            if (card.transform.rotation.x == 0)
                facedUpCards.Add(card);
        });
    }



    /// <summary>
    /// Rayがhitした表向きのカードのindexの取得
    /// </summary>
    private void SearchgrabbedCards()
    {
        _cardListIndexList = new List<int>();

        // デッキリスト
        var deckList = GameManager.deckList;
        int deckListIndex = 0;
        foreach (var deck in deckList)
        {
            int cardListIndex = 0;
            foreach (var card in deckList[deckListIndex].Value)
            {
                foreach (var hitFacedUpCard in grabbedCards)
                {
                    if (hitFacedUpCard.name == deckList[deckListIndex].Value[cardListIndex].name)
                    {
                        this._deckListIndex = deckListIndex;
                        _cardListIndexList.Add(cardListIndex);
                    }
                }
                cardListIndex++;
            }
            deckListIndex++;
        }
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
            float rayDistance;
            // PlaneにRayを飛ばす
            plane.Raycast(ray, out rayDistance);

            // 掴んでいるカードをRayが当たった位置へ移動
            int indexForGap = 0;
            foreach (var hitFacedUpCard in grabbedCards)
            {
                var mousePos = ray.GetPoint(rayDistance);
                hitFacedUpCard.transform.position = new Vector3(mousePos.x, mousePos.y + indexForGap * GameManager.cardGapY, mousePos.z + indexForGap * GameManager.cardGapZ);
                indexForGap++;
            }

            // 左クリックを放したとき
            if (Input.GetMouseButtonUp(0))
            {
                // 一時的にrayの可視化
                Debug.DrawRay(ray.origin, ray.direction * 15.0f, Color.red, 20, false);
                MouseButtonUp(ray);
            }
        }
    }

    /// <summary>
    /// 左クリックを放したとき
    /// </summary>
    /// <param name="ray">レイ</param>
    private void MouseButtonUp(Ray ray)
    {
        isGrabbing = false;
        // rayがhitしたobjが入っている配列
        RaycastHit[] hitObjs = Physics.RaycastAll(ray);
        List<RaycastHit> hitOrderedObjs = new List<RaycastHit>(hitObjs).OrderBy(h => h.distance).ToList();

        // 掴んでいるカードを動かせないなら元の位置に戻す
        if (JudgeMove(hitOrderedObjs) == false)
        {
            ReturnOriginPos();
        }
    }

    /// <summary>
    /// 掴んでいるカードを動かせれるか
    /// </summary>
    /// <param name="hitObjects">rayがhitしたobjが入っている配列</param>
    private bool JudgeMove(List<RaycastHit> hitObjs)
    {
        for (int index = 0; index < hitObjs.Count; index++)
        {
            if (hitObjs[index].collider.GetComponent<Deck>() != null)
            {
                return JudgeFrontOrBack(hitObjs, index);
            }
        }

        return false;
    }

    /// <summary>
    /// 移動先がFrontかBackか
    /// </summary>
    /// <param name="hitObjects">rayがhitしたobj配列</param>
    /// <param name="deckIndexInHitObjects">rayがhitしたobj配列内のdeckIndex</param>
    /// <returns></returns>
    private bool JudgeFrontOrBack(List<RaycastHit> hitObjs, int deckIndexInHitObjects)
    {
        // 移動先デッキ情報
        GameObject addedDeck = hitObjs[deckIndexInHitObjects].transform.gameObject;
        // 移動先がFrontDeckかどうか
        bool isFrontDeck = addedDeck.GetComponent<Deck>().isFrontDeck;
        // 移動先のDeck名
        string addedDeckName = addedDeck.name;
        // 掴んでいるカードの一番奥の記号
        string holdSuit = grabbingInnnermostCard.GetComponent<Card>().suit;
        // 掴んでいるカードの一番奥の数字
        int holdNum = grabbingInnnermostCard.GetComponent<Card>().num;

        // FrontDeckに移動させるとき
        if (isFrontDeck)
        {
            return FrontDeckProcess(holdSuit, holdNum, addedDeck);
        }
        // BackDeckに移動させるとき
        else if (isFrontDeck == false)
        {
            return BackDeckProcess(holdSuit, holdNum, addedDeckName, addedDeck);
        }

        return false;
    }

    /// <summary>
    /// 掴んでいるカードを元の位置に戻す
    /// </summary>
    private void ReturnOriginPos()
    {
        int hitFacedUpCardIndex = 0;
        foreach (var hitFacedUpCard in grabbedCards)
        {
            int backupCardsPosIndex = 0;
            foreach (var backupCardPos in backupCardsPos)
            {
                if (hitFacedUpCardIndex == backupCardsPosIndex)
                {
                    hitFacedUpCard.transform.position = backupCardPos;
                }
                backupCardsPosIndex++;
            }
            hitFacedUpCardIndex++;
        }
    }

    /// <summary>
    /// FrontDeckに移動させるときの処理
    /// </summary>
    /// <param name="holdSuit">掴んでいるカードの一番奥の記号</param>
    /// <param name="holdNum">掴んでいるカードの一番奥の数字</param>
    /// <param name="addedDeckInfo">移動先デッキ情報</param>
    private bool FrontDeckProcess(string holdSuit, int holdNum, GameObject addedDeck)
    {
        if (JudgeMoveToFront(holdNum, holdSuit))
        {
            CardTransform(addedDeck);
            return true;
        }
        return false;
    }

    /// <summary>
    /// BackDeckに移動させるときの処理
    /// </summary>
    /// <param name="holdSuit">掴んでいるカードの記号</param>
    /// <param name="holdNum">掴んでいるカードの数字</param>
    /// <param name="hitObjects">rayがhitしたobjが入っている配列</param>
    /// <param name="deckName">移動先のデッキ名</param>
    private bool BackDeckProcess(string holdSuit, int holdNum, string deckName, GameObject addedDeck)
    {
        // 持っているカードマークによって場合分け
        if (holdSuit == "club" || holdSuit == "spade")
        {
            var suits = new string[2] { "heart", "diamond" };
            return TransormForBack(holdNum, deckName, suits, addedDeck);
        }
        else if (holdSuit == "heart" || holdSuit == "diamond")
        {
            var suits = new string[2] { "club", "spade" };
            return TransormForBack(holdNum, deckName, suits, addedDeck);
        }

        return false;
    }

    /// <summary>
    /// 掴んでいるカードが黒の時の動きでBackDeckに移動させるとき
    /// </summary>
    /// <param name="holdNum">掴んでいるカードの数字</param>
    /// <param name="hitObjects">rayがhitしたobj配列</param>
    /// <param name="addedDeckName">カードの移動先デッキ名</param>
    /// <returns></returns>
    private bool TransormForBack(int holdNum, string addedDeckName, string[] suits, GameObject addedDeck)
    {
        var testdeck = GameManager.deckList[GetAddedDeckIndex(addedDeckName)].Value;
        if (testdeck.Count == 0)
        {
            if (holdNum == 13)
            {
                CardTransformForBack(addedDeck);
                return true;
            }
        }
        else
        {
            if (CheckSuits(suits, addedDeckName) == false)
                return false;

            if (CheckNum(holdNum, addedDeckName))
            {
                CardTransformForBack(addedDeck);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 移動させようとしているデッキの一番上のカードの数字が掴んでいるカードの一番奥の数字 + 1の数字になっているか
    /// </summary>
    /// <param name="holdNum">掴んでいるカードの一番奥の数字</param>
    /// <param name="addedDeckName">移動させようとしているデッキ名</param>
    /// <returns></returns>
    private bool CheckNum(int holdNum, string addedDeckName)
    {
        if (holdNum + 1 == GetTopCard(addedDeckName).GetComponent<Card>().num)
            return true;

        return false;
    }

    /// <summary>
    /// 移動させようとしているデッキの一番上のカードが赤色か
    /// </summary>
    /// <param name="suits"></param>
    /// <param name="addeddDeckName"></param>
    /// <returns></returns>
    private bool CheckSuits(string[] suits, string addeddDeckName)
    {
        if (GetTopCard(addeddDeckName).GetComponent<Card>() == null)
            return false;

        foreach (var suit in suits)
        {
            if (GetTopCard(addeddDeckName).GetComponent<Card>().suit == suit)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 移動させようとしているデッキの一番上のカード
    /// </summary>
    /// <param name="addeddDeckName">移動させようとしているデッキ名</param>
    /// <returns></returns>
    private GameObject GetTopCard(string addeddDeckName)
    {
        return GameManager.deckList[GetAddedDeckIndex(addeddDeckName)].Value.Last();
    }

    /// <summary>
    /// 掴んでいるカードをFrontDeckに移動させれるかどうか
    /// </summary>
    /// <param name="holdNum">掴んでいるカードの数字</param>
    /// <param name="holdSuit">掴んでいるカードの記号</param>
    /// <returns></returns>
    private bool JudgeMoveToFront(int holdNum, string holdSuit)
    {
        int deckIndex = GetDeckIndex(holdSuit);

        var targetDeck = GameManager.deckList[deckIndex].Value;

        // フロントには昇順で入れる
        if (targetDeck.Count == 0 && holdNum == 1)
        {
            return true;
        }
        else if (targetDeck.Count >= 1)
        {
            if (targetDeck.Last().GetComponent<Card>().num == holdNum - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// DeckList内から移動先DeckのIndexの取得
    /// </summary>
    /// <param name="holdSuit">掴んでいるカードの記号</param>
    /// <returns></returns>
    private int GetDeckIndex(string holdSuit)
    {
        int index = 0;
        int deckIndex = 0;
        foreach (var deck in GameManager.deckList)
        {
            if (deck.Key.Contains(holdSuit))
            {
                deckIndex = index;
            }
            index++;
        }

        return deckIndex;
    }

    /// <summary>
    /// カードの移動
    /// </summary>
    /// <param name="addedDeck">移動先のデッキ情報</param>
    private void CardTransform(GameObject addedDeck)
    {
        var deckPos = addedDeck.transform.position;
        float addedDeckLength = GetAddedDeckList(GetAddedDeckIndex(addedDeck.name)).Value.Count;
        float indexForGap = 1;

        foreach (var hitFacedUpCard in grabbedCards)
        {
            hitFacedUpCard.transform.position = new Vector3(deckPos.x, deckPos.y + (addedDeckLength + indexForGap) * GameManager.cardGapY, deckPos.z);
            indexForGap++;
        }
        ResetDeckList(addedDeck.name);
    }

    /// <summary>
    /// カードの移動
    /// </summary>
    /// <param name="addedDeck">移動先のデッキ情報</param>
    private void CardTransformForBack(GameObject addedDeck)
    {
        var deckPos = addedDeck.transform.position;
        // 移動先デッキ
        var addedDeckInDeckList = GetAddedDeckList(GetAddedDeckIndex(addedDeck.name)).Value;
        float addedDeckLength = addedDeckInDeckList.Count;
        var topCard = addedDeckInDeckList.Last();
        Debug.LogError(topCard.name);
        float indexForGap = 1;

        foreach (var hitFacedUpCard in grabbedCards)
        {
            hitFacedUpCard.transform.position = new Vector3(deckPos.x, deckPos.y + (addedDeckLength + indexForGap) * GameManager.cardGapY, addedDeckLength * GameManager.cardGapZ + deckPos.z + GameManager.cardStartPosZ);
            indexForGap++;
        }
        ResetDeckList(addedDeck.name);
    }

    /// <summary>
    /// 新しいカードが追加されるデッキのDeckList内のindexの取得
    /// </summary>
    /// <param name="addedDeckName">追加されるカードの名前</param>
    /// <returns></returns>
    private int GetAddedDeckIndex(string addedDeckName)
    {
        //新しいカードが追加されるデッキのDeckList内のindex
        int deckIndex = 0;
        int index = 0;
        foreach (var deck in GameManager.deckList)
        {
            if (deck.Key == addedDeckName)
            {
                deckIndex = index;
            }
            index++;
        }
        return deckIndex;
    }

    /// <summary>
    /// 移動させるカードが所属しているデッキのDeckList内のindexの取得
    /// </summary>
    /// <returns></returns>
    private int GetBelongDeckIndex()
    {
        // 移動させるカードが所属しているデッキのDeckList内のindex
        int deckIndex = 0;
        int index = 0;
        foreach (var deck in GameManager.deckList)
        {
            if (index == this._deckListIndex)
            {
                deckIndex = index;
            }
            index++;
        }
        return deckIndex;
    }

    /// <summary>
    /// 移動するカードが所属しているデッキを取得
    /// </summary>
    /// <param name="belongDeckIndex">移動するカードが所属しているデッキのDeckList内のIndex</param>
    /// <returns></returns>
    private List<GameObject> GetBelogDeckList(int belongDeckIndex)
    {
        return GameManager.deckList[belongDeckIndex].Value;
    }

    /// <summary>
    /// 移動先のデッキを取得
    /// </summary>
    /// <param name="addedDeckIndex">移送先デッキのDeckList内のIndex</param>
    /// <returns></returns>
    private KeyValuePair<string, List<GameObject>> GetAddedDeckList(int addedDeckIndex)
    {
        return GameManager.deckList[addedDeckIndex];
    }

    /// <summary>
    /// カード移動後のデッキリストのリセット
    /// </summary>
    /// <param name="addedDeck">移動先のDeck名</param>
    private void ResetDeckList(string addedDeck)
    {
        int belongDeckIndex = GetBelongDeckIndex();
        int addedDeckIndex = GetAddedDeckIndex(addedDeck);
        var belongDeckList = GetBelogDeckList(belongDeckIndex);
        int cardIndex = 0;
        foreach (var card in belongDeckList)
        {
            foreach (var cardIndexInCardList in this._cardListIndexList)
            {
                if (cardIndexInCardList == cardIndex)
                {
                    // 移動先デッキにカードを追加
                    GameManager.deckList[addedDeckIndex].Value.Add(belongDeckList.Last());
                }
            }
            cardIndex++;
        }

        // 移動したカードを所属していたデッキから削除
        // 複数移動に未対応
        GameManager.deckList[belongDeckIndex].Value.RemoveAt(belongDeckList.Count - 1);

        // 移動したカードが所属していたデッキの一番上のカードを表向きにする
        GameManager.OpenCard(belongDeckIndex);
    }
}