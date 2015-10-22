using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(MEventSystem))]
public class MEventTriggerEditor : Editor
{
    private MEventSystem _mEventTrigger;

    protected virtual void OnEnable()
    {
        if (_mEventTrigger == null)
            this._mEventTrigger = (MEventSystem)target;

        UpdateReferences();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        //if prefab parent is null then this is a prefab that hasn't been instantiated yet
        if (_mEventTrigger.Controller == null && PrefabUtility.GetPrefabParent(_mEventTrigger) != null)
        {
            MUtilEditor.DrawErrorBox("Not on the same gameobject or a child of a MController! Assign this anywhere next to / under a MController component.");
        }

        Color origColor = GUI.color;
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < _mEventTrigger.registeredHandlers.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            _mEventTrigger.registeredHandlers[i] = EventLine(_mEventTrigger.registeredHandlers[i]);

            //not in the eventline method for the sake of simplification (removing a return variable is a nono)
            GUI.color = Color.red;
            if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(14) }))
            {
                //not work, crappy code!
                _mEventTrigger.registeredHandlers.Remove(_mEventTrigger.registeredHandlers[i]);
            }
            GUI.color = origColor;
            EditorGUILayout.EndHorizontal();
        }

        GUI.color = Color.green;
        if (GUILayout.Button("+ Add new event", new GUILayoutOption[] { GUILayout.Height(14) }))
        {
            _mEventTrigger.registeredHandlers.Add(new MInteractionEvent());
        }
        GUI.color = origColor;

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_mEventTrigger, _mEventTrigger.name + " changed");
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.EndVertical();    
    }

    public virtual void OnSceneGUI() { }

    private void UpdateReferences()
    {
        if (_mEventTrigger.Controller == null) _mEventTrigger.Controller = _mEventTrigger.transform.GetComponentInParentRecursively<MController>();
        EditorUtility.SetDirty(target);
    }

    public MInteractionEvent EventLine(MInteractionEvent interactionEvent)
    {
        interactionEvent.mapInteractionType = (MInteractionType)EditorGUILayout.EnumPopup(interactionEvent.mapInteractionType, new GUILayoutOption[] { GUILayout.Width(70) });

        EditorGUIUtility.labelWidth = 1;
        interactionEvent.target = (MonoBehaviour)EditorGUILayout.ObjectField("On click", interactionEvent.target, typeof(MonoBehaviour), true);

        if (interactionEvent.target != null)
        {
            List<MethodInfo> methods = MEventDelegate.GetMethods(interactionEvent.target);
            string[] methodNames = new string[methods.Count];

            for (int i = 0; i < methodNames.Length; i++)
            {
                methodNames[i] = methods[i].Name;
            }

            if (methodNames.Length > 0)
            {
                //TODO abstract this, return when found
                int selected = 0;
                for (int i = 0; i < methodNames.Length; i++)
                {
                    if (methodNames[i] == interactionEvent.methodName) selected = i;
                }

                interactionEvent.methodName = methodNames[EditorGUILayout.Popup(selected, methodNames)];
            }
        }
        return interactionEvent;
    }
}