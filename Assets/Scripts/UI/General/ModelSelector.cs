using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSelector : Singleton<ModelSelector>
{
    public struct CharacterAttributes
    {
        public Color HairColor;
        public Color GlassesColor;
        public float WeightVariation;
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
        RIGHT,
        UP,
        DOWN
    }

    [Header("Input")]
    [SerializeField] GameObject[] m_femaleModelTemplates = null;
    [SerializeField] GameObject[] m_maleModelTemplates = null;
    [SerializeField] Transform m_maleLocation = null;
    [SerializeField] Transform m_femaleLocation = null;
    [SerializeField] [Range(0.0f, 30.0f)] float m_modelSpacing = 15.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_swipeSpeed = 1.0f;
    [SerializeField] CanvasTransitioner.InterpolationType m_interpolationType = CanvasTransitioner.InterpolationType.EXPO_OUT;
    [SerializeField] ColorPicker m_hairColorSelector = null;
    [SerializeField] ColorPicker m_glassesColorSelector = null;
    [SerializeField] FloatPicker m_weightSelector = null;
    [SerializeField] FloatPicker m_heightSelector = null;
    [Space(15)]
    [Header("Values")]
    [SerializeField] [Range(0.0f, 5.0f)] float m_weightVariation = 0.25f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_heightVariation = 0.5f;

    public CharacterAttributes CurrentCharacterAttributes;

    public int m_currentModel = 0;
    public int m_currentRow = 0;
    
    List<ModelViewData>[] m_playerModels = null;
    List<TargetPoint>[] m_targetPoints;
    Vector3 m_rotation;
    Vector3 m_lastMousePosition;
    float m_startingWeight;
    float m_startingHeight;
    float m_time = 0.0f;
    bool m_swiping = false;
    bool m_viewingMaleModels;

    private void Start()
    {
        CreateModels();
        InitializeCharacterAttributes();
        SetTargetPositions(true);
        SetCallbacks();
        m_rotation = Vector3.up * 180.0f;
        m_playerModels[m_currentRow][m_currentModel].transform.localEulerAngles = m_rotation;
    }

    void InitializeCharacterAttributes()
    {
        ModelViewData curModel = m_playerModels[m_currentRow][m_currentModel];
        m_startingWeight = curModel.transform.localScale.x;
        m_startingHeight = curModel.transform.localScale.y;

        CurrentCharacterAttributes = new CharacterAttributes() {
            HairColor = curModel.HairMaterial.color, GlassesColor = curModel.GlassesMaterial.color,
            WeightVariation = 0.0f, HeightVariation = 0.0f };
    }

    void CreateModels()
    {
        m_playerModels = new List<ModelViewData>[2];
        m_targetPoints = new List<TargetPoint>[m_playerModels.Length];
        for (int i = 0; i < m_playerModels.Length; i++)
        {
            if (i == 0)
            {
                m_playerModels[i] = new List<ModelViewData>();
                m_targetPoints[i] = new List<TargetPoint>();
                foreach (GameObject g in m_maleModelTemplates)
                {
                    GameObject go = Instantiate(g, m_maleLocation);
                    ModelViewData data = go.GetComponent<ModelViewData>();
                    m_playerModels[i].Add(data);
                }
            }
            else if (i == 1)
            {
                m_playerModels[i] = new List<ModelViewData>();
                m_targetPoints[i] = new List<TargetPoint>();
                foreach (GameObject g in m_femaleModelTemplates)
                {
                    GameObject go = Instantiate(g, m_femaleLocation);
                    ModelViewData data = go.GetComponent<ModelViewData>();
                    m_playerModels[i].Add(data);
                }
            }
        }
    }

    void SetCallbacks()
    {
        m_hairColorSelector.OnValueChange += SetCharacterHairColor;
        m_glassesColorSelector.OnValueChange += SetCharacterGlassesColor;
        m_weightSelector.OnValueChange += SetCharacterWeightVariation;
        m_heightSelector.OnValueChange += SetCharacterHeightVariation;
        m_hairColorSelector.Initialize(CurrentCharacterAttributes.HairColor);
        m_glassesColorSelector.Initialize(CurrentCharacterAttributes.GlassesColor);
        m_weightSelector.Initialize(m_weightVariation);
        m_heightSelector.Initialize(m_heightVariation);
    }

    void SetTargetPositions(bool setModelPositions)
    {
        //for (int i = 0; i < m_models.Length; i++)
        //{
        //    int indexFromCurrent = i - m_currentModel;
        //    float offset = m_modelSpacing * indexFromCurrent;
        //    Vector3 target = new Vector3(offset, m_models[i].transform.position.y, 0.0f);
        //    m_targetPoints[i].Start = m_targetPoints[i].Target;
        //    m_targetPoints[i].Target = target;
        //    if (setModelPositions)
        //        m_models[i].transform.localPosition = target;
        //}

        for (int i = 0; i < m_targetPoints.Length; i++)
        {
            for (int j = 0; j < m_targetPoints[i].Count; j++)
            {
                TargetPoint targetPoint = m_targetPoints[i][j];
                int indexFromCurrent = i - m_currentModel;
                float offset = m_modelSpacing * indexFromCurrent;
                Vector3 target = new Vector3(offset, m_playerModels[i][j].transform.position.y, 0.0f);
                targetPoint.Start = targetPoint.Target;
                targetPoint.Target = target;
                if (setModelPositions)
                    m_playerModels[i][j].transform.localPosition = target;
            }
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
            m_currentModel = m_playerModels[m_currentRow].Count - 1;
        else if (m_currentModel > m_playerModels[m_currentRow].Count - 1)
            m_currentModel = 0;

        int modelsBefore = 0;
        for (int i = 0; i < m_currentRow; i++)
        {
            modelsBefore += m_playerModels[i].Count;
        }
        NomadNetworkManager.m_currentModel = m_currentModel + modelsBefore;
        
        SetTargetPositions(false);
        m_swiping = true;
        m_time = 0.0f;

        ApplyCharacterAttributes();
    }

    public void SwipeRight()
    {
        SwipeModel(SwipeDirection.RIGHT);
    }

    public void SwipeLeft()
    {
        SwipeModel(SwipeDirection.LEFT);
    }

    public void SetCharacterHairColor(Color color)
    {
        CurrentCharacterAttributes.HairColor = color;
        ApplyCharacterAttributes();
    }

    public void SetCharacterGlassesColor(Color color)
    {
        CurrentCharacterAttributes.GlassesColor = color;
        ApplyCharacterAttributes();
    }

    public void SetCharacterWeightVariation(float weightVariation)
    {
        float weight = Mathf.Clamp(weightVariation, -m_weightVariation, m_weightVariation);
        CurrentCharacterAttributes.WeightVariation = weight;
        ApplyCharacterAttributes();
    }

    public void SetCharacterHeightVariation(float heightVariation)
    {
        float height = Mathf.Clamp(heightVariation, -m_heightVariation, m_heightVariation);
        CurrentCharacterAttributes.HeightVariation = height;
        ApplyCharacterAttributes();
    }

    void ApplyCharacterAttributes()
    {
        ModelViewData curModel = m_playerModels[m_currentRow][m_currentModel];
        Vector3 curScale = curModel.transform.localScale;
        float weight = m_startingWeight + CurrentCharacterAttributes.WeightVariation;
        float height = m_startingHeight + CurrentCharacterAttributes.HeightVariation;
        curModel.HairMaterial.color = CurrentCharacterAttributes.HairColor;
        curModel.GlassesMaterial.color = CurrentCharacterAttributes.GlassesColor;
        curModel.transform.localScale = new Vector3(weight, height, weight);
        m_playerModels[m_currentRow][m_currentModel].transform.localEulerAngles = m_rotation;
    }

    public void SetCharacterRotation()
    {
        if (Input.GetMouseButtonDown(0))
            m_lastMousePosition = Input.mousePosition;

        Vector3 delta = Input.mousePosition - m_lastMousePosition;
        float x = delta.x * Time.deltaTime * 20.0f;
        m_rotation += Vector3.up * x;
        m_playerModels[m_currentRow][m_currentModel].transform.localEulerAngles = m_rotation;

        m_lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if (m_swiping)
        {
            m_time += Time.deltaTime * m_swipeSpeed;
            CanvasTransitioner transitioner = CanvasTransitioner.Instance;
            for (int i = 0; i < m_playerModels.Length; i++)
            {
                for (int j = 0; j < m_targetPoints[i].Count; j++)
                {
                    TargetPoint target = m_targetPoints[i][j];
                    Vector3 position = transitioner.GetInterpolatedPosition(target.Start, target.Target, m_time, m_interpolationType);
                    m_playerModels[i][j].transform.localPosition = position;
                }
            }

            if (m_time >= 1.0f)
            {
                m_swiping = false;
            }
        }
    }
}
