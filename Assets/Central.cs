using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    public static Central Instance;

    int level;
    // Start is called before the first frame update
    void Start()
    {
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (level)
        {
            case 1:
                
                break;
            case 2:
                break;
            default:
                break;
        }
    }
}