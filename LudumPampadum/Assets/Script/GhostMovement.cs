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

    #endregion

    #region Fields

    int _currentNode = 0;
    private bool _canWalk;

    #endregion

    #region Unity Methods

    private void Start()
    {
        //SetListNodes(...);
        direction = (_listPoints[0] - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (_canWalk)
        {
            transform.position += direction * speed * Time.deltaTime;
            UpdatePathNode();
        }
    }

    #endregion

    #region Nodes

    public void SetListNodes(List<Vector3> listP)
    {
        if (_listPoints == null)
        {
            _listPoints = listP;
        }

        //_canWalk = true;

    }

    private void UpdatePathNode()
    {
        float distance = Vector3.Distance(transform.position, _listPoints[_currentNode]);

        if (distance < maxDistance)
        {
            if (_currentNode != _listPoints.Count -1)
            {
                _currentNode++;
            }
            else
            {
                _canWalk = false;
            }

            //transform.LookAt(_pathNodes[_currentNode]);
            direction = (_listPoints[_currentNode] - transform.position).normalized;
        }
    }

    public void Launch()
    {
        Debug.Log("bla");
        _currentNode = 0;
        transform.position = _listPoints[_currentNode];

        _canWalk = true;
    }
    #endregion
}
