using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteSetter : MonoBehaviour {

    [SerializeField]
    static GameObject[] Levels;
    static PaletteSetter _theOnlyOneAllowed;
    private GameObject currentLevel;


    void Start()
    {
        currentLevel = Instantiate(Levels[0]);

    }

    public IEnumerator ChangeWorld()
    {
        
        Debug.Log("palette has been reached");
  
        {
           

            yield return null;
        }
    }

    public void nextLevel()
    {
        Destroy(currentLevel);
        currentLevel = Instantiate(Levels[1]);
       
    }

    


   
}
