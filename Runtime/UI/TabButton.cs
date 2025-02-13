using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace UsefulUtils
{
    [RequireComponent(typeof(Image))]
    public class TabButton : Selectable, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [MenuItem("GameObject/UI/Tabs/Tab Button", false, 11)]
        static void CreateTabButtonObject()
        {
            // Create new undo group.
            Undo.IncrementCurrentGroup();

            // Create object in the active Scene Hierarchy
            GameObject tabButton = new GameObject("Tab Button");
            tabButton.AddComponent<TabButton>();
            tabButton.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
            Image tabBtnImg = tabButton.GetComponent<Image>();
            tabBtnImg.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            tabBtnImg.type = Image.Type.Sliced;

            // Create and initialise the text component child object of the button.
            GameObject textObj = new GameObject("Text (TMP)");
            textObj.AddComponent<TextMeshProUGUI>();
            textObj.transform.position = Vector3.zero;
            textObj.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);

            var textSettings = textObj.GetComponent<TextMeshProUGUI>();
            textSettings.alignment = TextAlignmentOptions.Center;
            textSettings.verticalAlignment = VerticalAlignmentOptions.Middle;
            textSettings.fontSize = 24;
            textSettings.color = Color.black;
            textSettings.text = "Button";

            // Save undo group
            Undo.RegisterCreatedObjectUndo(tabButton, "Create Tab Button");
            Undo.RegisterFullObjectHierarchyUndo(tabButton, "Update Tab Button");

            // Position object under the Scene Hierarchy actively selected GameObject, if any
            GameObject activeGameObject = Selection.activeGameObject;
            GameObjectUtility.SetParentAndAlign(tabButton, activeGameObject);
            GameObjectUtility.SetParentAndAlign(textObj, tabButton);

            // Select Prefab
            Selection.activeObject = tabButton;

            // Name undo group
            Undo.SetCurrentGroupName("Add Tab Button");
        }

        /* ---- Inspector ---- */
        [Tooltip("The tab group that this tab button is associated with.")]
        [SerializeField] private TabGroup m_TabGroup;

        /* ---- Events ---- */
        [Space(10)]
        public UnityEvent onTabSelected;
        public UnityEvent onTabDeselected;
        public override void Select() { base.Select(); onTabSelected?.Invoke(); }
        public void Deselect() => onTabDeselected?.Invoke();

        public override void OnPointerEnter(PointerEventData eventData) { base.OnPointerEnter(eventData); m_TabGroup.OnTabEnter(this); }
        public void OnPointerClick(PointerEventData eventData) => m_TabGroup.OnTabSelected(this);
        public override void OnPointerExit(PointerEventData eventData) { base.OnPointerExit(eventData); m_TabGroup.OnTabExit(this); }

        protected override void Start()
        {
            if(Application.isPlaying)
                m_TabGroup.Subscribe(this);
        }

    }
}
