using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demon : MonoBehaviour
{

    public string doneMsg = "Final Score: ";
    public string preRew = "+";

    public int mode;

    public float visibleTime;
    public float selectTime;
    public float rewardTime;
    public float returnTime;

    public int direction;
    public int trialNum = 0;

    public int[] contexts;
    public float[] holds;
    public float[] postHits;

    public int[] leftObjects;
    public int[] leftRewards;
    public int[] rightObjects;
    public int[] rightRewards;

    public float westXPos = 184.2314f;
    public float eastXPos = 304.2314f;
    public float yPos = 3f;
    public float zPos = 255f;

    public float east = 90f;
    public float west = 270f;

    public float holdTurnRate = 90f;
    public float returnRate = 180f;
    public float returnSpeedX = 20f;
    public float returnSpeedZ = 20f;

    public Text rewardText;
    public Text scoreText;

    SimpleMovement move;

    public float endTimer = 5f;

    public enum segments
    {
        Hallway,
        HoldA,
        Selection,
        PostHit,
        HoldB,
        Reward,
        Return,
        EndRun
    };

    public segments segment;

    public GameObject contextN;
    public GameObject contextS;

    public GameObject objectNE;
    public GameObject objectSE;
    public GameObject objectSW;
    public GameObject objectNW;

    public GameObject goalNE;
    public GameObject goalSE;
    public GameObject goalSW;
    public GameObject goalNW;

    public FileWriter writer;
    public FileReader reader;

    public string [] contextList =
    {
        "Gray",
        "Wood",
        "Brick",
        "Stone",
        "Metal"
    };

    private float choiceStart;
    private float selectStart;
    private bool vis;

    private int score;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            reader = GameObject.Find("FileReader").GetComponent<FileReader>();
        }
        catch
        {
            Debug.Log("Start with Gray_Screen.");
        }
        reader.XMazeInit();

        if(mode == 3) //Switch demons.
        {
            ReplayDemon replay = GetComponent<ReplayDemon>();
            replay.enabled = true;
            this.enabled = false;
            replay.rewardText = rewardText;
            replay.scoreText = scoreText;
        }
        else
        {
            writer = GameObject.Find("FileWriter").GetComponent<FileWriter>();
            writer.XMazeInit();
            move = GetComponent<SimpleMovement>();
            move.enabled = true; 
            trialNum = 0;
            StartHallway();
        }

        scoreText.text = 0.ToString();

        returnRate = 180f / returnTime;
        returnSpeedX = 20f / returnTime;
        returnSpeedZ = 20f / returnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(segment == segments.Hallway)
        {
            if(direction == 1 && transform.position.x >= eastXPos)
            {
                StartHoldA();
            }
            else if(direction == 2 && transform.position.x <= westXPos)
            {
                StartHoldA();
            }
        }

        else if(segment == segments.HoldA)
        {
            Rotate(holdTurnRate);

            if(!move.IsHolding())
            {
                StartSelection();
            }
        }

        else if(segment == segments.Selection)
        {
            if(Time.time - selectStart >= selectTime)
            {
                GiveReward(null);
            }
        }

        else if(segment == segments.PostHit)
        {
            if(!move.IsHolding())
            {
                StartHoldB();
            }
        }

        else if(segment == segments.HoldB)
        {
            if(!move.IsHolding())
            {
                StartReward();
            }
        }

        else if(segment == segments.Reward)
        {
            if(!move.IsHolding())
            {
                StartReturn();
            }
        }

        else if(segment == segments.Return)
        {
            if(direction == 2)
            {
                if(transform.position.x > eastXPos)
                {
                    move.move.x -= returnSpeedX * Time.deltaTime;
                    if(transform.position.x + move.move.x < eastXPos)
                    {
                        move.move.x = eastXPos - transform.position.x;
                    }
                }
            }
            else if(direction == 1)
            {
                if(transform.position.x < westXPos)
                {
                    move.move.x += returnSpeedX * Time.deltaTime;
                    if(transform.position.x + move.move.x > eastXPos)
                    {
                        move.move.x = eastXPos - transform.position.x;
                    }
                }
            }
            else{Debug.Log("Direction machine broke.");}

            if(transform.position.z > zPos)
            {
                move.move.z -= returnSpeedZ * Time.deltaTime;
                if(transform.position.z + move.move.z < zPos)
                {
                    move.move.z = zPos - transform.position.z;
                }
            }
            else if(transform.position.z < zPos)
            {
                move.move.z += returnSpeedZ * Time.deltaTime;
                if(transform.position.z + move.move.z > zPos)
                {
                    move.move.z = zPos - transform.position.z;
                }
            }

            Rotate(returnRate);

            if(!move.IsHolding())
            {
                ++trialNum;
                StartHallway();
            }
       }

        else if(segment == segments.EndRun)
        {
            if(endTimer <= 0)
            {
                Debug.Log("Quitting!");
                Application.Quit();
            }
            endTimer -= Time.deltaTime;
            //Debug.Log("Quitting in: " + endTimer.ToString());
        }
    }

    void GiveReward(GameObject obj)
    {
        if(segment == segments.Selection)
        {
            int reward = 0;
            int select = 0;
            if(mode == 2)
            {
                if(obj == objectNE)
                {
                    reward = leftRewards[trialNum];
                    select = 1;
                }
                else if(obj == objectSE)
                {
                    reward = rightRewards[trialNum];
                    select = 2;
                }
                else if(obj == objectSW)
                {
                    reward = leftRewards[trialNum];
                    select = 1;
                }
                else if(obj == objectNW)
                {
                    reward = rightRewards[trialNum];
                    select = 2;
                }
            }
            else
            {
                if(obj == objectNE)
                {
                    reward = leftObjects[trialNum];
                    select = 1;
                }
                else if(obj == objectSE)
                {
                    reward = rightObjects[trialNum];
                    select = 2;
                }
                else if(obj == objectSW)
                {
                    reward = leftObjects[trialNum];
                    select = 1;
                }
                else if(obj == objectNW)
                {
                    reward = rightObjects[trialNum];
                    select = 2;
                }
            }

            rewardText.text = preRew + reward.ToString();
            score += reward;
            writer.WriteSelect(select, reward, score);
            if(obj == null)
                StartReward();
            else
                StartPostHit();
        }
    }

    void ClearVisibility()
    {
        int zero = 0;

        objectNE.SendMessage("Sprite", zero);
        objectSE.SendMessage("Sprite", zero);
        objectSW.SendMessage("Sprite", zero);
        objectNW.SendMessage("Sprite", zero);

        goalNE.SendMessage("Render", zero);
        goalSE.SendMessage("Render", zero);
        goalSW.SendMessage("Render", zero);
        goalNW.SendMessage("Render", zero);

        vis = false;
    }

    void Rotate(float rate)
    {
        if(direction == 1)
        {
            if(transform.eulerAngles.y < east || transform.eulerAngles.y > west)
            {
                move.rotate.y += Time.deltaTime * rate;
                if(transform.eulerAngles.y + move.rotate.y > east &&
                        transform.eulerAngles.y + move.rotate.y < west)
                {
                    move.rotate.y = east - transform.eulerAngles.y;
                }
            }
            else if(transform.eulerAngles.y > east)
            {
                move.rotate.y -= Time.deltaTime * rate;
                if(transform.eulerAngles.y + move.rotate.y < east)
                {
                    move.rotate.y = east - transform.eulerAngles.y;
                }
            }
        }
        else if(direction == 2)
        {
            if(transform.eulerAngles.y > west || transform.eulerAngles.y < east)
            {
                move.rotate.y -= Time.deltaTime * rate;
                if(transform.eulerAngles.y + move.rotate.y < west &&
                        transform.eulerAngles.y + move.rotate.y > east)
                {
                    move.rotate.y = west - transform.eulerAngles.y;
                }
            }
            else if(transform.eulerAngles.y < west)
            {
                move.rotate.y += Time.deltaTime * rate;
                if(transform.eulerAngles.y + move.rotate.y > west)
                {
                    move.rotate.y = west - transform.eulerAngles.y;
                }
            }
        }
        else{Debug.Log("Direction machine broke.");}
    }

    void SetContexts()
    {
        if(mode != 2)
        {
            contexts[trialNum] = 0;
            return;
        }
            contextN.SendMessage(contextList[contexts[trialNum]]);
            contextS.SendMessage(contextList[contexts[trialNum]]);
    }

    void ClearContexts()
    {
        contextN.SendMessage(contextList[0]);
        contextS.SendMessage(contextList[0]);
    }

    void StartHallway()
    {
        if(trialNum == contexts.Length)
        {
            StartEndRun();
            return;
        }
        segment = segments.Hallway;
        writer.WriteSegment();
        move.setForward(true);
        move.EndHold();
        rewardText.enabled = false;
        if(mode == 2)
        {
            SetContexts();
        }

        if(direction == 1)
            transform.position = new Vector3(westXPos, transform.position.y,
                    zPos);
        else if(direction == 2)
            transform.position = new Vector3(eastXPos, transform.position.y,
                    zPos);
        else
            Debug.Log("Direction machine broke.");
}

    void StartHoldA()
    {
        segment = segments.HoldA;
        move.setForward(false);
        choiceStart = Time.time;
        writer.WriteSegment();

        {   // Catch up on time
            float holdTime = holds[trialNum];
            float dif = writer.getRunTime() % 1; 
            if(dif > 0.5)
                holdTime += (1 - dif);
            else
                holdTime -= dif;
            move.BeginHold(holdTime);
        }

        if(direction == 1)
            transform.position = new Vector3(eastXPos, transform.position.y,
                    zPos);
        else if(direction == 2)
            transform.position = new Vector3(westXPos, transform.position.y,
                    zPos);
        else
            Debug.Log("Direction machine broke.");

        ClearContexts();
        if(direction == 1)
        {
            if(mode == 2)
            {
                objectNE.SendMessage("Sprite", leftObjects[trialNum]);
                objectSE.SendMessage("Sprite", rightObjects[trialNum]);
            }
            else
            {
                goalNE.SendMessage("Render", leftObjects[trialNum]);
                goalSE.SendMessage("Render", rightObjects[trialNum]);
            }
        }
        else if(direction == 2)
        {
            if(mode == 2)
            {
                objectSW.SendMessage("Sprite", leftObjects[trialNum]);
                objectNW.SendMessage("Sprite", rightObjects[trialNum]);
            }
            else
            {
                goalSW.SendMessage("Render", leftObjects[trialNum]);
                goalNW.SendMessage("Render", rightObjects[trialNum]);
            }
        }
        else
        {
            Debug.Log("Direction machine broke.");
        }

        Debug.Log(leftObjects[trialNum] + ", " + rightObjects[trialNum]);
        vis = true;
    }

    void StartSelection()
    {
        segment = segments.Selection;
        selectStart = Time.time;
        writer.WriteSegment();
    }

    void StartPostHit()
    {
        segment = segments.PostHit;
        writer.WriteSegment();
        move.BeginHold(selectTime - (Time.time - selectStart));
    }

    void StartHoldB()
    {
        segment = segments.HoldB;
        writer.WriteSegment();
        move.BeginHold(postHits[trialNum]);
    }

    void StartReward()
    {
        segment = segments.Reward;
        writer.WriteSegment();
        rewardText.enabled = true;
        scoreText.text = score.ToString();
        move.BeginHold(rewardTime);
        ClearVisibility();

        if(direction == 1)
        {
            direction = 2;
        }
        else if(direction == 2)
        {
            direction = 1;
        }
        else
        {
            Debug.Log("Direction machine broke.");
        }
    }

    void StartReturn()
    {
        segment = segments.Return;
        writer.WriteSegment();
        move.BeginHold(returnTime);
    }

    void StartEndRun()
    {
        rewardText.text = doneMsg + score.ToString();
        rewardText.enabled = true;
        scoreText.enabled = false;
        segment = segments.EndRun;
        move.BeginHold(endTimer);
        writer.WriteSegment();
    }
}
