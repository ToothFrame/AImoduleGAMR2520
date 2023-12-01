using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{

    public AStarShip _aStar;
    public GameObject targetGameObject;
    List<Node> path;
    private Vector3 velocity;
    private Vector3 velocityRot;
    private Vector3 velocityCentre;
    Vector3 centrePosNew = Vector3.zero;
    Vector3 centrePosHolder;


    private void Update()
    {
        //Request Path
        path = _aStar.RequestPath(this.gameObject, targetGameObject);
        //MoveShip function
        MoveShip();

    }

    void MoveShip()
    {
        if (path != null && path.Count > 3)
        {
            Vector3 centrePos = Vector3.SmoothDamp(centrePosHolder, FindCentre(path), ref velocityCentre, 0.3f);

            Vector3 goTo = Vector3.SmoothDamp(transform.position, centrePos, ref velocity,
                0.1f * Mathf.InverseLerp(-5f, 5f, Vector3.Distance(transform.position, targetGameObject.transform.position)));

            GetComponent<Rigidbody>().MovePosition(goTo);

            transform.LookAt(Vector3.SmoothDamp(transform.position, centrePos, ref velocityRot, 1f));

        }
    }

    Vector3 FindCentre(List<Node> _path)
    {
        float x = 0;
        float y = 0;
        float z = 0;

        centrePosHolder = centrePosNew;

        for (int i = 0; i < 4; i++)
        {
            x += _path[i].nodePos.x;
            y += _path[i].nodePos.y;
            z += _path[i].nodePos.z;

        }

        x = x / 4;
        y = y / 4;
        z = z / 4;

        centrePosNew = new Vector3(x, y, z);

        return centrePosNew;
    }






}
