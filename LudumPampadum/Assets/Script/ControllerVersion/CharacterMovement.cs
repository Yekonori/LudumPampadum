using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;

    [Header("Parameter")]
    [SerializeField]
    float speed = 10;
    [SerializeField]
    float joystick = 10;

    [Header("Record")]
    [SerializeField]
    float recordInterval = 5;
    [SerializeField] float maxDistance = 1.0f;

    /*private bool isPlayable = true;

    public bool IsPlayable
    {
        get { return isPlayable; }
        set { isPlayable = value; }
    }*/

    int currentNode = 0;

    [SerializeField]
    private List<Vector3> positions = new List<Vector3>();
    public List<Vector3> Positions
    {
        get { return positions; }
        set { positions = value; }
    }

    private bool moveAuto = false;
    public bool MoveAuto
    {
        get { return moveAuto; }
        set { moveAuto = value; }
    }

    Vector3 mousePosition;

    private bool canRecord = true;
    public bool CanRecord
    {
        get { return canRecord; }
        set { canRecord = value; }
    }

    private bool inReplay = false;
    public bool InReplay
    {
        get { return inReplay; }
        set { inReplay = value; }
    }

    float animationSpeed = 1f;
    float recordTime = 0f;

    private void Start()
    {
        recordInterval /= 60f;
    }
    private void Update()
    {
        if(moveAuto == true)
        {
            MoveCharacterWorld(mousePosition.x - transform.position.x, mousePosition.z - transform.position.z);
            float distance = Vector3.Distance(transform.position, mousePosition);
            if (distance < maxDistance)
            {
                moveAuto = false;
                GameManagerController.Get.StopTimer();
            }
        }
        if (canRecord == true)
        {
            RecordPosition();
        }
        else if(inReplay == true)
        {
            ReplayUpdate();
        }
    }

    public void MoveCharacterWorld(float directionX, float directionZ)
    {
        Vector3 move = new Vector3(directionX, 0, directionZ);
        move.Normalize();
        move *= (speed * Mathf.Abs(animationSpeed));
        characterController.Move(move * Time.deltaTime);
    }

    public void SetPosition(Vector3 pos)
    {
        characterController.enabled = false;
        characterController.transform.position = pos;
        characterController.enabled = true;
    }



    public void MoveAutoTo(Vector3 pos)
    {
        mousePosition = pos;
        moveAuto = true;
    }












    // ======================================================================================================================
    private void RecordPosition()
    {
        if(canRecord == true)
        {
            recordTime -= Time.deltaTime * animationSpeed;
            if (recordTime <= 0)
            {
                positions.Add(this.transform.position);
                recordTime = recordInterval;
            }
        }
    }


    public void SetAnimationSpeed(float value)
    {
        animationSpeed = value;
    }

    public void RewindReplay()
    {
        currentNode = positions.Count - 1;
        moveAuto = false;
        canRecord = false;
        inReplay = true;
    }

    public void PlayReplay()
    {
        currentNode = 0;
        moveAuto = false;
        canRecord = false;
        inReplay = true;
    }

    private void ReplayUpdate()
    {
        Vector3 direction = (positions[currentNode] - transform.position).normalized;
        MoveCharacterWorld(direction.x, direction.z);
        float distance = Vector3.Distance(transform.position, positions[currentNode]);
        if (distance < maxDistance)
        {
            currentNode += (int)(1 * Mathf.Sign(animationSpeed));
            currentNode = Mathf.Clamp(currentNode, 0, positions.Count - 1);
            /*if (currentNode != positions.Count - 1)
            {
                currentNode += (int) (1 * Mathf.Sign(animationSpeed));
            }*/

            //transform.LookAt(_pathNodes[_currentNode]);
            //direction = (positions[currentNode] - transform.position).normalized * Mathf.Sign(animationSpeed);
        }

    }



}
