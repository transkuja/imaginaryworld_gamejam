using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour {

    #region Properties
    public Card cardData;

    private MeshRenderer refMeshRenderer;
    #endregion

    public void Start()
    {
        refMeshRenderer = GetComponent<MeshRenderer>();

        //Material mat = refMeshRenderer.material;
    }

    public void Update()
    {

    }


    public void ChangeShaderMask(int maskIndice)
    {
        // Do nothing for now
    }

    public void RefreshValues()
    {
        for (int i = 0; i < cardData.combinationValues.Length; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<Text>().text = "" + cardData.combinationValues[i];
        } 

    }
}
