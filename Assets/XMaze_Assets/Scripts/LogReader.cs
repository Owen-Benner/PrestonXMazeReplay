using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogReader : MonoBehaviour
{

    public struct Frame
    {
        float pose;
        float time;
        float x;
        float y;

        public Frame(float p, float t, float _x, float _y)
        {
            pose = p;
            time = t;
            x = _x;
            y = _y;
        }
    };

    public struct Selection
    {
        int reward;
        int score;

        public Selection(int r, int s)
        {
            reward = r;
            score = s;
        }
    };

    public struct Segment
    {
        float pose;
        float time;
        float x;
        float y;
        string segment;

        public Segment(float p, float t, float _x, float _y, string s)
        {
            pose = p;
            time = t;
            x = _x;
            y = _y;
            segment = s;
        }
    };

    private string fileName;
    public string partCode;
    public int runNum;
    public int mode;

    public List<LogReader.Frame> frames;
    public List<LogReader.Selection> selects;
    public List<LogReader.Segment> segs;

    public void ReadLog(string filename, List<Frame> frames,
        List<Selection> selections, List<Segment> segments)
    {
        try
        {
            StreamReader reader = new StreamReader(filename);
            
            Debug.Log("Replaying: " + reader.ReadLine());
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();

            while(!reader.EndOfStream)
            {    
                string [] line = reader.ReadLine().Split(' ');
                if(line[0] == "Frame")
                {
                    Frame frame = new Frame(Single.Parse(line[4]),
                        Single.Parse(line[5]), Single.Parse(line[6]),
                        Single.Parse(line[7]));
                    frames.Add(frame);
                }
                else if(line[0] == "Selection")
                {
                    Selection select = new Selection(Int32.Parse(line[3]),
                        Int32.Parse(line[4]));
                    selections.Add(select);
                }
                else if(line[0] == "Segment:")
                {
                    Segment seg = new Segment(Single.Parse(line[3]),
                        Single.Parse(line[4]), Single.Parse(line[5]),
                        Single.Parse(line[6]), line[9]);
                    segments.Add(seg);
                }
            }
            reader.Close();
        }
        catch(Exception e)
        {
            Debug.LogError("Error parsing file (1)!!");
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

        ReadLog(fileName, frames, selects, segs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
