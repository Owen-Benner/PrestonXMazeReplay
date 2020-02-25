using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eye : MonoBehaviour
{
    public float xPos;
    public float yPos;
    public float diameter;

    public int xRes;
    public int yRes;

    public CanvasScaler scale;

    private Transform pupil;

    // Start is called before the first frame update
    void Start()
    {
        pupil = transform.GetChild(0);
        scale.referenceResolution = new Vector2(xRes, yRes);
    }

    // Update is called once per frame
    void Update()
    {
        float xVal = (xPos * xRes + xRes) / 2;
        float yVal = (yPos * yRes + yRes) / 2;
        transform.position = new Vector3(xVal, yVal, transform.position.z);

        pupil.localScale = new Vector3(diameter, diameter, pupil.localScale.z);
    }
}
