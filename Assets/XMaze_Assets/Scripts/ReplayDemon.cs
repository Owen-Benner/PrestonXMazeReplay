using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayDemon : MonoBehaviour
{

    public LogReader logReader;

    public FrameMovement frameMove;

    public FileWriter writer;

    private float startTime;

    private bool replaying = false;

    public Text rewardText;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        logReader = GameObject.Find("LogReader").GetComponent<LogReader>();
        frameMove = GetComponent<FrameMovement>();
        frameMove.enabled = true;
        writer = GameObject.Find("FileWriter").GetComponent<FileWriter>();
        startTime = writer.startTime;
        rewardText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        LogReader.Segment seg = logReader.segs[0];
        if(Time.time - startTime > seg.time)
        {
            if(seg.segment == "Hallway" && seg.num == 0)
            {
                replaying = true;
            }
            if(seg.segment == "EndRun")
            {
                Debug.Log("End of replay.");
                Application.Quit();
            }
            logReader.segs.RemoveAt(0);
        }

        //Skip to last applicable segment.
        while(logReader.frames[1].time < Time.time - startTime)
        {
            logReader.frames.RemoveAt(0);
        }
        LogReader.Frame frame = logReader.frames[0];
        if(Time.time - startTime > frame.time)
        {
            if(replaying)
            {
                frameMove.MoveToFrame(frame.pose, frame.x, frame.z);
            }
            logReader.frames.RemoveAt(0);
        }
    }

}
