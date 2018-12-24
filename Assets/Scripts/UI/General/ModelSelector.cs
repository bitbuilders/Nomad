using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Transform[] m_rowLocations = null;
    [SerializeField] [Range(0.0f, 30.0f)] float m_modelSpacing = 15.0f;
    [SerializeField] [Range(0.0f, 30.0f)] float m_rowSpacing = 10.0f;
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
    [SerializeField] bool m_randomStart = true;
    [Space(15)]
    [Header("Icons")]
    [SerializeField] Image m_genderImage = null;
    [SerializeField] Sprite m_maleIcon = null;
    [SerializeField] Color m_maleColor = Color.cyan;
    [SerializeField] Sprite m_femaleIcon = null;
    [SerializeField] Color m_femaleColor = Color.red;

    public CharacterAttributes CurrentCharacterAttributes;

    public int m_currentModel = 0;
    public int m_currentRow = 0;
    
    List<ModelViewData>[] m_playerModels = null;
    List<TargetPoint>[] m_targetPoints;
    TargetPoint[] m_rowTargetPoints;
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
        if (m_randomStart && LocalPlayerData.Instance.FirstLoadIn)
        {
            m_currentRow = Random.Range(0, 2);
            m_currentModel = Random.Range(0, m_playerModels[m_currentRow].Count);
        }

        InitializeCharacterAttributes();
        UpdateNetworkModel();
        SetTargetPositions(true);
        SetCallbacks();
        SetModelData();
        SetGenderIcon();
        m_rotation = Vector3.up * 180.0f;
        m_playerModels[m_currentRow][m_currentModel].transform.localEulerAngles = m_rotation;
    }

    void InitializeCharacterAttributes()
    {
        if (!LocalPlayerData.Instance.FirstLoadIn)
        {
            LocalPlayerData.ModelData modelData = LocalPlayerData.Instance.Attributes.MData;
            m_currentModel = modelData.ModelNumber;
            m_currentRow = modelData.RowNumber;
        }
        ModelViewData curModel = m_playerModels[m_currentRow][m_currentModel];
        m_startingWeight = curModel.transform.localScale.x;
        m_startingHeight = curModel.transform.localScale.y;
        CurrentCharacterAttributes = new CharacterAttributes()
        {
            HairColor = curModel.HairMaterial.color,
            GlassesColor = curModel.GlassesMaterial.color,
            WeightVariation = 0.0f,
            HeightVariation = 0.0f
        };
    }

    void SetModelData()
    {
        if (!LocalPlayerData.Instance.FirstLoadIn)
        {
            LocalPlayerData.ModelData modelData = LocalPlayerData.Instance.Attributes.MData;
            m_hairColorSelector.m_hueSelector.m_slider.value = modelData.HairHue;
            m_hairColorSelector.m_valueSelector.SetPoint(modelData.HairValue);
            m_glassesColorSelector.m_hueSelector.m_slider.value = modelData.GlassesHue;
            m_glassesColorSelector.m_valueSelector.SetPoint(modelData.GlassesValue);
            m_weightSelector.m_slider.value = modelData.WeightValue;
            m_heightSelector.m_slider.value = modelData.HeightValue;
        }
    }

    void SetGenderIcon()
    {
        Sprite s = null;
        Color c = Color.white;

        if (m_currentRow == 0)
        {
            s = m_maleIcon;
            c = m_maleColor;
        }
        else
        {
            s = m_femaleIcon;
            c = m_femaleColor;
        }

        m_genderImage.sprite = s;
        m_genderImage.color = c;
    }

    public LocalPlayerData.ModelData GetModelData()
    {
        return new LocalPlayerData.ModelData()
        {
            ModelNumber = m_currentModel,
            RowNumber = m_currentRow,
            HairHue = m_hairColorSelector.m_hueSelector.m_slider.value,
            HairValue = m_hairColorSelector.m_valueSelector.m_point,
            GlassesHue = m_glassesColorSelector.m_hueSelector.m_slider.value,
            GlassesValue = m_glassesColorSelector.m_valueSelector.m_point,
            WeightValue = m_weightSelector.m_slider.value,
            HeightValue = m_heightSelector.m_slider.value,
        };
    }

    void CreateModels()
    {
        m_playerModels = new List<ModelViewData>[2];
        m_targetPoints = new List<TargetPoint>[m_playerModels.Length];
        m_rowTargetPoints = new TargetPoint[m_playerModels.Length];
        for (int i = 0; i < m_playerModels.Length; i++)
        {
            if (i == 0)
            {
                m_playerModels[i] = new List<ModelViewData>();
                m_targetPoints[i] = new List<TargetPoint>();
                foreach (GameObject g in m_maleModelTemplates)
                {
                    GameObject go = Instantiate(g, m_rowLocations[i]);
                    ModelViewData data = go.GetComponent<ModelViewData>();
                    m_playerModels[i].Add(data);
                    m_targetPoints[i].Add(new TargetPoint());
                }
            }
            else if (i == 1)
            {
                m_playerModels[i] = new List<ModelViewData>();
                m_targetPoints[i] = new List<TargetPoint>();
                foreach (GameObject g in m_femaleModelTemplates)
                {
                    GameObject go = Instantiate(g, m_rowLocations[i]);
                    ModelViewData data = go.GetComponent<ModelViewData>();
                    m_playerModels[i].Add(data);
                    m_targetPoints[i].Add(new TargetPoint());
                }
            }
            m_rowTargetPoints[i] = new TargetPoint();
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

    void UpdateNetworkModel()
    {
        int modelsBefore = 0;
        for (int i = 0; i < m_currentRow; i++)
        {
            modelsBefore += m_playerModels[i].Count;
        }
        NomadNetworkManager.m_currentModel = m_currentModel + modelsBefore;
    }

    void SetTargetPositions(bool setModelPositions)
    {
        for (int i = 0; i < m_targetPoints.Length; i++)
        {
            int indexFromCurrentRow = i - m_currentRow;
            float rowOffset = m_rowSpacing * indexFromCurrentRow;

            for (int j = 0; j < m_targetPoints[i].Count; j++)
            {
                TargetPoint targetPoint = m_targetPoints[i][j];
                int indexFromCurrent = j - m_currentModel;
                float offset = m_modelSpacing * indexFromCurrent;
                Vector3 target = new Vector3(offset, -4.0f, 0.0f);
                targetPoint.Start = targetPoint.Target;
                targetPoint.Target = target;
                m_targetPoints[i][j] = targetPoint;
                if (setModelPositions)
                    m_playerModels[i][j].transform.localPosition = target;
            }
            
            Vector3 rowTarget = new Vector3(0.0f, rowOffset, 0.0f);
            m_rowTargetPoints[i].Start = m_rowTargetPoints[i].Target;
            m_rowTargetPoints[i].Target = rowTarget;
            if (setModelPositions)
                m_rowLocations[i].transform.localPosition = rowTarget;
        }
    }

    public void SwipeModel(SwipeDirection direction)
    {
        bool changedRow = false;
        switch (direction)
        {
            case SwipeDirection.LEFT:
                m_currentModel--;
                break;
            case SwipeDirection.RIGHT:
                m_currentModel++;
                break;
            case SwipeDirection.UP:
                m_currentRow++;
                changedRow = true;
                break;
            case SwipeDirection.DOWN:
                m_currentRow--;
                changedRow = true;
                break;
        }

        if (m_currentRow < 0)
            m_currentRow = m_playerModels.Length - 1;
        else if (m_currentRow > m_playerModels.Length - 1)
            m_currentRow = 0;
        if (changedRow && m_currentModel > m_playerModels[m_currentRow].Count - 1)
            m_currentModel = m_playerModels[m_currentRow].Count - 1;
        else if (m_currentModel < 0)
            m_currentModel = m_playerModels[m_currentRow].Count - 1;
        else if (m_currentModel > m_playerModels[m_currentRow].Count - 1)
            m_currentModel = 0;

        UpdateNetworkModel();
        
        SetTargetPositions(false);
        m_swiping = true;
        m_time = 0.0f;

        ApplyCharacterAttributes();
        SetGenderIcon();
    }

    public void SwipeRight()
    {
        SwipeModel(SwipeDirection.RIGHT);
    }

    public void SwipeLeft()
    {
        SwipeModel(SwipeDirection.LEFT);
    }

    public void SwipeUp()
    {
        SwipeModel(SwipeDirection.UP);
    }

    public void SwipeDown()
    {
        SwipeModel(SwipeDirection.DOWN);
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
            for (int i = 0; i < m_targetPoints.Length; i++)
            {
                for (int j = 0; j < m_targetPoints[i].Count; j++)
                {
                    TargetPoint target = m_targetPoints[i][j];
                    Vector3 position = transitioner.GetInterpolatedPosition(target.Start, target.Target, m_time, m_interpolationType);
                    m_playerModels[i][j].transform.localPosition = position;
                    if (m_time >= 1.0f)
                    {
                        m_playerModels[i][j].transform.localPosition = target.Target;
                    }
                }

                TargetPoint rowTarget = m_rowTargetPoints[i];
                Vector3 rowPosition = transitioner.GetInterpolatedPosition(rowTarget.Start, rowTarget.Target, m_time, m_interpolationType);
                m_rowLocations[i].transform.localPosition = rowPosition;
                if (m_time >= 1.0f)
                {
                    m_rowLocations[i].transform.localPosition = rowTarget.Target;
                }
            }

            if (m_time >= 1.0f)
            {
                m_swiping = false;
            }
        }
    }
}
