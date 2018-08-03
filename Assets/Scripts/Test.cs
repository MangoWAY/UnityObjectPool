
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public GameObjectPool Pool;
    public float intervalTime = 1f;
    private float time = 0;
	// Use this for initialization
	void Start () {
        InvokeRepeating("fire1", 0, intervalTime);
        InvokeRepeating("fire2", 0, intervalTime);
        InvokeRepeating("fire3", 0, intervalTime);
    }
	
	// Update is called once per frame
	void Update () {
	}
    void fire1()
    {
        var go = Pool.GetGameObject("Cube");
        go.GetComponent<Rigidbody>().AddForce(Vector3.up * 150);
    }
    void fire2()
    {
        var go = Pool.GetGameObject("Sphere");
        go.GetComponent<Rigidbody>().AddForce(Vector3.right * 150);
    }
    void fire3()
    {
        var go = Pool.GetGameObject("Capsule");
        go.GetComponent<Rigidbody>().AddForce(Vector3.forward * 150);
    }
}
