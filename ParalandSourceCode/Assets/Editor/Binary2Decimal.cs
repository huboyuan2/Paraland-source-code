using UnityEditor;
using UnityEngine;

public class Binary2Decimal : EditorWindow
{
    private bool[] toggles = new bool[25];

    [MenuItem("Custom Tools/Binary to Decimal Converter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Binary2Decimal));
    }
    [MenuItem("Custom Tools/TestSelect")]
    public static void TestSelect()
    {
        foreach (var obj in Selection.gameObjects)
        {
            Debug.Log(obj.name);
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Binary to Decimal Converter", EditorStyles.boldLabel);

        for (int i = 0; i < 5; i++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < 5; j++)
            {
                int index = i * 5 + j;
                toggles[index] = EditorGUILayout.Toggle(toggles[index]);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Convert to Decimal"))
        {
            ConvertToDecimal();
        }
    }

    private void ConvertToDecimal()
    {
        string binaryString = "";

        // 根据 Toggle 的勾选状态确定对应的二进制数字
        for (int i = 0; i < toggles.Length; i++)
        {
            binaryString += toggles[i] ? "1" : "0";
        }

        // 将二进制字符串转换为十进制整数
        int decimalNumber = System.Convert.ToInt32(binaryString, 2);

        Debug.Log("Binary: " + binaryString + ", Decimal: " + decimalNumber);
    }
}