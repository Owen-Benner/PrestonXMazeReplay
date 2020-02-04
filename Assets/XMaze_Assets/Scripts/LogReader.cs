using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogReader : MonoBehaviour
{

    public struct Frame
    {
        float pose;
        float time;
        float x;
        float y;
    }

    public struct Selection
    {
        int reward;
        int score;
    }

    public struct Segment
    {
        float pose;
        float time;
        float x;
        float y;
        String segment;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
