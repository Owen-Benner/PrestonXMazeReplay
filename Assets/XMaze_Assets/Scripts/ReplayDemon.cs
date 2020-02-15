using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayDemon : MonoBehaviour
{

    public LogReader logReader;

    public FrameMovement frameMove;

    public FileWriter writer;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        logReader = GameObject.Find("LogReader").GetComponent<LogReader>();
        frameMove = GetComponent<FrameMovement>();
        frameMove.enabled = true;
        writer = GameObject.Find("FileWriter").GetComponent<FileWriter>();
        startTime = writer.startTime;
    }

    // Update is called once per frame
    void Update()
    {
        LogReader.Frame frame = logReader.frames[0];
        if(Time.time - startTime > frame.time)
        {
            frameMove.MoveToFrame(frame.pose, frame.x, frame.z);
            logReader.frames.RemoveAt(0);
        }
    }

}
