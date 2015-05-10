using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class HighlightHelper
{
	static HighlightHelper()
	{
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
		
		SceneView.onSceneGUIDelegate += OnSceneGUIDelegate;
	}
	
	private static readonly Color HoverColor = new Color(1, 1, 1, 0.75f);
	private static readonly Color DragColor = new Color(1f, 0, 0, 0.75f);
	
	private static void OnSceneGUIDelegate(SceneView sceneView)
	{
		switch (Event.current.type)
		{
		case EventType.DragUpdated:
		case EventType.DragPerform:
		case EventType.DragExited:
			sceneView.Repaint();
			break;
		}
		
		if (Event.current.type == EventType.repaint)
		{
			var drawnInstanceIDs = new HashSet<int>();
			
			Color handleColor = Handles.color;
			
			Handles.color = DragColor;
			foreach (var objectReference in DragAndDrop.objectReferences)
			{
				var gameObject = objectReference as GameObject;
				
				if (gameObject && gameObject.activeInHierarchy)
				{
					DrawObjectBounds(gameObject);
					
					drawnInstanceIDs.Add(gameObject.GetInstanceID());
				}
			}
			
			Handles.color = HoverColor;
			if (_hoveredInstance != 0 && !drawnInstanceIDs.Contains(_hoveredInstance))
			{
				GameObject sceneGameObject = EditorUtility.InstanceIDToObject(_hoveredInstance) as GameObject;
				
				if (sceneGameObject)
				{
					DrawObjectBounds(sceneGameObject);
				}
			}
			
			Handles.color = handleColor;
		}
	}
	
	private static void DrawObjectBounds(GameObject sceneGameObject)
	{
		var bounds = new Bounds(sceneGameObject.transform.position, Vector3.one);
		foreach (var renderer in sceneGameObject.GetComponents<Renderer>())
		{
			Bounds rendererBounds = renderer.bounds;
			rendererBounds.center = sceneGameObject.transform.position;
			bounds.Encapsulate(renderer.bounds);
		}
		
		
		float onePixelOffset = HandleUtility.GetHandleSize(bounds.center) * 1 / 64f;
		
		float circleSize = bounds.size.magnitude * 0.5f;
		
		Handles.CircleCap(0, bounds.center,
		                  sceneGameObject.transform.rotation, circleSize - onePixelOffset);
		Handles.CircleCap(0, bounds.center,
		                  sceneGameObject.transform.rotation, circleSize + onePixelOffset);
		Handles.CircleCap(0, bounds.center, sceneGameObject.transform.rotation, circleSize);
	}
	
	private static int _hoveredInstance = 0;
	
	private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
	{
		var current = Event.current;
		
		switch (current.type)
		{
		case EventType.repaint:
			if (selectionRect.Contains(current.mousePosition))
			{
				if (_hoveredInstance != instanceID)
				{
					_hoveredInstance = instanceID;
					if (SceneView.lastActiveSceneView)
					{
						SceneView.lastActiveSceneView.Repaint();
					}
				}
			}
			else
			{
				if (_hoveredInstance == instanceID)
				{
					_hoveredInstance = 0;
					if (SceneView.lastActiveSceneView)
					{
						SceneView.lastActiveSceneView.Repaint();
					}
				}
			}
			break;
		case EventType.MouseDrag:
		case EventType.DragUpdated:
		case EventType.DragPerform:
		case EventType.DragExited:
			if (SceneView.lastActiveSceneView)
			{
				SceneView.lastActiveSceneView.Repaint();
			}
			break;
		}
		
		EditorApplication.RepaintHierarchyWindow();
	}
}