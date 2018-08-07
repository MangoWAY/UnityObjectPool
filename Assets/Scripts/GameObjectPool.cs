using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct PoolGameObject
    {
        public GameObject Prefab;
        public string Name;
        public int PreAllocSize;
        public int AutoIncreaseSize;
    }
    public PoolGameObject[] PoolSetting;
    public static GameObjectPool instance;
    private Dictionary<string, List<GameObject>> mPool;
    private Dictionary<string, Transform> mDirectory;

    private void Start()
    {
        RuntimeInitPool();
        instance = this;
    }

    //初始化对象池
    public void RuntimeInitPool()
    {
        mPool = new Dictionary<string, List<GameObject>>();
        mDirectory = new Dictionary<string, Transform>();
        for(int i=0;i<transform.childCount;i++)
        {
            mDirectory.Add(transform.GetChild(i).name, transform.GetChild(i));
            mPool.Add(transform.GetChild(i).name, new List<GameObject>());
            for(int j=0;j<transform.GetChild(i).childCount; j++)
            {
                mPool[transform.GetChild(i).GetChild(j).name].Add(transform.GetChild(i).GetChild(j).gameObject);
            }
        }
      
    }
    public void EditorInitPool()
    {
        for(int i= transform.childCount-1; i>=0;i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);    
        }
        string keyTemp = "";
        for (int i = 0; i < PoolSetting.Length; i++)
        {
            //生成物体对应的目录层级
            keyTemp = PoolSetting[i].Name + "(Clone)";
            if (transform.Find(keyTemp))
            {
                break;
            }
            GameObject directory = new GameObject();
            directory.transform.SetParent(transform);
            directory.name = keyTemp;


            //根据设定的大小，生成物体
            for (int j = 0; j < PoolSetting[i].PreAllocSize; j++)
            {
                var go = Instantiate(PoolSetting[i].Prefab, directory.transform);
                go.name = keyTemp;
                go.SetActive(false);
            }
        }
    }

    private void AutoIncrease(string key)
    {
        if (mPool[key].Count != 0)
            return;
        for (int i = 0; i < PoolSetting.Length; i++)
        {
            if (PoolSetting[i].Name + "(Clone)" == key)
            {
                for (int j = 0; j < PoolSetting[i].AutoIncreaseSize; j++)
                {
                    var go = Instantiate(PoolSetting[i].Prefab, mDirectory[PoolSetting[i].Name + "(Clone)"]);
                    mPool[go.name].Add(go);
                }
            }
        }
    }


    //从对象池里根据名字取物体
    public GameObject GetGameObject(string name)
    {
        GameObject go = null;
        string key = name + "(Clone)";
        if (!mPool.ContainsKey(key))
        {
            Debug.LogError("Get: The pool didn't contanin the key");
            go = null;
        }
        else
        {
            AutoIncrease(key);
            go = mPool[key][0];
            go.SetActive(true);
            mPool[key].RemoveAt(0);
        }
        return go;
    }

    //使用完成后的游戏对象返回对象池
    public void ReturnGameObject(GameObject go)
    {
        if (!mPool.ContainsKey(go.name))
        {
            Debug.LogError("Return: The pool didn't contanin the key");
            return;
        }
        go.transform.SetParent(mDirectory[go.name]);
        mPool[go.name].Add(go);
        go.SetActive(false);
    }
}