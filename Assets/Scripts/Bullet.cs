using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    private void OnEnable()
    {
        Invoke("DestroySelf", 3f);
    }

    // Update is called once per frame
    void Update () {
		
	}
    void DestroySelf()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.position = Vector3.zero;
        GameObjectPool.instance.ReturnGameObject(gameObject);
    }
}
