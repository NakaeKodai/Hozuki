using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class HideObject : MonoBehaviour
{
    public bool canHide,isHide;

    Animator animator; //アニメーション変数
    public GameObject hideDirectorObject,hideStartPoint,hideEndPoint;
    public PlayableDirector hideDirector;
    public ControlTimeline controlTimeline;

    [SerializeField] private GameObject operation;
    [SerializeField] private TextMeshProUGUI operationText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canHide && !GameManager.instance.isOtherMenu)
        {
            //操作説明
            if(GameManager.controllerType == GameManager.ControllerType.Unknown)
            {
                operationText.text = "Space : 隠れる";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.PlayStation)
            {
                operationText.text = "● : 隠れる";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Nintendo)
            {
                operationText.text = "A : 隠れる";
            }
            else if(GameManager.controllerType == GameManager.ControllerType.Xbox)
            {
                operationText.text = "B : 隠れる";
            }

            if(!isHide && !GameManager.instance.isIvent)
            {
                if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
                {
                    Hide();
                }
            }
        }
        else if(!canHide && isHide)
        {
            if(GameManager.instance.playerInputAction.Player.ActionANDDecision.triggered)
            {
                GameManager.instance.isHide = false;
                //StartCoroutine(EndHideTimeline(150));
            }
        }
        
    }

    private void Hide()
    {
        GameManager.instance.director = hideDirector;
        controlTimeline.director = hideDirector;
        hideStartPoint.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1,0);
        hideEndPoint.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y, 0);
        Debug.Log("隠れる");
        animator.SetTrigger("Hide");
        // hideDirectorObject.SetActive(true);
        hideDirector.Play();
        GameManager.instance.isHide = true;
    }



    private IEnumerator EndHideTimeline(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null; // 1フレーム待つ
        }

        hideDirectorObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canHide  = true;
            operation.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canHide  = false;
            operation.SetActive(false);
        }
    }
}
