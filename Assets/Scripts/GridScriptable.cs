using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class GridScriptable : ScriptableObject
{
    public Sprite FullBox;
    public Sprite UpperLeftBox;
    public Sprite UpperMiddleBox;
    public Sprite UpperRightBox;
    public Sprite MiddleLeftBox;
    public Sprite MiddleBox;
    public Sprite MiddleRightBox;
    public Sprite BottonLeftBox;
    public Sprite BottonMiddleBox;
    public Sprite BottonRightBox;
}
[CustomEditor(typeof(GridScriptable))]
public class SpriteGridEditor : Editor
{
    GridScriptable grid;
    private void OnEnable() => grid = target as GridScriptable;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (grid.FullBox == null)
            return;

        //Convert the weaponSprite (see SO script) to Texture
        Texture2D fullBox = AssetPreview.GetAssetPreview(grid.FullBox);
        Texture2D upperLeft = AssetPreview.GetAssetPreview(grid.UpperLeftBox);
        Texture2D upperMiddle = AssetPreview.GetAssetPreview(grid.UpperMiddleBox);
        Texture2D upperRight = AssetPreview.GetAssetPreview(grid.UpperRightBox);
        Texture2D middleLeft = AssetPreview.GetAssetPreview(grid.MiddleLeftBox);
        Texture2D middle = AssetPreview.GetAssetPreview(grid.MiddleBox);
        Texture2D middleRight = AssetPreview.GetAssetPreview(grid.MiddleRightBox);
        Texture2D bottomLeft = AssetPreview.GetAssetPreview(grid.BottonLeftBox);
        Texture2D bottonMiddle = AssetPreview.GetAssetPreview(grid.BottonMiddleBox);
        Texture2D bottonRight = AssetPreview.GetAssetPreview(grid.BottonRightBox);


        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(0, 280), new Vector2(100, 100)), upperLeft);

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(105, 280), new Vector2(100, 100)), upperMiddle);

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(210, 280), new Vector2(100, 100)), upperRight);

        //----------------------------------------------------------------------------------

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(0, 385), new Vector2(100, 100)), middleLeft);

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(105, 385), new Vector2(100, 100)), middle);

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(210, 385), new Vector2(100, 100)), middleRight);

        //----------------------------------------------------------------------------------

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(0, 490), new Vector2(100, 100)), bottomLeft);

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(105, 490), new Vector2(100, 100)), bottonMiddle);

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(210, 490), new Vector2(100, 100)), bottonRight);

        //----------------------------------------------------------------------------------

        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(new Rect(new Vector2(350, 385), new Vector2(100, 100)), fullBox);
    }
}
