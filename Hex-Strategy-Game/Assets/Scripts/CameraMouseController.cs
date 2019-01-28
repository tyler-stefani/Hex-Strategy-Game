using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isDragging = false;
    Vector3 lastMousePosition;

    // Update is called once per frame
    void Update()
    {
        // Camera Controls
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
        Vector3 planeIntersect = mouseRay.origin - (mouseRay.direction * rayLength);

        if (Input.GetMouseButtonDown(0))
        {
            // Mouse just clicked (on only the map?)
            isDragging = true;

            lastMousePosition = planeIntersect;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            // Mouse just let up
            isDragging = false;
        }

        // Translate camera in the opposite direction of dragging
        if (isDragging)
        {
            Vector3 difference = lastMousePosition - planeIntersect;
            Camera.main.transform.Translate(difference, Space.World);
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            rayLength = (mouseRay.origin.y / mouseRay.direction.y);
            lastMousePosition = mouseRay.origin - (mouseRay.direction * rayLength);
        }

        // Zooming with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.0)
        {
            // Camera moves towards the intersect on the game board for the mouse
            // TODO: add lerp for smooth scrolls
            Vector3 direction = planeIntersect - Camera.main.transform.position;
            Camera.main.transform.Translate(direction * scroll, Space.World);
        }
    }
}
