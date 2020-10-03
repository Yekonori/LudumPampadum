using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Script Parameters

    [SerializeField] private float speed = 5f;
    [SerializeField] private float _maxDistance = 1.0f;
    [SerializeField] Vector3 direction;

    private List<Vector3> _listPoints;
    public List<Vector3> ListPoints
    {
        get { return _listPoints; }
    }

    #endregion

    #region Fields

    private float _distance = 4.5f;

    private int _currentNode = 0;

    private bool _isPreviewWalking;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _listPoints = new List<Vector3>();
        _listPoints.Add(transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!_isPreviewWalking)
            {   
                CastRayWorld();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isPreviewWalking)
        {
            transform.position += direction * speed * Time.fixedDeltaTime;
            UpdatePathNode();
        }
    }

    #endregion

    public void CastRayWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.origin + (ray.direction * _distance);

        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.rigidbody != null)
        {
            _listPoints.Add(new Vector3(hit.point.x, 0f, hit.point.z));

            _isPreviewWalking = true;
            direction = (_listPoints[_currentNode] - transform.position).normalized;
        }

        //transform.position = positions[positions.Count - 1];
    }

    private void UpdatePathNode()
    {
        float distance = Vector3.Distance(transform.position, _listPoints[_currentNode]);

        if (distance < _maxDistance)
        {
            if (_currentNode != _listPoints.Count - 1)
            {
                _currentNode++;
            }
            else
            {
                _isPreviewWalking = false;
            }

            //transform.LookAt(_pathNodes[_currentNode]);
            direction = (_listPoints[_currentNode] - transform.position).normalized;
        }
    }

    public void Launch()
    {
        _currentNode = 0;
        transform.position = _listPoints[_currentNode];

        ResetListPoints();
    }

    private void ResetListPoints()
    {
        _listPoints = new List<Vector3>();
        _listPoints.Add(transform.position);
    }
}
