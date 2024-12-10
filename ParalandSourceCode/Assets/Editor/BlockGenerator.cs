//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//using System.Collections.Generic;
//using System;

//public class BlockGenerator : EditorWindow
//{
//    private int tempcount = 0;
//    private int blockPrefabCount = 0;
//    private int[,] blocks = new int[5, 5];
//    private List<GameObject> prefabsToInstantiate = new List<GameObject>();
//    private List<Voxel> prefabVoxels = new List<Voxel>(); //�洢ÿ��prefab��voxel

//    //private List<Tuple<int, int>> dirs = new List<Tuple<int, int>>() {new Tuple<int, int>(1,0), new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, 1), new Tuple<int, int>(0, -1) };
//    private List<bool[,]> prefabConnects = new List<bool[,]>(); //�洢ÿ��prefab����ǰ�������ĸ��������ͨ�� 
//    [MenuItem("Custom Tools/Block Generator")]
//    public static void ShowWindow()
//    {
//        EditorWindow.GetWindow(typeof(BlockGenerator));
//    }


//    private void OnGUI()
//    {
//        GUILayout.Label("Binary to Decimal Converter", EditorStyles.boldLabel);
//        blockPrefabCount = EditorGUILayout.IntField("blockPrefabCount", blockPrefabCount);

//        for (int i = 0; i < blockPrefabCount; i++)
//        {
//            if (i >= prefabsToInstantiate.Count)
//                prefabsToInstantiate.Add(null);
//            prefabsToInstantiate[i] = EditorGUILayout.ObjectField("Prefab to Instantiate", prefabsToInstantiate[i], typeof(GameObject), false) as GameObject;
//        }
//        //prefabsToInstantiate.RemoveRange();


//        for (int i = 0; i < 5; i++)
//        {
//            EditorGUILayout.BeginHorizontal();

//            for (int j = 0; j < 5; j++)
//            {
//                blocks[i, j] = EditorGUILayout.IntField(blocks[i, j]);
//            }

//            EditorGUILayout.EndHorizontal();
//        }

//        if (GUILayout.Button("GenerateBlocks"))
//        {
//            GenerateBlocks();
//        }
//        if (GUILayout.Button("ClearAllToggles"))
//        {
//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    blocks[i, j] = -1;
//                }
//            }
            
//        }

//        if (GUILayout.Button("GetRandomMap"))
//        {
//            GetRandomMap(0,0);
//        }
//        if (GUILayout.Button("GetRandomMap2"))
//        {
//            GetRandomMap(2, 2);
//        }


//        if (GUILayout.Button("TSET"))
//        {
//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    blocks[i, j] = -1;
//                }
//            }
//            blocks[0, 0] = 0;
//            blocks[1, 0] = 1;
//            blocks[0, 1] = 2;
//        }
//    }

//    private void GenerateBlocks()
//    {
//        Debug.Log("GenerateBlocks");
//        for (int i = 0; i < 5; i++)
//        {
//            for (int j = 0; j < 5; j++)
//            {
//                Debug.Log("GenerateBlocks1");
//                if (blocks[i, j]>=0&& blocks[i, j]< prefabsToInstantiate.Count)
//                {
//                    Debug.Log("GenerateBlocks2");
//                    GameObject block = GetAppropriateBlock(blocks[i, j]);
//                    if(null==block)
//                     block = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                    block.transform.position = new Vector3(j * 15f,0, i*-15f);
//                    Undo.RegisterCreatedObjectUndo(block, "Create Block");
//                }
//            }               
//        }
//    }
//    //TODO:�ò����������㷨�ҳ����ʵ�Block
//    //����5*5��ͼ��һ������⣬���dfs���涨-1����Ϊ�ø��Ӳ������κ�prefab��-2Ϊ�ø�����δ��������
//    private void GetRandomMap(int x_first, int y_first)
//    {
//        //�Ƚ���ͼȫ�����Ϊδ����
//        for (int i = 0; i < 5; i++)
//        {
//            for (int j = 0; j < 5; j++)
//            {
//                blocks[i, j] = -2;
//            }
//        }
//        //prefabVoxels�洢ÿ��prefab��Voxel
//        if (prefabVoxels.Count == 0)
//        {
//            for (int i = 0; i < blockPrefabCount; i++)
//            {
//                prefabVoxels.Add(prefabsToInstantiate[i].GetComponent<Voxel>());
//            }
//        }
//        else
//        {
//            for (int i = 0; i < blockPrefabCount; i++)
//            {
//                prefabVoxels[i] = prefabsToInstantiate[i].GetComponent<Voxel>();
//            }
//        }




//        //bool[,] prefabConnects0 = new bool[blockPrefabCount, blockPrefabCount];
//        //bool[,] prefabConnects1 = new bool[blockPrefabCount, blockPrefabCount];
//        //bool[,] prefabConnects2 = new bool[blockPrefabCount, blockPrefabCount];
//        //bool[,] prefabConnects3 = new bool[blockPrefabCount, blockPrefabCount];

//        //����prefabConnects���������洢����ÿ����prefabǰ�������ĸ�����Ŀ���ͨ�ԣ�trueΪ����ͨ��falseΪ���ɡ���ǰ���������ݡ�
//        if (prefabConnects.Count == 0)
//        {
//            for (int i = 0; i < 4; i++)
//            {
//                prefabConnects.Add(new bool[blockPrefabCount, blockPrefabCount]);
//            }
//        }

//        for (int i = 0; i < blockPrefabCount; i++)
//        {
//            for (int j = 0; j < blockPrefabCount; j++)
//            {
//                for (int k = 0; k < 4; k++)
//                {
//                    JudgeConnection(i, j, k);
//                }
//            }
//        }

//        dfs_first(x_first, y_first);


//        GenerateBlocks();
//        //Debug.Log("done");
//    }

//    //��������prefab�������x��yΪprefab��ţ������ͨ�ԣ�����(1,0)(-1,0)(0,1)(0,-1)�ĸ����򣬱��Ϊ0��1��2��3����dir������
//    private void JudgeConnection(int x, int y, int dir)
//    {
//        int dir1=0,dir2=0;
//        if (dir == 0)
//        {
//            dir1 = 3;
//            dir2 = 2;
//        }
//        if (dir == 1)
//        {
//            dir1 = 2;
//            dir2 = 3;
//        }
//        if (dir == 2)
//        {
//            dir1 = 0;
//            dir2 = 5;
//        }
//        if (dir == 3)
//        {
//            dir1 = 5;
//            dir2 = 0;
//        }
     
//               if (Voxel.JudgeConnectivity(prefabVoxels[x].sixDirBinary[dir1], prefabVoxels[y].sixDirBinary[dir2]))
//                {
//                    prefabConnects[dir][x, y] = true;
//                }
            
//    }

//    private void dfs_first(int x, int y)
//    {
//        int n = UnityEngine.Random.Range(0, blockPrefabCount);
//        blocks[x, y] = n;
//        dfs(x + 1, y, blocks[x, y], 0);
//        dfs(x - 1, y, blocks[x, y], 1);
//        dfs(x, y + 1, blocks[x, y], 2);
//        dfs(x, y - 1, blocks[x, y], 3);
//    }


//    //x,y��ʾ��ǰ�������е��ĸ������꣬k��ʾ�ϴ��ƶ�ʱ��prefab��ţ�dirΪ�ƶ�����
//    private void dfs(int x,int y,int k,int dir)
//    {
      
//        //����
//        if (x >= 5 || y >= 5 || x < 0 || y < 0)
//        {
//            return;
//        }
//        //�����Ѿ������
//        if (blocks[x,y]>-2)
//        {
//            return;
//        }
//        //����roll��һ�������prefab
//        List<int>pre2roll = new List<int>();
//        for (int i = 0; i < blockPrefabCount; i++)
//        {
//            if (prefabConnects[dir][k, i])
//            {
//                pre2roll.Add(i);
//            }
//        }
//        blocks[x,y] = getArandomValueFromList(pre2roll);
//        if(blocks[x, y] == -1)
//        {
//            return;
//        }

//        dfs(x+1,y, blocks[x, y],0);
//        dfs(x-1,y, blocks[x, y],1);
//        dfs(x, y+1, blocks[x, y],2);
//        dfs(x, y-1, blocks[x, y], 3);
//    }


//    private int getArandomValueFromList(List<int> ints)
//    {
//        if (ints.Count != 0)
//        {
//            int  n =  UnityEngine.Random.Range(0, ints.Count);
//            return  ints[n];
//        }
//        else 
//        {
//            return -1;
//        }
      
//    }


//    private GameObject GetAppropriateBlock(int index=0,int i=0,int j=0)
//    {
//        GameObject go = PrefabUtility.InstantiatePrefab(prefabsToInstantiate[index]) as GameObject;
//        return go;
//    }
//}