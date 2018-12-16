using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    public struct CharacterAttributes
    {
        public Color HairColor;
        public Color GlassesColor;
        public float WidthVariation;
        public float HeightVariation;
    }

    public struct TargetPoint
    {
        public Vector3 Target;
        public Vector3 Start;
    }

    public enum SwipeDirection
    {
        LEFT,
        RIGHT
    }

    [Header("Input")]
    [SerializeField] GameObject[] m_modelTemplates = null;
    [SerializeField] [Range(0.0f, 30.0f)] float m_modelSpacing = 15.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_swipeSpeed = 1.0f;
    [SerializeField] CanvasTransitioner.InterpolationType m_interpolationType = CanvasTransitioner.InterpolationType.EXPO_OUT;
    [SerializeField] ColorPicker m_hairColorSelector = null;
    [SerializeField] ColorPicker m_glassesColorSelector = null;
    [SerializeField] FloatPicker m_widthSelector = null;
    [SerializeField] FloatPicker m_heightSelector = null;
    [Space(15)]
    [Header("Values")]
    [SerializeField] [Range(0.0f, 5.0f)] float m_widthVariation = 0.25f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_heightVariation = 0.5f;

    ModelViewData[] m_models = null;
    TargetPoint[] m_targetPoints;
    CharacterAttributes m_currentCharacterAttributes;
    float m_startingWidth;
    float m_startingHeight;
    int m_currentModel = 0;
    float m_time = 0.0f;
    bool m_swiping = false;

    private void Start()
    {
        CreateModels();
        InitializeCharacterAttributes();
        SetTargetPositions(true);
        SetCallbacks();
    }

    void InitializeCharacterAttributes()
    {
        ModelViewData curModel = m_models[m_currentModel];
        m_startingWidth = curModel.transform.localScale.x;
        m_startingHeight = curModel.transform.localScale.y;

        m_currentCharacterAttributes = new CharacterAttributes() {
            HairColor = curModel.HairMaterial.color, GlassesColor = curModel.GlassesMaterial.color,
            WidthVariation = 0.0f, HeightVariation = 0.0f };
    }

    void CreateModels()
    {
        m_models = new ModelViewData[m_modelTemplates.Length];
        m_targetPoints = new TargetPoint[m_modelTemplates.Length];
        for (int i = 0; i < m_modelTemplates.Length; i++)
        {
            GameObject go = Instantiate(m_modelTemplates[i], transform);
            ModelViewData data = go.GetComponent<ModelViewData>();
            m_models[i] = data;
        }
    }

    void SetCallbacks()
    {
        m_hairColorSelector.Initialize(m_currentCharacterAttributes.HairColor);
        m_glassesColorSelector.Initialize(m_currentCharacterAttributes.GlassesColor);
        m_hairColorSelector.OnValueChange += SetCharacterHairColor;
        m_glassesColorSelector.OnValueChange += SetCharacterGlassesColor;
        m_widthSelector.Initialize(m_widthVariation);
        m_heightSelector.Initialize(m_heightVariation);
        m_widthSelector.OnValueChange += SetCharacterWidthVariation;
        m_heightSelector.OnValueChange += SetCharacterHeightVariation;
    }

    void SetTargetPositions(bool setModelPositions)
    {
        for (int i = 0; i < m_models.Length; i++)
        {
            int indexFromCurrent = i - m_currentModel;
            float offset = m_modelSpacing * indexFromCurrent;
            Vector3 target = new Vector3(offset, m_models[i].transform.position.y, 0.0f);
            m_targetPoints[i].Start = m_targetPoints[i].Target;
            m_targetPoints[i].Target = target;
            if (setModelPositions)
                m_models[i].transform.localPosition = target;
        }
    }

    public void SwipeModel(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.LEFT:
                m_currentModel--;
                break;
            case SwipeDirection.RIGHT:
                m_currentModel++;
                break;
        }

        if (m_currentModel < 0)
            m_currentModel = m_models.Length - 1;
        else if (m_currentModel > m_models.Length - 1)
            m_currentModel = 0;
        
        SetTargetPositions(false);
        m_swiping = true;
        m_time = 0.0f;

        ApplyCharacterAttributes();
    }

    public void SetCharacterHairColor(Color color)
    {
        m_currentCharacterAttributes.HairColor = color;
        ApplyCharacterAttributes();
    }

    public void SetCharacterGlassesColor(Color color)
    {
        m_currentCharacterAttributes.GlassesColor = color;
        ApplyCharacterAttributes();
    }

    public void SetCharacterWidthVariation(float widthVariation)
    {
        float width = Mathf.Clamp(widthVariation, -m_widthVariation, m_widthVariation);
        m_currentCharacterAttributes.WidthVariation = width;
        ApplyCharacterAttributes();
    }

    public void SetCharacterHeightVariation(float heightVariation)
    {
        float height = Mathf.Clamp(heightVariation, -m_heightVariation, m_heightVariation);
        m_currentCharacterAttributes.HeightVariation = height;
        ApplyCharacterAttributes();
    }

    void ApplyCharacterAttributes()
    {
        ModelViewData curModel = m_models[m_currentModel];
        Vector3 curScale = curModel.transform.localScale;
        float width = m_startingWidth + m_currentCharacterAttributes.WidthVariation;
        float height = m_startingHeight + m_currentCharacterAttributes.HeightVariation;
        curModel.HairMaterial.color = m_currentCharacterAttributes.HairColor;
        curModel.GlassesMaterial.color = m_currentCharacterAttributes.GlassesColor;
        curModel.transform.localScale = new Vector3(width, height, curScale.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SwipeModel(SwipeDirection.LEFT);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SwipeModel(SwipeDirection.RIGHT);

        if (m_swiping)
        {
            m_time += Time.deltaTime * m_swipeSpeed;
            CanvasTransitioner transitioner = CanvasTransitioner.Instance;
            for (int i = 0; i < m_models.Length; i++)
            {
                TargetPoint target = m_targetPoints[i];
                Vector3 position = transitioner.GetInterpolatedPosition(target.Start, target.Target, m_time, m_interpolationType);
                m_models[i].transform.localPosition = position;
            }

            if (m_time >= 1.0f)
            {
                m_swiping = false;
            }
        }
    }
}
