using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCameraController : MonoBehaviour
{
    public float zoomLevel;
    public float dragSpeed;

    public InputField PositionX;
    public InputField PositionY;
    public Transform LandSearch;

    bool locating;
    void Update()
    {
        if (!locating) {
            zoomLevel -= Input.mouseScrollDelta.y;
            zoomLevel = Mathf.Clamp(zoomLevel, 6, 20);

            if (Input.GetMouseButton(2))
                transform.position += new Vector3(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, 0, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed);

            transform.position = new Vector3(transform.position.x, zoomLevel, transform.position.z);
        }
        else
        {
            if (GameObject.Find("GridController").GetComponent<GridController>().selected != null)
                GameObject.Find("GridController").GetComponent<GridController>().selected.Selected(false);
            GameObject.Find("GridController").GetComponent<GridController>().selected = LandSearch.GetComponent<IndexBlock>();
            LandSearch.GetComponent<IndexBlock>().Selected(true, true);

            transform.position = Vector3.Lerp(transform.position, new Vector3(LandSearch.position.x, transform.position.y, LandSearch.position.z), 1 * Time.deltaTime);
            Vector3 distance = new Vector3(transform.position.x - LandSearch.position.x, 0, transform.position.z - LandSearch.position.z);
            Debug.Log(distance.x);
            if (distance.x >= -1 && distance.x <= 1 && distance.z >= -1 && distance.z <= 1)
                locating = false;
        }
    }

    public void FindLand()
    {
        int x = int.Parse(PositionX.text);
        int y = int.Parse(PositionY.text);

        if (PositionX.text == string.Empty || PositionY.text == string.Empty)
            return;

        string search = $"X {x.ToString("000")}, Y {y.ToString("000")}";
        LandSearch = GameObject.Find(search).transform;
        if (LandSearch != null) {
            locating = true;
        }
        else
            return;
    }
}
