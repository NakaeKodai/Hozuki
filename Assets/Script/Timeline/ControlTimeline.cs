using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TimelineControlAnimation
{
    public Animator targetAnimator;
    public RuntimeAnimatorController defaultController;
}

public class ControlTimeline : MonoBehaviour
{
    public PlayableDirector director;
    private bool waitingForInput = false;

    public List<TimelineControlAnimation> timelineControlAnimation = new List<TimelineControlAnimation>();

    public Animator targetAnimator;
    public RuntimeAnimatorController defaultController;

    void Update()
    {
        if(GameManager.instance.playerInputAction.UI.DecisionMenu.triggered && waitingForInput)
        {
            director.Resume();

            for(int i = 0;i < timelineControlAnimation.Count;i++)
            {
                if(timelineControlAnimation[i].targetAnimator != null && timelineControlAnimation[i].defaultController != null)
                {
                    timelineControlAnimation[i].targetAnimator.runtimeAnimatorController = timelineControlAnimation[i].defaultController;
                }
            }
            waitingForInput = false;
        }
    }

    public void puase()
    {
        Debug.Log("ポーズ");

        for(int i = 0;i < timelineControlAnimation.Count;i++)
        {
            if(targetAnimator != null)
            {
                timelineControlAnimation[i].targetAnimator.runtimeAnimatorController = null;
            }
        }
        director.Pause();
        director.Evaluate(); 
        waitingForInput = true;
    }
}
