using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    public Animator animator;
    public NavMeshAgent agent;
    public float inputHoldDelay = 0.5f;
    public float turnSpeedThreshold = 0.5f;
    public float speedDampTime = 0.1f;
    public float slowingSpeed = 0.175f;
    public float turnSmoothing = 15f;

    private WaitForSeconds inputHoldWait;
    private Vector3 destinationPosition;
    private Interactable currentInteractable;
    private bool handleInput = true;

    private const float stopDistanceProportion = 0.1f;//把需要停止的距離跟目標分為10等分
    private const float navMeshSampleDistance = 4f;


    private readonly int hashSpeedPara = Animator.StringToHash("Speed");
    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");

    // Use this for initialization
    void Start() {
        //agent.updateRotation = false;

        inputHoldWait = new WaitForSeconds(inputHoldDelay);

        destinationPosition = transform.position;

        
    }

    

    private void OnAnimatorMove()//在動畫State與motion被賦值時每偵調用 (update之後)，並且動畫物件的變換可以套用到動畫中成為動畫的一部份(Root motion)
    {
       agent.velocity = animator.deltaPosition / Time.deltaTime;//速度等於 兩偵之間移動的距離 / 兩偵之間的的時間
    }

    // Update is called once per frame
    void Update () {
        if (agent.pathPending)
        {
            return;
        }
        float speed = agent.desiredVelocity.magnitude;//取得現在當前受到的向量長度
        
        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)//自己與目的地座標的距離 <= 距離目標多少需要停止的距離乘上一個比例(最接近終點)
        {
            Stopping(out speed);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)//自己與目的地座標的距離 <= 距離目標多少需要停止的距離(進入需要停止的範圍內)
        {
            Slowing(out speed, agent.remainingDistance);
        }
        else if(speed > turnSpeedThreshold)//還未靠近停止距離範圍內，並且速度大於0.5就繼續移動
        {
            Moving();

        }

        animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
	}

    private void Stopping(out float speed)
    {
        agent.isStopped = true;
        transform.position = destinationPosition;
        speed = 0;

        if (currentInteractable)
        {
            transform.rotation = currentInteractable.interactionLocation.rotation;
            currentInteractable.Interact();
            currentInteractable = null;
            StartCoroutine("WaitForInteraction");

        }
    }

    private void Slowing(out float speed, float distanceToDestination)
    {
        agent.isStopped = true;
        transform.position = Vector3.MoveTowards(transform.position, destinationPosition, slowingSpeed * Time.deltaTime);
        float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;
        speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);

        Quaternion targetRotation =
            currentInteractable ? currentInteractable.interactionLocation.rotation : transform.rotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);
    }

    private void Moving()
    {
        Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }

    public void OnGroundClick(BaseEventData data)
    {
        if (!handleInput)
        {
            return;
        }

        currentInteractable = null;

        PointerEventData pData = (PointerEventData)data;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            destinationPosition = hit.position;
            
        }
        else
        {
            destinationPosition = pData.pointerCurrentRaycast.worldPosition;
            
        }

        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }

    public void OnInteractableClick(Interactable interactable)
    {
        if (!handleInput)
        {
            return;
        }

        currentInteractable = interactable;
        destinationPosition = currentInteractable.interactionLocation.position;

        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }

    private IEnumerator WaitForInteraction()
    {
        handleInput = false;
        yield return inputHoldWait;
        while (animator.GetCurrentAnimatorStateInfo(0).tagHash != hashLocomotionTag)
        {
            yield return null;
        }

        handleInput = true;
    }

}
