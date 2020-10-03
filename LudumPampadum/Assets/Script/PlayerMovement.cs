using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Script Parameters

    public float distance = 4.5f;

    #endregion

    #region Fields

    private List<Vector3> positions;

    #endregion

    #region Unity Methods

    private void Start()
    {
        positions = new List<Vector3>();
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
        Vector3 point = ray.origin + (ray.direction * distance);
        //Debug.Log("World point " + point);

        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.rigidbody != null)
        {
            positions.Add(new Vector3(hit.point.x, 0.1f, hit.point.z));
        }

        transform.position = positions[positions.Count - 1];
    }
}
