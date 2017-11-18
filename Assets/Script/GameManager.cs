using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public SpriteUtils SpriteUtils;
    public LayerMask layerMask;
    public Texture2D icon;
    //private PointerEventData pointerData;
    // Use this for initialization
    void Start () {
        instance = this;
        SpriteUtils = GetComponentInChildren<SpriteUtils>();
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
