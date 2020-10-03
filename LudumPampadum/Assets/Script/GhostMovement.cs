using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    #region Script Parameters

    [SerializeField] float speed = 10;
    [SerializeField] Vector3 direction;
    [SerializeField] float maxDistance = 1.0f;

    private List<Vector3> _listPoints;

    public List<Vector3> ListPoints
    {
        get { return _listPoints; }
        set { _listPoints = value; }
    }

    #endregion

    #region Fields

    int _currentNode = 0;

    #endregion

    #region Unity Methods

    void Start()
    {
        //SetListNodes(...);
        direction = (_listPoints[0] - transform.position).normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        UpdatePathNode();
    }

    #endregion

    #region Nodes

    void SetListNodes(List<Vector3> listP)
    {
        if (_listPoints == null)
        {
            _listPoints = listP;
        }
    }

    void UpdatePathNode()
    {
        float distance = Vector3.Distance(transform.position, _listPoints[_currentNode]);
        Debug.Log(distance);

        if (distance < maxDistance)
        {
            if (_currentNode != _listPoints.Count -1)
            {
                _currentNode++;
            }

            //transform.LookAt(_pathNodes[_currentNode]);
            direction = (_listPoints[_currentNode] - transform.position).normalized;
        }
    }

    #endregion
}
