using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    public float xPos;
    public float yPos;
    public float diameter;

    private Transform pupil;

    // Start is called before the first frame update
    void Start()
    {
        pupil = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
