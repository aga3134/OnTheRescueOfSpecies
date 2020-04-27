using UnityEngine;
using System.Collections;

public class FirePattern : MonoBehaviour {
    public float m_Speed = 50.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, 1), Time.deltaTime*m_Speed);
	}
}
