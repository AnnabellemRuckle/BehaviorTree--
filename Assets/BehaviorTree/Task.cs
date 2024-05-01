using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    public abstract void run();
    public bool succeeded;
    protected int eventId;
    const string EVENT_NAME_PREFIX = "FinishedTask";
    public string TaskFinished
    {
        get
        {
            return EVENT_NAME_PREFIX + eventId;
        }
    }
    public Task()
    {
        eventId = EventBus.GetEventID();
    }
}

public class IsTrue : Task
{
    bool varToTest;
    public IsTrue(bool someBool)
    {
        varToTest = someBool;

    }

    public override void run()
    {
        succeeded = varToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}
public class IsFalse : Task
{
    bool varToTest;
    public IsFalse(bool someBool)
    {
        varToTest = someBool;
    }

    public override void run()
    {
        succeeded = !varToTest;
        EventBus.TriggerEvent(TaskFinished);
    }
}
public class OpenDoor : Task
{
    Door mDoor;

    public OpenDoor(Door someDoor)
    {
        mDoor = someDoor;
    }

    public override void run()
    {
        succeeded = mDoor.Open();
        EventBus.TriggerEvent(TaskFinished);
    }
}
public class ShrinkCube : Task
{
    GameObject mPlayer;

    public ShrinkCube(GameObject somePlayer)
    {
        mPlayer = somePlayer;
    }

    public override void run()
    {
        Debug.Log("Shrinking cube");
        mPlayer.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f); //shrinking the cube
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}
public class GiveUpAndGoHome : Task
{
    GameObject mPlayerLeave;
    GameObject mText;

    public GiveUpAndGoHome(GameObject somePlayerLeave, GameObject someText)
    {
        mPlayerLeave = somePlayerLeave;
        mText = someText;
    }

    public override void run()
    {
        Debug.Log("Leaving");
        mPlayerLeave.GetComponent<Rigidbody>().AddForce(0f, 50, 0f, ForceMode.VelocityChange);
        mText.SetActive(true);
        succeeded = false;
        EventBus.TriggerEvent(TaskFinished);
    }
}
public class YayText : Task
{
    GameObject mYayText;

    public YayText(GameObject someParticleEffect)
    {
        mYayText = someParticleEffect;
    }

    public override void run()
    {
        Debug.Log("yayyy");
        mYayText.SetActive(true);
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Wait : Task
{
    float mTimeToWait;

    public Wait(float time)
    {
        mTimeToWait = time;
    }

    public override void run()
    {
        succeeded = true;
        EventBus.ScheduleTrigger(TaskFinished, mTimeToWait);
    }
}

public class MoveKinematicToObject : Task
{
    Arriver mMover;
    GameObject mTarget;

    public MoveKinematicToObject(Kinematic mover, GameObject target)
    {
        mMover = mover as Arriver;
        mTarget = target;
    }

    public override void run()
    {
        mMover.OnArrived += MoverArrived;
        mMover.myTarget = mTarget;
    }

    public void MoverArrived()
    {
        mMover.OnArrived -= MoverArrived;
        succeeded = true;
        EventBus.TriggerEvent(TaskFinished);
    }
}

public class Sequence : Task
{
    List<Task> children;
    Task currentTask;
    int currentTaskIndex = 0;

    public Sequence(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        currentTask = children[currentTaskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }

    void OnChildTaskFinished()
    {
        if (currentTask.succeeded)
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            currentTaskIndex++;
            if (currentTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                succeeded = true;
                EventBus.TriggerEvent(TaskFinished);
            }
        }
        else
        {
            succeeded = false;
            EventBus.TriggerEvent(TaskFinished);
        }
    }
}

public class Selector : Task
{
    List<Task> children;
    Task currentTask;
    int currentTaskIndex = 0;

    public Selector(List<Task> taskList)
    {
        children = taskList;
    }

    public override void run()
    {
        currentTask = children[currentTaskIndex];
        EventBus.StartListening(currentTask.TaskFinished, OnChildTaskFinished);
        currentTask.run();
    }

    void OnChildTaskFinished()
    {
        if (currentTask.succeeded)
        {
            succeeded = true;
            EventBus.TriggerEvent(TaskFinished);
        }
        else
        {
            EventBus.StopListening(currentTask.TaskFinished, OnChildTaskFinished);
            currentTaskIndex++;
            if (currentTaskIndex < children.Count)
            {
                this.run();
            }
            else
            {
                succeeded = false;
                EventBus.TriggerEvent(TaskFinished);
            }
        }
    }
}
