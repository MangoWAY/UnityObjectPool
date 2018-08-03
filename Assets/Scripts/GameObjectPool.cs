using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct PoolGameObject
    {
        public GameObject Prefab;//需要生成的物体
        public string Name;//物体的名字
        public int PreCount;//预分配的数量
    }
    public PoolGameObject[] PoolSetting;
    public static GameObjectPool instance;
    private Dictionary<string, List<GameObject>> mPool;//一个key值对应一个list，一个list存一类对象
    private Dictionary<string, Transform> mDirectory;//一个key对应一个目录，便于管理

    private void Start()
    {
        InitPool();
        instance = this;
    }

    //初始化对象池
    private void InitPool()
    {
        mPool = new Dictionary<string, List<GameObject>>();
        mDirectory = new Dictionary<string, Transform>();
        string keyTemp = "";
        for (int i = 0; i < PoolSetting.Length; i++)
        {
            //生成物体对应的目录层级
            keyTemp = PoolSetting[i].Name + "(Clone)";
            GameObject directory = new GameObject();
            directory.transform.SetParent(transform);
            directory.name = keyTemp;
            mDirectory.Add(keyTemp, directory.transform);

            //根据设定的大小，生成物体
            mPool.Add(keyTemp, new List<GameObject>());
            for (int j = 0; j < PoolSetting[i].PreCount; j++)
            {
                var go = Instantiate(PoolSetting[i].Prefab, directory.transform);
                go.name = keyTemp;
                go.SetActive(false);
                mPool[keyTemp].Add(go);
            }
        }
    }

    //从对象池里根据名字取物体
    public GameObject GetGameObject(string name)
    {
        GameObject go = null;
        string keyTemp = name + "(Clone)";
        if (!mPool.ContainsKey(keyTemp))
        {
            Debug.LogError("Get: The pool didn't contanin the key");
            go = null;
        }

        else if (mPool[keyTemp].Count != 0)
        {
            go = mPool[keyTemp][0];
            go.SetActive(true);
            mPool[keyTemp].RemoveAt(0);
        }
        else
        {
            for (int i = 0; i < PoolSetting.Length; i++)
            {
                if (PoolSetting[i].Name + "(Clone)" == keyTemp)
                {
                    go = Instantiate(PoolSetting[i].Prefab, transform);
                    break;
                }
            }
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