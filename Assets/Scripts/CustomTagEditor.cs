using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(CustomTag))]
public class CustomTagEditor : Editor
{
    /// <summary>タグList</summary>
    private string[] unityTags;
    SerializedProperty tagsProp;
    private ReorderableList list;

    private void OnEnable()
    {
        unityTags = InternalEditorUtility.tags;
        tagsProp = serializedObject.FindProperty("tags");

        // ReorderableListの参照 : https://kan-kikuchi.hatenablog.com/entry/ReorderableList
        // elements              : 要素
        // elementType           : 要素の種類
        // draggable             : ドラッグして要素を入れ替えられるか
        // displayHeader         : ヘッダーを表示するか
        // displayAddButton      : 要素追加用の + ボタンを表示するか
        // displayRemoveButton   : 要素削除用の - ボタンを表示するか 
        list = new ReorderableList(serializedObject, tagsProp, true, true, true, true);

        // ヘッダーの描画(diaplayHeaderをfalseにしてても有効)
        list.drawHeaderCallback += DrawHeader;
        // 要素の描画
        list.drawElementCallback += DrawElement;
        // + ボタンにメニューを追加
        list.onAddDropdownCallback += OnAddDropdown;
    }

    /// <summary>
    /// ヘッダーの描画
    /// </summary>
    /// <param name="rect">ラベルの表示される位置</param>
    private void DrawHeader(Rect rect)
    {
        // position : ラベルの表示される位置
        // label    : Label Fieldの前のラベル
        // style    : ラベルを表示するためのStyle情報(フォント、色など)
        EditorGUI.LabelField(rect, new GUIContent("Tags"), EditorStyles.boldLabel);
    }

    /// <summary>
    /// 要素の描画
    /// </summary>
    /// <param name="rect">要素の表示される位置</param>
    /// <param name="index">要素番号</param>
    /// <param name="isActive"></param>
    /// <param name="isFocused"></param>
    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        // listの指定したindexにあるelementを返す
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.LabelField(rect, element.stringValue);
    }

    /// <summary>
    /// + ボタンにメニューを追加
    /// </summary>
    /// <param name="buttonRect"></param>
    /// <param name="list"></param>
    private void OnAddDropdown(Rect buttonRect, ReorderableList list)
    {
        // 追加するメニューの作成
        GenericMenu menu = new GenericMenu();

        // タグの数だけ繰り返し
        for (int i = 0; i < unityTags.Length; i++)
        {
            var label = new GUIContent(unityTags[i]);

            // Don't allow duplicate tags to be added.
            if (PropertyContainsString(tagsProp, unityTags[i]))
                menu.AddDisabledItem(label);
            else
                menu.AddItem(label, false, OnAddClickHandler, unityTags[i]);
        }

        menu.ShowAsContext();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="property"></param>
    /// <param name="value">タグの要素</param>
    /// <returns></returns>
    private bool PropertyContainsString(SerializedProperty property, string value)
    {
        // 配列かどうか
        if (property.isArray)
        {
            for (int i = 0; i < property.arraySize; i++)
            {
                if (property.GetArrayElementAtIndex(i).stringValue == value)
                    return true;
            }
        }
        else
            return property.stringValue == value;

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    private void OnAddClickHandler(object tag)
    {
        int index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;

        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.stringValue = (string)tag;
        // プロパティの変更の適用
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        // 要素と要素の間のスペースを6pixels空ける
        GUILayout.Space(6);
        // 内部キャッシュから値をロードする
        serializedObject.Update();
        // 描画
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(3);
    }
}
