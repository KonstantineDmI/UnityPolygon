using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI.CoroutineTween;

namespace TMPro
{
    [AddComponentMenu("UI/Dropdown - TextMeshPro", 35)]
    [RequireComponent(typeof(RectTransform))]
    /// <summary>
    ///   A standard dropdown that presents a list of options when clicked, of which one can be chosen.
    /// </summary>
    /// <remarks>
    /// The dropdown component is a Selectable. When an option is chosen, the label and/or image of the control changes to show the chosen option.
    ///
    /// When a dropdown event occurs a callback is sent to any registered listeners of onValueChanged.
    /// </remarks>
    public class TMP_Dropdown : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler
    {
        protected internal class DropdownItem : MonoBehaviour, IPointerEnterHandler, ICancelHandler
        {
            [SerializeField]
            private TMP_Text m_Text;
            [SerializeField]
            private Image m_Image;
            [SerializeField]
            private RectTransform m_RectTransform;
            [SerializeField]
            private Toggle m_Toggle;

            public TMP_Text text { get { return m_Text; } set { m_Text = value; } }
            public Image image { get { return m_Image; } set { m_Image = value; } }
            public RectTransform rectTransform { get { return m_RectTransform; } set { m_RectTransform = value; } }
            public Toggle toggle { get { return m_Toggle; } set { m_Toggle = value; } }

            public virtual void OnPointerEnter(PointerEventData eventData)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }

            public virtual void OnCancel(BaseEventData eventData)
            {
                TMP_Dropdown dropdown = GetComponentInParent<TMP_Dropdown>();
                if (dropdown)
                    dropdown.Hide();
            }
        }

        [Serializable]
        /// <summary>
        /// Class to store the text and/or image of a single option in the dropdown list.
        /// </summary>
        public class OptionData
        {
            [SerializeField]
            private string m_Text;
            [SerializeField]
            private Sprite m_Image;

            /// <summary>
            /// The text associated with the option.
            /// </summary>
            public string text { get { return m_Text; } set { m_Text = value; } }

            /// <summary>
            /// The image associated with the option.
            /// </summary>
            public Sprite image { get { return m_Image; } set { m_Image = value; } }

            public OptionData() { }

            public OptionData(string text)
            {
                this.text = text;
            }

            public OptionData(Sprite image)
            {
                this.image = image;
            }

            /// <summary>
            /// Create an object representing a single option for the dropdown list.
            /// </summary>
            /// <param name="text">Optional text for the option.</param>
            /// <param name="image">Optional image for the option.</param>
            public OptionData(string text, Sprite image)
            {
                this.text = text;
                this.image = image;
            }
        }

        [Serializable]
        /// <summary>
        /// Class used internally to store the list of options for the dropdown list.
        /// </summary>
        /// <remarks>
        /// The usage of this class is not exposed in the runtime API. It's only relevant for the PropertyDrawer drawing the list of options.
        /// </remarks>
        public class OptionDataList
        {
            [SerializeField]
            private List<OptionData> m_Options;

            /// <summary>
            /// The list of options for the dropdown list.
            /// </summary>
            public List<OptionData> options { get { return m_Options; } set { m_Options = value; } }


            public OptionDataList()
            {
                options = new List<OptionData>();
            }
        }

        [Serializable]
        /// <summary>
        /// UnityEvent callback for when a dropdown current option is changed.
        /// </summary>
        public class DropdownEvent : UnityEvent<int> { }

        // Template used to create the dropdown.
        [SerializeField]
        private RectTransform m_Template;

        /// <summary>
        /// The Rect Transform of the template for the dropdown list.
        /// </summary>
        public RectTransform template { get { return m_Template; } set { m_Template = value; RefreshShownValue(); } }

        // Text to be used as a caption for the current value. It's not required, but it's kept here for convenience.
        [SerializeField]
        private TMP_Text m_CaptionText;

        /// <summary>
        /// The Text component to hold the text of the currently selected option.
        /// </summary>
        public TMP_Text captionText { get { return m_CaptionText; } set { m_CaptionText = value; RefreshShownValue(); } }

        [SerializeField]
        private Image m_CaptionImage;

        /// <summary>
        /// The Image component to hold the image of the currently selected option.
        /// </summary>
        public Image captionImage { get { return m_CaptionImage; } set { m_CaptionImage = value; RefreshShownValue(); } }

        [Space]

        [SerializeField]
        private TMP_Text m_ItemText;

        /// <summary>
        /// The Text component to hold the text of the item.
        /// </summary>
        public TMP_Text itemText { get { return m_ItemText; } set { m_ItemText = value; RefreshShownValue(); } }

        [SerializeField]
        private Image m_ItemImage;

        /// <summary>
        /// The Image component to hold the image of the item
        /// </summary>
        public Image itemImage { get { return m_ItemImage; } set { m_ItemImage = value; RefreshShownValue(); } }

        [Space]

        [SerializeField]
        private int m_Value;

        [Space]

        // Items that will be visible when the dropdown is shown.
        // We box this into its own class so we can use a Property Drawer for it.
        [SerializeField]
        private OptionDataList m_Options = new OptionDataList();

        /// <summary>
        /// The list of possible options. A text string and an image can be specified for each option.
        /// </summary>
        /// <remarks>
        /// This is the list of options within the Dropdown. Each option contains Text and/or image data that you can specify using UI.Dropdown.OptionData before adding to the Dropdown list.
        /// This also unlocks the ability to edit the Dropdown, including the insertion, removal, and finding of options, as well as other useful tools
        /// </remarks>
        /// /// <example>
        /// <code>
        /// //Create a new Dropdown GameObject by going to the Hierarchy and clicking Create>UI>Dropdown - TextMeshPro. Attach this script to the Dropdown GameObject.
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        /// using System.Collections.Generic;
        /// using TMPro;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     //Use these for adding options to the Dropdown List
        ///     TMP_Dropdown.OptionData m_NewData, m_NewData2;
        ///     //The list of messages for the Dropdown
        ///     List<TMP_Dropdown.OptionData> m_Messages = new List<TMP_Dropdown.OptionData>();
        ///
        ///
        ///     //This is the Dropdown
        ///     TMP_Dropdown m_Dropdown;
        ///     string m_MyString;
        ///     int m_Index;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Dropdown GameObject the script is attached to
        ///         m_Dropdown = GetComponent<TMP_Dropdown>();
        ///         //Clear the old options of the Dropdown menu
        ///         m_Dropdown.ClearOptions();
        ///
        ///         //Create a new option for the Dropdown menu which reads "Option 1" and add to messages List
        ///         m_NewData = new TMP_Dropdown.OptionData();
        ///         m_NewData.text = "Option 1";
        ///         m_Messages.Add(m_NewData);
        ///
        ///         //Create a new option for the Dropdown menu which reads "Option 2" and add to messages List
        ///         m_NewData2 = new TMP_Dropdown.OptionData();
        ///         m_NewData2.text = "Option 2";
        ///         m_Messages.Add(m_NewData2);
        ///
        ///         //Take each entry in the message List
        ///         foreach (TMP_Dropdown.OptionData message in m_Messages)
        ///         {
        ///             //Add each entry to the Dropdown
        ///             m_Dropdown.options.Add(message);
        ///             //Make the index equal to the total number of entries
        ///             m_Index = m_Messages.Count - 1;
        ///         }
        ///     }
        ///
        ///     //This OnGUI function is used here for a quick demonstration. See the [[wiki:UISystem|UI Section]] for more information about setting up your own UI.
        ///     void OnGUI()
        ///     {
        ///         //TextField for user to type new entry to add to Dropdown
        ///         m_MyString = GUI.TextField(new Rect(0, 40, 100, 40), m_MyString);
        ///
        ///         //Press the "Add" Button to add a new entry to the Dropdown
        ///         if (GUI.Button(new Rect(0, 0, 100, 40), "Add"))
        ///         {
        ///             //Make the index the last number of entries
        ///             m_Index = m_Messages.Count;
        ///             //Create a temporary option
        ///             TMP_Dropdown.OptionData temp = new TMP_Dropdown.OptionData();
        ///             //Make the option the data from the TextField
        ///             temp.text = m_MyString;
        ///
        ///             //Update the messages list with the TextField data
        ///             m_Messages.Add(temp);
        ///
        ///             //Add the Textfield data to the Dropdown
        ///             m_Dropdown.options.Insert(m_Index, temp);
        ///         }
        ///
        ///         //Press the "Remove" button to delete the selected option
        ///         if (GUI.Button(new Rect(110, 0, 100, 40), "Remove"))
        ///         {
        ///             //Remove the current selected item from the Dropdown from the messages List
        ///             m_Messages.RemoveAt(m_Dropdown.value);
        ///             //Remove the current selection from the Dropdown
        ///             m_Dropdown.options.RemoveAt(m_Dropdown.value);
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<OptionData> options
        {
            get { return m_Options.options; }
            set { m_Options.options = value; RefreshShownValue(); }
        }

        [Space]

        // Notification triggered when the dropdown changes.
        [SerializeField]
        private DropdownEvent m_OnValueChanged = new DropdownEvent();

        /// <summary>
        /// A UnityEvent that is invoked when a user has clicked one of the options in the dropdown list.
        /// </summary>
        /// <remarks>
        /// Use this to detect when a user selects one or more options in the Dropdown. Add a listener to perform an action when this UnityEvent detects a selection by the user. See https://unity3d.com/learn/tutorials/topics/scripting/delegates for more information on delegates.
        /// </remarks>
        /// <example>
        ///  <code>
        /// //Create a new Dropdown GameObject by going to the Hierarchy and clicking Create>UI>Dropdown - TextMeshPro. Attach this script to the Dropdown GameObject.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        /// using TMPro;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     TMP_Dropdown m_Dropdown;
        ///     public Text m_Text;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Dropdown GameObject
        ///         m_Dropdown = GetComponent<TMP_Dropdown>();
        ///         //Add listener for when the value of the Dropdown changes, to take action
        ///         m_Dropdown.onValueChanged.AddListener(delegate {
        ///                 DropdownValueChanged(m_Dropdown);
        ///             });
        ///
        ///         //Initialize the Text to say the first value of the Dropdown
        ///         m_Text.text = "First Value : " + m_Dropdown.value;
        ///     }
        ///
        ///     //Output the new value of the Dropdown into Text
        ///     void DropdownValueChanged(TMP_Dropdown change)
        ///     {
        ///         m_Text.text =  "New Value : " + change.value;
        ///     }
        /// }
        /// </code>
        /// </example>
        public DropdownEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

        private GameObject m_Dropdown;
        private GameObject m_Blocker;
        private List<DropdownItem> m_Items = new List<DropdownItem>();
        private TweenRunner<FloatTween> m_AlphaTweenRunner;
        private bool validTemplate = false;

        private static OptionData s_NoOptionData = new OptionData();

        /// <summary>
        /// The Value is the index number of the current selection in the Dropdown. 0 is the first option in the Dropdown, 1 is the second, and so on.
        /// </summary>
        /// <example>
        /// <code>
        /// //Create a new Dropdown GameObject by going to the Hierarchy and clicking Create>UI>Dropdown - TextMeshPro. Attach this script to the Dropdown GameObject.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        /// using TMPro;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     //Attach this script to a Dropdown GameObject
        ///     TMP_Dropdown m_Dropdown;
        ///     //This is the string that stores the current selection m_Text of the Dropdown
        ///     string m_Message;
        ///     //This Text outputs the current selection to the screen
        ///     public Text m_Text;
        ///     //This is the index value of the Dropdown
        ///     int m_DropdownValue;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the DropDown component from the GameObject
        ///         m_Dropdown = GetComponent<TMP_Dropdown>();
        ///         //Output the first Dropdown index value
        ///         Debug.Log("Starting Dropdown Value : " + m_Dropdown.value);
        ///     }
        ///
        ///     void Update()
        ///     {
        ///         //Keep the current index of the Dropdown in a variable
        ///         m_DropdownValue = m_Dropdown.value;
        ///         //Change the message to say the name of the current Dropdown selection using the value
        ///         m_Message = m_Dropdown.options[m_DropdownValue].text;
        ///         //Change the on screen Text to reflect the current Dropdown selection
        ///         m_Text.text = m_Message;
        ///     }
        /// }
        /// </code>
        /// </example>
        public int value
        {
            get
            {
                return m_Value;
            }
            set
            {
                SetValue(value);
            }
        }

        //                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                (int i = 0; i < 4; i++)
                {
                    Vector3 corner = rootCanvasRectTransform.InverseTransformPoint(corners[i]);
                    if ((corner[axis] < rootCanvasRect.min[axis] && !Mathf.Approximately(corner[axis], rootCanvasRect.min[axis])) ||
                        (corner[axis] > rootCanvasRect.max[axis] && !Mathf.Approximately(corner[axis], rootCanvasRect.max[axis])))
                    {
                        outside = true;
                        break;
                    }
                }
                if (outside)
                    RectTransformUtility.FlipLayoutOnAxis(dropdownRectTransform, axis, false, false);
            }

            for (int i = 0; i < m_Items.Count; i++)
            {
                RectTransform itemRect = m_Items[i].rectTransform;
                itemRect.anchorMin = new Vector2(itemRect.anchorMin.x, 0);
                itemRect.anchorMax = new Vector2(itemRect.anchorMax.x, 0);
                itemRect.anchoredPosition = new Vector2(itemRect.anchoredPosition.x, offsetMin.y + itemSize.y * (m_Items.Count - 1 - i) + itemSize.y * itemRect.pivot.y);
                itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x, itemSize.y);
            }

            // Fade in the popup
            AlphaFadeList(0.15f, 0f, 1f);

            // Make drop-down template and item template inactive
            m_Template.gameObject.SetActive(false);
            itemTemplate.gameObject.SetActive(false);

            m_Blocker = CreateBlocker(rootCanvas);
        }

        /// <summary>
        /// Create a blocker that blocks clicks to other controls while the dropdown list is open.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain a blocker GameObject.
        /// </remarks>
        /// <param name="rootCanvas">The root canvas the dropdown is under.</param>
        /// <returns>The created blocker object</returns>
        protected virtual GameObject CreateBlocker(Canvas rootCanvas)
        {
            // Create blocker GameObject.
            GameObject blocker = new GameObject("Blocker");

            // Setup blocker RectTransform to cover entire root canvas area.
            RectTransform blockerRect = blocker.AddComponent<RectTransform>();
            blockerRect.SetParent(rootCanvas.transform, false);
            blockerRect.anchorMin = Vector3.zero;
            blockerRect.anchorMax = Vector3.one;
            blockerRect.sizeDelta = Vector2.zero;

            // Make blocker be in separate canvas in same layer as dropdown and in layer just below it.
            Canvas blockerCanvas = blocker.AddComponent<Canvas>();
            blockerCanvas.overrideSorting = true;
            Canvas dropdownCanvas = m_Dropdown.GetComponent<Canvas>();
            blockerCanvas.sortingLayerID = dropdownCanvas.sortingLayerID;
            blockerCanvas.sortingOrder = dropdownCanvas.sortingOrder - 1;

            // Add raycaster since it's needed to block.
            blocker.AddComponent<GraphicRaycaster>();

            // Add image since it's needed to block, but make it clear.
            Image blockerImage = blocker.AddComponent<Image>();
            blockerImage.color = Color.clear;

            // Add button since it's needed to block, and to close the dropdown when blocking area is clicked.
            Button blockerButton = blocker.AddComponent<Button>();
            blockerButton.onClick.AddListener(Hide);

            return blocker;
        }

        /// <summary>
        /// Convenience method to explicitly destroy the previously generated blocker object
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of a blocker GameObject that blocks clicks to other controls while the dropdown list is open.
        /// </remarks>
        /// <param name="blocker">The blocker object to destroy.</param>
        protected virtual void DestroyBlocker(GameObject blocker)
        {
            Destroy(blocker);
        }

        /// <summary>
        /// Create the dropdown list to be shown when the dropdown is clicked. The dropdown list should correspond to the provided template GameObject, equivalent to instantiating a copy of it.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain a dropdown list GameObject.
        /// </remarks>
        /// <param name="template">The template to create the dropdown list from.</param>
        /// <returns>The created drop down list gameobject.</returns>
        protected virtual GameObject CreateDropdownList(GameObject template)
        {
            return (GameObject)Instantiate(template);
        }

        /// <summary>
        /// Convenience method to explicitly destroy the previously generated dropdown list
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of a dropdown list GameObject.
        /// </remarks>
        /// <param name="dropdownList">The dropdown list GameObject to destroy</param>
        protected virtual void DestroyDropdownList(GameObject dropdownList)
        {
            Destroy(dropdownList);
        }

        /// <summary>
        /// Create a dropdown item based upon the item template.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain an option item.
        /// The option item should correspond to the provided template DropdownItem and its GameObject, equivalent to instantiating a copy of it.
        /// </remarks>
        /// <param name="itemTemplate">e template to create the option item from.</param>
        /// <returns>The created dropdown item component</returns>
        protected virtual DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            return (DropdownItem)Instantiate(itemTemplate);
        }

        /// <summary>
        ///  Convenience method to explicitly destroy the previously generated Items.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of an option item.
        /// Likely no action needed since destroying the dropdown list destroys all contained items as well.
        /// </remarks>
        /// <param name="item">The Item to destroy.</param>
        protected virtual void DestroyItem(DropdownItem item) { }

        // Add a new drop-down list item with the specified values.
        private DropdownItem AddItem(OptionData data, bool selected, DropdownItem itemTemplate, List<DropdownItem> items)
        {
            // Add a new item to the dropdown.
            DropdownItem item = CreateItem(itemTemplate);
            item.rectTransform.SetParent(itemTemplate.rectTransform.parent, false);

            item.gameObject.SetActive(true);
            item.gameObject.name = "Item " + items.Count + (data.text != null ? ": " + data.text : "");

            if (item.toggle != null)
            {
                item.toggle.isOn = false;
            }

            // Set the item's data
            if (item.text)
                item.text.text = data.text;
            if (item.image)
            {
                item.image.sprite = data.image;
                item.image.enabled = (item.image.sprite != null);
            }

            items.Add(item);
            return item;
        }

        private void AlphaFadeList(float duration, float alpha)
        {
            CanvasGroup group = m_Dropdown.GetComponent<CanvasGroup>();
            AlphaFadeList(duration, group.alpha, alpha);
        }

        private void AlphaFadeList(float duration, float start, float end)
        {
            if (end.Equals(start))
                return;

            FloatTween tween = new FloatTween { duration = duration, startValue = start, targetValue = end };
            tween.AddOnChangedCallback(SetAlpha);
            tween.ignoreTimeScale = true;
            m_AlphaTweenRunner.StartTween(tween);
        }

        private void SetAlpha(float alpha)
        {
            if (!m_Dropdown)
                return;
            CanvasGroup group = m_Dropdown.GetComponent<CanvasGroup>();
            group.alpha = alpha;
        }

        /// <summary>
        /// Hide the dropdown list. I.e. close it.
        /// </summary>
        public void Hide()
        {
            if (m_Dropdown != null)
            {
                AlphaFadeList(0.15f, 0f);

                // User could have disabled the dropdown during the OnValueChanged call.
                if (IsActive())
                    StartCoroutine(DelayedDestroyDropdownList(0.15f));
            }
            if (m_Blocker != null)
                DestroyBlocker(m_Blocker);
            m_Blocker = null;
            Select();
        }

        private IEnumerator DelayedDestroyDropdownList(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            ImmediateDestroyDropdownList();
        }

        private void ImmediateDestroyDropdownList()
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i] != null)
                    DestroyItem(m_Items[i]);
            }
            m_Items.Clear();

            if (m_Dropdown != null)
                DestroyDropdownList(m_Dropdown);
            m_Dropdown = null;
        }

        // Change the value and hide the dropdown.
        private void OnSelectItem(Toggle toggle)
        {
            if (!toggle.isOn)
                toggle.isOn = true;

            int selectedIndex = -1;
            Transform tr = toggle.transform;
            Transform parent = tr.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == tr)
                {
                    // Subtract one to account for template child.
                    selectedIndex = i - 1;
                    break;
                }
            }

            if (selectedIndex < 0)
                return;

            value = selectedIndex;
            Hide();
        }
    }
}