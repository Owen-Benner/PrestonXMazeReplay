using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMovement : MonoBehaviour
{

    private Transform playPos;

    public List<LogReader.Frame> frames;
    public List<LogReader.Segment> segs;

    // Start is called before the first frame update
    void Start()
    {
        playPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToFrame(float pose, float xPos, float zPos)
    {

    }

}
