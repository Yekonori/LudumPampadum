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

    bool isPlayable = true;

    int currentNode = 0;

    private List<Vector3> positions;
    public List<Vector3> Positions
    {
        get { return positions; }
        set { positions = value; }
    }

    private bool canRecord = false;
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


    private void Update()
    {
        if(isPlayable == true)
        {
            MoveCharacterWorld(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        if (canRecord == true)
        {
            RecordPosition();
        }
        if(inReplay == true)
        {
            ReplayUpdate();
        }
    }

    private void MoveCharacterWorld(float directionX, float directionZ)
    {
        Vector3 move = new Vector3(directionX, 0, directionZ);
        move.Normalize();
        move *= speed;
        characterController.Move((move * animationSpeed * Time.deltaTime));
    }

    public void SetPosition(Vector3 pos)
    {
        characterController.enabled = false;
        characterController.transform.position = pos;
        characterController.enabled = true;
    }



    // ======================================================================================================================
    private void RecordPosition()
    {
        if(canRecord == true)
        {
            recordTime -= Time.deltaTime;
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

    public void PlayReplay()
    {
        canRecord = false;
        inReplay = true;
    }

    private void ReplayUpdate()
    {
        Vector3 direction = (positions[currentNode] - transform.position).normalized  * Mathf.Sign(animationSpeed);
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
