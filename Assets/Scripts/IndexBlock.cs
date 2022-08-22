using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexBlock : MonoBehaviour
{
    public bool selected;
    public int IndexX;
    public int IndexY;

    public void Selected(bool state, bool find = false)
    {
        if (find) {
            selected = true;
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
        }
        else if (state) {
            selected = true;
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        else if (!state) {
            selected = false;
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
    }
}
