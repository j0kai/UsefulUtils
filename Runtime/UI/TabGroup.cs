using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UsefulUtils
{
    public class TabGroup : MonoBehaviour
    {
        [MenuItem("GameObject/UI/Tabs/Tab Group", false, 10)]
        static void CreateTabGroupObject()
        {
            // Create new undo group.
            Undo.IncrementCurrentGroup();

            // Create object in the active Scene Hierarchy
            GameObject tabGroup = new GameObject("Tab Group");
            tabGroup.AddComponent<TabGroup>();
            tabGroup.AddComponent<RectTransform>();

            // Save undo group
            Undo.RegisterCreatedObjectUndo(tabGroup, "Create Tab Group");
            Undo.RegisterFullObjectHierarchyUndo(tabGroup, "Update Tab Group");

            // Position object under the Scene Hierarchy actively selected GameObject, if any
            GameObject activeGameObject = Selection.activeGameObject;
            GameObjectUtility.SetParentAndAlign(tabGroup, activeGameObject);

            // Select Prefab
            Selection.activeObject = tabGroup;

            // Name undo group
            Undo.SetCurrentGroupName("Add Tab Group");
        }


        /* ---- Inspector ---- */
        [SerializeField] private List<TabButton> m_TabButtons;
        [SerializeField] [ReadOnly] private TabButton m_SelectedTab;

        [Header("Tab Pages")]
        [Space(10)]
        [Tooltip("The pages that will be switched between when switching tabs. " +
        "NOTE: the order of the objects in this list must match their order in the scene heirarchy.")]
        [SerializeField] private List<GameObject> m_ObjectsToSwap;

        public void Subscribe(TabButton btn)
        {
            if(m_TabButtons == null)
                m_TabButtons = new List<TabButton>();

           if(!m_TabButtons.Contains(btn)) 
                m_TabButtons.Add(btn);

            // Ensure the first tab is always the one selected.
            m_SelectedTab = m_TabButtons[0];
            OnTabSelected(btn);
        }

        public void OnTabEnter(TabButton btn)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton btn)
        {
            m_SelectedTab?.Deselect();
            m_SelectedTab = btn;
            m_SelectedTab?.Select();
            
            ResetTabs();

            // Show the selected tab.
            int btnIndex = btn.transform.GetSiblingIndex();
            for(int i = 0; i < m_ObjectsToSwap.Count; ++i)
            {
                if(i == btnIndex)
                    m_ObjectsToSwap[i].SetActive(true);
                else
                    m_ObjectsToSwap [i].SetActive(false);
            }
        }

        public void OnTabExit(TabButton btn)
        {
            ResetTabs();
        }

        public void ResetTabs()
        {
            foreach(TabButton btn in m_TabButtons)
            {
                if (m_SelectedTab != null && btn == m_SelectedTab) continue;
            }
        }
    }
}
