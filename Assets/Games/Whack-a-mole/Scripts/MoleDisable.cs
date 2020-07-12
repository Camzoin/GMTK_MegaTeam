using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleDisable : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Animator>().ResetTrigger("Whack");
        gameObject.GetComponent<Animator>().ResetTrigger("Burrow");
        gameObject.GetComponent<Animator>().ResetTrigger("PopUp");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
