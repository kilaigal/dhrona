using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiperQuestion : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    public Vector3 newpos;
    public Rect rectT;
    public float start_val;
    public float end_val;
    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
        rectT=transform.GetComponentInParent<RectTransform>().rect;
        start_val = 0;
        end_val = rectT.height;
    }
    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.y - data.position.y;
        newpos= panelLocation - new Vector3(0, difference, 0);

        if ((newpos.y > start_val+Screen.height)&&(newpos.y < end_val+start_val))
            {
            transform.position = panelLocation - new Vector3(0, difference, 0);
        }
    }
    public void OnEndDrag(PointerEventData data)
    {

        panelLocation = transform.position;
    }
    
}