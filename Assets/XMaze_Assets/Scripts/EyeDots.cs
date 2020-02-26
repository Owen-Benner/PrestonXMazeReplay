using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDots : MonoBehaviour
{

    public FileWriter writer;
    public Eye right;
    public Eye left;

    private LogReader logReader;

    private float startTime;

    private List<LogReader.Frame> frames;

    // Start is called before the first frame update
    void Start()
    {
        logReader = gameObject.GetComponent<LogReader>();
        frames = new List<LogReader.Frame>(logReader.frames);

        startTime = writer.startTime;
        Debug.Log("Started at: " + startTime.ToString());
        right.gameObject.SetActive(true);
        left.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Skip to last applicable frame.
        while(frames[1].time < Time.time - startTime)
        {
            frames.RemoveAt(0);
        }
        //Check to read next frame.
        LogReader.Frame frame = frames[0];
        if(Time.time - startTime > frame.time)
        {
            left.xPos = frame.lx;
            left.yPos = frame.ly;
            left.diameter = frame.lp;

            right.xPos = frame.rx;
            right.yPos = frame.ry;
            right.diameter = frame.rp;

            frames.RemoveAt(0);
        }
    }

}
