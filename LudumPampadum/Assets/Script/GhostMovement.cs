using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [SerializeField] float speed = 10;
    int currentNode = 0;
    [SerializeField] Vector3 direction;
    [SerializeField] float _maxDistance = 1.0f;

    [SerializeField]
    private List<Vector3> _listPoints;

    public List<Vector3> ListPoints
    {
        get { return _listPoints; }
        set { _listPoints = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetListNodes(...);
        direction = (_listPoints[0] - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        UpdatePathNode();
    }

    void SetListNodes(List<Vector3> listP)
    {
        if (_listPoints == null)
        {
            _listPoints = listP;
        }
    }

    void UpdatePathNode()
    {
        float distance = Vector3.Distance(transform.position, _listPoints[currentNode]);
        Debug.Log(distance);
        if (distance < _maxDistance)
        {
            if (currentNode != _listPoints.Count -1)
            {
                currentNode++;
            }
            //transform.LookAt(_pathNodes[_currentNode]);
            direction = (_listPoints[currentNode] - transform.position).normalized;
        }
    }
}
