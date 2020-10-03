using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Script Parameters

    [SerializeField] private float speed = 5f;
    [SerializeField] private float _maxDistance = 1.0f;
    [SerializeField] Vector3 direction;

    #endregion

    #region Fields

    private float _distance = 4.5f;

    private List<Vector3> _listPoints;
    private int currentNode = 0;
    private float _tolerance;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _listPoints = new List<Vector3>();

        _tolerance = speed * Time.deltaTime;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRayWorld();
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
            _listPoints.Add(new Vector3(hit.point.x, 0.1f, hit.point.z));
        }

        //transform.position = positions[positions.Count - 1];
    }

    private void UpdatePathNode()
    {
        float distance = Vector3.Distance(transform.position, _listPoints[currentNode]);
        Debug.Log(distance);
        if (distance < _maxDistance)
        {
            if (currentNode != _listPoints.Count - 1)
            {
                currentNode++;
            }
            //transform.LookAt(_pathNodes[_currentNode]);
            direction = (_listPoints[currentNode] - transform.position).normalized;
        }
    }
}
