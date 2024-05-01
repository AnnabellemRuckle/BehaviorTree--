using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruce : MonoBehaviour
{
    public Door theDoor;
    public GameObject theTreasure;
    public GameObject yayText;
    public GameObject theGiveUpText;
    public int randNum;
    bool executingBehavior = false;
    Task myCurrentTask;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            randNum = Random.Range(0, 2);
            Debug.Log(randNum);
            if (!executingBehavior)
            {
                Debug.Log("Starting behavior");
                executingBehavior = true;
                myCurrentTask = BuildTask_GetTreasure();

                myCurrentTask.run();
                Debug.Log("Finished");
            }
        }
    }

    void OnTaskFinished()
    {
        executingBehavior = false;
    }

    Task BuildTask_GetTreasure()
    {
        List<Task> taskList = new List<Task>();

        Task isDoorNotLocked = new IsFalse(theDoor.isLocked);
        Task waitABeat = new Wait(0.5f);
        Task openDoor = new OpenDoor(theDoor);
        taskList.Add(isDoorNotLocked);
        taskList.Add(waitABeat);
        taskList.Add(openDoor);
        Debug.Log("Door Opened");
        Sequence openUnlockedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(theDoor.isClosed);
        Task giveUp = new GiveUpAndGoHome(this.gameObject, theGiveUpText);
       // Task bargeDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(isDoorClosed);
        if (randNum == 0)
        {
            taskList.Add(waitABeat);
            taskList.Add(giveUp);
            taskList.Add(waitABeat);
            taskList.Add(waitABeat);
        }

        if (randNum == 1)
        {
            taskList.Add(waitABeat);
          //  taskList.Add(bargeDoor);
            taskList.Add(waitABeat);
            taskList.Add(new ShrinkCube(this.gameObject));
        }
        Sequence bargeClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        taskList = new List<Task>();
        Task moveToDoor = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theDoor.gameObject);
        Task moveToTreasure = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theTreasure.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(waitABeat);
        taskList.Add(openTheDoor);
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        taskList.Add(new YayText(yayText));
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(theDoor.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        taskList.Add(new YayText(yayText));
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        return getTreasure;
    }
}
