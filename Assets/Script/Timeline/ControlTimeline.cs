using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ControlTimeline : MonoBehaviour
{
    public PlayableDirector director;
    private bool waitingForInput = false;

    void Update()
    {
        if(GameManager.instance.playerInputAction.UI.DecisionMenu.triggered && waitingForInput)
        {
            director.Resume();
            waitingForInput = false;
        }
    }

    public void puase()
    {
        director.Pause();
        waitingForInput = true;
    }
}
