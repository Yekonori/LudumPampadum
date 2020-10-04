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

    [Header("3D Models")]
    [SerializeField] private GameObject camilleModel;
    [SerializeField] private GameObject alixModel;
    public bool isAlixModel;

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
    Camera camera;

    private void Start()
    {
        recordInterval /= 60f;
        camera = Camera.main;
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

    /// <summary>
    /// Move Character relative to camera position
    /// </summary>
    public void MoveCharacterWorld(float directionX, float directionZ)
    {
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = right * directionX + forward * directionZ;
        move.Normalize();
        move *= (speed * Mathf.Abs(animationSpeed));
        characterController.Move(move * Time.deltaTime);

        transform.LookAt(this.transform.position + move);

        if (isAlixModel)
        {
            alixModel.GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            camilleModel.GetComponent<Animator>().SetBool("isWalking", true);
        }
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

    public void UpdateModel()
    {
        if (isAlixModel)
        {
            camilleModel.SetActive(false);
            alixModel.SetActive(true);
        }
        else
        {
            camilleModel.SetActive(true);
            alixModel.SetActive(false);
        }
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

        if (animationSpeed == 0)
        {
            if (isAlixModel)
            {
                alixModel.GetComponent<Animator>().SetBool("isWalking", false);
            }
            else
            {
                camilleModel.GetComponent<Animator>().SetBool("isWalking", false);
            }
        }
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

            //transform.LookAt(positions[currentNode]);
            //direction = (positions[currentNode] - transform.position).normalized * Mathf.Sign(animationSpeed);
        }

    }



}
