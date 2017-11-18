using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public LayerMask layerMask;
    public Texture2D icon;
    //private PointerEventData pointerData;
    // Use this for initialization
    void Start () {
        instance = this;

        //Cursor.SetCursor(icon, Vector2.zero, CursorMode.Auto);
    }
	
	// Update is called once per frame
	void Update () {
        MouseControls();

    }

    public void MouseControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {


                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.main.ScreenToViewportPoint(Input.mousePosition)- Camera.main.transform.forward*0.01f, Camera.main.transform.forward, out hitInfo, Mathf.Infinity))
                {
                    Debug.Log("aaaa");
                    StartCoroutine(ScaleMe(hitInfo.transform));
                    //pointerData = new PointerEventData(EventSystem.current)
                    //{
                    //    position = Input.mousePosition
                    //};
                    //List<RaycastResult> results = new List<RaycastResult>();
                    //EventSystem.current.RaycastAll(pointerData, results);
                    //if (results.Count > 0)
                    //{

                    //    if (results[0].gameObject.gameObject.tag == "card")
                    //    {
                    //        StartCoroutine(ScaleMe(results[0].gameObject.transform.gameObject.transform));
                    //    }

                    //}
                }
            }

        }
    }
    IEnumerator ScaleMe(Transform objTr)
    {
        objTr.localPosition += new Vector3(0, 60f, 0);
        //objTr.localScale = new Vector3(1, 1.2f,1);
        yield return new WaitForSeconds(0.5f);

        objTr.localPosition -= new Vector3(0, 60f, 0);
        //objTr.localScale = new Vector3(1, 1f, 1);
    }
}

    public static GameManager instance;
    public LayerMask layerMask;
    public Texture2D icon;

    public List<CardInstance> selectedCards;
    public Dictionary<CardInstance, int> dictionarySelectedCardsValues;

    public bool clickOnValue =false;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        instance.selectedCards = new List<CardInstance>();
        instance.dictionarySelectedCardsValues = new Dictionary<CardInstance, int>();
        //Cursor.SetCursor(icon, Vector2.zero, CursorMode.Auto);
    public void MouseControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit2D hitInfo;
                clickOnValue = false;
                hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.forward * 0.01f, Camera.main.transform.forward, ~layerMask);
                if (hitInfo)
                {
                    if (hitInfo.transform.tag == "value" && hitInfo.transform.gameObject.GetComponentInParent<CardInstance>().IsReady && selectedCards.Contains(hitInfo.transform.gameObject.GetComponentInParent<CardInstance>()))
                    {
                        clickOnValue = true;
                        CardInstance cardInstance = hitInfo.transform.gameObject.GetComponentInParent<CardInstance>();
                        bool alreadySelected = false;

                        if (instance.dictionarySelectedCardsValues.ContainsKey(cardInstance))
                        {
                            if (hitInfo.transform.GetComponent<Outline>().effectColor == Color.red)
                            {
                                alreadySelected = true;
                            }

                            instance.dictionarySelectedCardsValues.Remove(cardInstance);
                            for (int i = 0; i < cardInstance.transform.childCount; i++)
                            {
                                cardInstance.transform.GetChild(i).GetComponent<Outline>().effectColor = Color.white;
                            }     

                        }

                        if(!alreadySelected)
                        {
                            instance.dictionarySelectedCardsValues.Add(cardInstance, Convert.ToInt32(hitInfo.transform.GetComponent<Text>().text));
                            hitInfo.transform.GetComponent<Outline>().effectColor = Color.red;

                        }


                    }
                }

                hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.forward * 0.01f, Camera.main.transform.forward, layerMask);
                if (!clickOnValue && hitInfo)
                {
                    if( hitInfo.transform.tag == "card" && hitInfo.transform.gameObject.GetComponent<CardInstance>().IsReady)
                    {
                        CardInstance cardInstance = hitInfo.transform.gameObject.GetComponent<CardInstance>();
                        if(!selectedCards.Contains(cardInstance))
                        {
                            instance.selectedCards.Add(cardInstance);
                            hitInfo.transform.localPosition += new Vector3(0, 60f, 0);
                        }
                        else
                        {
                            instance.selectedCards.Remove(cardInstance);
                            hitInfo.transform.localPosition -= new Vector3(0, 60f, 0);

                            instance.dictionarySelectedCardsValues.Remove(cardInstance);
                            for (int i = 0; i < cardInstance.transform.childCount; i++)
                            {
                                cardInstance.transform.GetChild(i).GetComponent<Outline>().effectColor = Color.white;
                            }

                        }
                    }
                }
            }

        }


