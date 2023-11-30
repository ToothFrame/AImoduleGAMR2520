using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoveScript : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    AStar aStarScript;

    private void Start()
    {
        aStarScript = GameObject.Find("AStarPlane").GetComponent<AStar>();
    }

    void OnMouseDown()
    {
        if (aStarScript.dynamicMode || (!aStarScript.dynamicMode && !aStarScript.searching))
        {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);

            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }
    }

    void OnMouseDrag()
    {
        if (aStarScript.dynamicMode || (!aStarScript.dynamicMode && !aStarScript.searching))
        {
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
            transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);
        }
    }   
}
