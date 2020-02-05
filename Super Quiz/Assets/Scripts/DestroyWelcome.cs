using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWelcome : MonoBehaviour
{
    public GameObject WelcomePanel;
    public void destroyWelcome()

    {
        Destroy(WelcomePanel);
    }

}
