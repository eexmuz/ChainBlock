using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConstructorBlock))]
public class ConstructorBlockEditor : Editor
{
    private Dictionary<int, string> _directions = new Dictionary<int, string>
    {
        {1, "UP"},
        {3, "LEFT"},
        {5, "RIGHT"},
        {7, "DOWN"},
    };
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        ConstructorBlock block = target as ConstructorBlock;

        DrawBlockPOT(block);
        DrawFreezeFlag(block);
        DrawMergeableFlag(block);
        DrawMovementPanel(block);
    }

    private void DrawFreezeFlag(ConstructorBlock block)
    {
        if (block.BlockData.Mergeable == false)
        {
            return;
        }
        
        bool movable = !EditorGUILayout.Toggle("Frozen", block.BlockData.Movable == false);
        if (movable == block.BlockData.Movable)
        {
            return;
        }

        block.SetMovable(movable);
    }

    private void DrawMergeableFlag(ConstructorBlock block)
    {
        bool mergeable = !EditorGUILayout.Toggle("Barrier", block.BlockData.Mergeable == false);
        if (mergeable == block.BlockData.Mergeable)
        {
            return;
        }

        block.SetMergeable(mergeable);
    }

    private void DrawBlockPOT(ConstructorBlock block)
    {
        if (block.BlockData.Mergeable == false)
        {
            return;
        }
        
        int pot = EditorGUILayout.IntField("Power of Two", block.BlockData.PowerOfTwo);
        if (pot == block.BlockData.PowerOfTwo)
        {
            return;
        }
        
        pot = Mathf.Clamp(pot, 0, 30);
        block.SetNumber(pot);
    }

    private void DrawMovementPanel(ConstructorBlock block)
    {
        GUIStyle buttonStyle = GUI.skin.button;
        GUIStyle emptyStyle = GUI.skin.box;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        bool left = false, right = false, up = false, down = false;
        Direction direction = Direction.Down;
        
        for (int y = 0; y < 3; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 3; x++)
            {
                string display = _directions.ContainsKey(y * 3 + x) ? _directions[y * 3 + x] : "";
                int dir = y * 3 + x;
                switch (dir)
                {
                    case 1:
                        up = GUILayout.Button(display, buttonStyle, GUILayout.Width(50f), GUILayout.Height(50f));
                        if (up) direction = Direction.Up;
                        break;
                    case 3:
                        left = GUILayout.Button(display, buttonStyle, GUILayout.Width(50f), GUILayout.Height(50f));
                        if (left) direction = Direction.Left;
                        break;
                    case 5:
                        right = GUILayout.Button(display, buttonStyle, GUILayout.Width(50f), GUILayout.Height(50f));
                        if (right) direction = Direction.Right;
                        break;
                    case 7:
                        down = GUILayout.Button(display, buttonStyle, GUILayout.Width(50f), GUILayout.Height(50f));
                        if (down) direction = Direction.Down;
                        break;
                    default:
                        GUILayout.Box(display, emptyStyle, GUILayout.Width(50f), GUILayout.Height(50f));
                        break;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        if (up || down || right || left)
        {
            block.Move(direction);
        }
    }
}
