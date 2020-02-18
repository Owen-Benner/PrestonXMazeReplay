using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogReader : MonoBehaviour
{

    public struct Frame
    {
        public float pose;
        public float time;
        public float x;
        public float z;

        public Frame(float p, float t, float _x, float _z)
        {
            pose = p;
            time = t;
            x = _x;
            z = _z;
        }
    };

    public struct Selection
    {
        public int reward;
        public int score;

        public Selection(int r, int s)
        {
            reward = r;
            score = s;
        }
    };

    public struct Segment
    {
        public float pose;
        public float time;
        public float x;
        public float y;
        public string segment;
        public int num;

        public Segment(float p, float t, float _x, float _y, string s, int n)
        {
            pose = p;
            time = t;
            x = _x;
            y = _y;
            segment = s;
            num = n;
        }
    };

    private string fileName;
    public string partCode;
    public int runNum;
    public int mode;

    public List<LogReader.Frame> frames;
    public List<LogReader.Selection> selects;
    public List<LogReader.Segment> segs;

    public void ReadLog(string filename, List<Frame> _frames,
        List<Selection> _selects, List<Segment> _segs)
    {
        try
        {
            StreamReader reader = new StreamReader(filename);
            
            Debug.Log("Replaying: " + reader.ReadLine());
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();

            int breakCount = -1;
            while(!reader.EndOfStream && breakCount != 0)
            {    
                string [] line = reader.ReadLine().Split(' ');
                if(line[0] == "Frame")
                {
                    Frame frame = new Frame(Single.Parse(line[4]),
                        Single.Parse(line[5]), Single.Parse(line[6]),
                        Single.Parse(line[7]));
                    _frames.Add(frame);
                }
                else if(line[0] == "Selection")
                {
                    Selection select = new Selection(Int32.Parse(line[3]),
                        Int32.Parse(line[4]));
                    _selects.Add(select);
                }
                else if(line[0] == "Segment:")
                {
                    Segment seg = new Segment(Single.Parse(line[3]),
                        Single.Parse(line[4]), Single.Parse(line[5]),
                        Single.Parse(line[6]), line[9], Int32.Parse(line[8]));
                    _segs.Add(seg);
                    if(line[9] == "EndRun")
                    {
                        breakCount = 5;
                    }
                }
                if(breakCount > 0)
                {
                    --breakCount;
                }
            }
            reader.Close();
        }
        catch(Exception e)
        {
            Debug.LogError("Error parsing log!!");
            Debug.LogError(e);
            Application.Quit();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fileName = partCode + "_";
        if (mode == 1)
        {
            fileName += "practice";
        }
        else
        {
            fileName += "task";
        }
        fileName += "_run" + runNum + ".xml";

        frames = new List<Frame>();
        selects = new List<Selection>();
        segs = new List<Segment>();

        ReadLog(fileName, frames, selects, segs);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
