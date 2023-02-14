using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectShip : MonoBehaviour
{
    public int index;
    private GameObject selection;
    // Start is called before the first frame update
    void Start()
    {
        selection = GameObject.Find("Selection");
        if(MainManager.Instance.shipSelection == index)
        {
            setPosition();
        }
    }

    private void OnMouseDown()
    {
        MainManager.Instance.shipSelection = index;
        MainManager.Instance.SavePlayerInfo();
        setPosition();
    }

    private void setPosition()
    {
        selection.transform.position = gameObject.transform.position;
       
    }
}
