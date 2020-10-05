using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CharacterPosition
{
    public Vector3 Positions;
    public float Time;

    public CharacterPosition(Vector3 pos, float t)
    {
        Positions = pos;
        Time = t;
    }
}



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
    private List<CharacterPosition> positions = new List<CharacterPosition>();
    public List<CharacterPosition> Positions
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
    Camera camera;

    float recordTime = 0f;
    float totalRecordTime = 0f;
    float replayTime = 0f;



    private void Start()
    {
        //recordInterval /= 60f;
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
    public void MoveCharacterCamera(float directionX, float directionZ)
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

    public void MoveCharacterWorld(float directionX, float directionZ)
    {
        Vector3 move = new Vector3(directionX, 0, directionZ);
        move.Normalize();
        move *= (speed * Mathf.Abs(animationSpeed));
        characterController.Move(move * Time.deltaTime);

        transform.LookAt(this.transform.position + move * Mathf.Sign(animationSpeed));
        
        if (isAlixModel)
        {
            alixModel.GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            camilleModel.GetComponent<Animator>().SetBool("isWalking", true);
        }
    }


    /// <summary>
    /// Move Character without Time.deltatime
    /// </summary>
    /// <param name="directionX"></param>
    /// <param name="directionZ"></param>
    public void MoveCharacterManual(float speedX, float speedY, float speedZ)
    {
        Vector3 move = new Vector3(speedX, speedY, speedZ);
        characterController.Move(move);
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


    public void ClearPosition()
    {
        positions.Clear();
    }







    // ======================================================================================================================
    private void RecordPosition()
    {
        if(canRecord == true)
        {
            recordTime -= Time.deltaTime * animationSpeed;
            totalRecordTime += Time.deltaTime * animationSpeed;
            if (recordTime <= 0)
            {
                positions.Add(new CharacterPosition(this.transform.position, totalRecordTime));
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
        replayTime = totalRecordTime;
        moveAuto = false;
        canRecord = false;
        inReplay = true;
    }

    public void PlayReplay()
    {
        currentNode = 0;
        replayTime = 0;
        moveAuto = false;
        canRecord = false;
        inReplay = true;
    }

    public void ResetReplay()
    {
        currentNode = 0;
        replayTime = 0;
        moveAuto = false;
        canRecord = false;
        inReplay = true;
    }
    /* private void ReplayUpdate()
     {
         Vector3 direction = (positions[currentNode] - transform.position).normalized;
         MoveCharacterWorld(direction.x, direction.z);
         float distance = Vector3.Distance(transform.position, positions[currentNode]);
         if (distance < maxDistance)
         {
             currentNode += (int)(1 * Mathf.Sign(animationSpeed));
             currentNode = Mathf.Clamp(currentNode, 0, positions.Count - 1);

         }

     }*/

    private void ReplayUpdate()
    {
        replayTime += Time.deltaTime * animationSpeed;
        Vector3 destination;
        if (animationSpeed < 0)
            destination = CalculateRewindDestination();
        else
        {
            destination = CalculateReplayDestination();

            // Mettre un raycast si on veut que ça se passe bien
            /*RaycastHit hit;
            int layerMask = 1 << 0;
            //Debug.DrawRay(this.transform.position + new Vector3(0, 0.2f, 0), destination - this.transform.position, Color.yellow, destination.magnitude);
            if (Physics.Raycast(this.transform.position + new Vector3(0, 0.2f, 0), destination - this.transform.position + new Vector3(0, 0.2f, 0), out hit, destination.magnitude, layerMask))
            {
                //Debug.Log(hit.collider.gameObject.name);
                replayTime -= Time.deltaTime * animationSpeed;
                return;
            }*/
        }
        MoveCharacterManual(destination.x - transform.position.x, 0, destination.z - transform.position.z);
    }

    // C'est giga sale
    private Vector3 CalculateReplayDestination()
    {
        if (currentNode == positions.Count - 1)
            return positions[positions.Count - 1].Positions;
        float t = (replayTime - positions[currentNode].Time) / (positions[currentNode+1].Time - positions[currentNode].Time);
        while(t > 1) 
        {
            currentNode += 1;
            if (currentNode == positions.Count - 1)
                return positions[positions.Count - 1].Positions;
            t = (replayTime - positions[currentNode].Time) / (positions[currentNode + 1].Time - positions[currentNode].Time);
        }
        return Vector3.Lerp(positions[currentNode].Positions, positions[currentNode+1].Positions, t);
    }

    // C'est giga sale
    private Vector3 CalculateRewindDestination()
    {
        if (currentNode == 0)
            return positions[0].Positions;
        float t = (positions[currentNode].Time - replayTime) / (positions[currentNode].Time - positions[currentNode - 1].Time);
        while (t > 1)
        {
            currentNode -= 1;
            if (currentNode == 0)
                return positions[0].Positions;
            t = (positions[currentNode].Time - replayTime) / (positions[currentNode].Time - positions[currentNode - 1].Time);
        }
        return Vector3.Lerp(positions[currentNode].Positions, positions[currentNode - 1].Positions, t);
    }

    public void SoundRewind(bool active)
    {
        if (active)
            AudioManager.instance.Play("SFX_Rewind_Play", false);
        else
            AudioManager.instance.Play("SFX_Rewind_Pause", false);
    }

}
