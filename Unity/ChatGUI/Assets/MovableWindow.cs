using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MovableWindow : MonoBehaviour, IBeginDragHandler, IDragHandler {

	public Vector3 offset;

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		offset = transform.position - (Vector3)eventData.position;

	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = (Vector3) eventData.position + offset;
		//will show sthe position of the mouse
	}

	#endregion
}
