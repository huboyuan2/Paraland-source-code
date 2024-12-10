using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //int[] test = { 1, 1 };
        //int t = ++test[0];
        //Debug.LogError(t);
        //Debug.LogError(test[0]);
        //Dictionary<int, int> dic = new Dictionary<int, int>();
        //dic.Add(1, 2);
        //dic[1] = 3;
        //string str = "s";
        //str = str.Insert(0, "pppp");
        //Debug.LogError(str);
        //char a = 'a';
        //Debug.LogError("a=" + (int)a);
        //HashSet<string> testset = new HashSet<string>();
        //testset.Add("asd");
        //testset.Add("asd");
        //testset.Add("qwe");
        //foreach (var item in testset)
        //    Debug.LogError(item);
        //Debug.LogError(testset.Count);
        //IList<string> il;
        ////testset.arrat
        //LinkedListNode<int> ln1 = new LinkedListNode<int>(1);

        //LinkedListNode<int> ln2 = ln1;
        //ln2 = new LinkedListNode<int>(2);
        //Debug.Log("ln1:" + ln1.Value);
        //IList<IList<int>> il2 = new List<IList<int>>();
        //List<int> il1 = new List<int>();
        //il1.Add(3);
        //il2.Add(il1);
        //List<int> il3 = il1;
        //il3.Add(3);
        //il1.Insert(0, 0);
        //Debug.LogError(il1.Count);

        //List<string> list = new List<string>();
        //List<string> copy = list; // 复制一个引用
        //ModifyList(list);
        //list.Add("1");
        //Debug.LogError(copy.Count); // 复制的这个引用仍然指向原来最早的那个List
        //Debug.LogError(list.Count); // list这个引用已经在ModifyList方法里被修改了，指向的是在ModifyList方法里新new出来的对象了
        //int aa = 9;
        //int bb = 5;
        //Debug.LogError(aa&bb);
    }



   

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ModifyList(List<string> list)
    {
        list = new List<string>(); // 因为有ref，所以这里其实已经将方法外原本的那个引用也指向了新的内存地址，所以后续的Add操作是在操作新对象的内存数据，并且方法外原本的那个引用也指向了这个新的对象
        list.Add("1");
        list.Add("2");
        list.Add("3");
    }
}
