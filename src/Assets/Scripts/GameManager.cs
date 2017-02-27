using System.Collections;
using UnityEngine;

public enum GameState
{
    Ready,
    Play,
    End
}

public class GameManager : MonoBehaviour
{
    public GameState GS;

    public mole[] m_mHoles;
    public float m_nLimitTime;
    public GUIText m_gtTimeText;
    public int m_nCountBad;
    public int m_nCountGood;

    public GameObject m_goFinishGUI;
    public GUIText m_gtFinalCountBad;
    public GUIText m_gtFinalCountGood;
    public GUIText m_gtFinalScore;
    public GUIText m_gtComboScore;
    public AudioClip m_acReadySound;
    public AudioClip m_acGoSound;
    public AudioClip m_acFinishSound;

    public int m_nCombo = 0;
    private float m_nComboDisplayTime = 1;

    private float shake = 0;

    public Transform camTransform;
    private Vector3 originalPosition = Vector3.zero;

    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;

    public void Ready()
    {
        GetComponent<AudioSource>().clip = m_acReadySound;
        GetComponent<AudioSource>().Play();
    }

    public void Go()
    {
        GS = GameState.Play;
        GetComponent<AudioSource>().clip = m_acGoSound;
        GetComponent<AudioSource>().Play();
    }

    // Use this for initialization
    private void Start()
    {
        //GetComponent<AudioSource>().clip = m_acReadySound;
        //GetComponent<AudioSource>().Play();
        m_gtComboScore.text = m_nCombo.ToString() + " 콤보";

        if (camTransform == null)
        {
            camTransform = Camera.main.transform;//GetComponent(typeof(Transform)) as Transform;
        }

        originalPosition = camTransform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GS == GameState.Play)
        {
            m_nLimitTime -= Time.deltaTime;
            if (m_nLimitTime <= 0)
            {
                m_nLimitTime = 0;
                End();
            }
        }

        m_gtTimeText.text = string.Format("{0:N2}", m_nLimitTime);
        m_gtComboScore.text = m_nCombo.ToString() + " 콤보";

        if (m_nComboDisplayTime <= 0)
        {
            Color c = m_gtComboScore.material.color;
            if (c.a > 0)
            {
                c.a -= Time.deltaTime;
                m_gtComboScore.material.color = c;
            }
            else
            {
                if (m_nCombo != 0)
                {
                    m_nCombo = 0;
                    c.a = 1;
                    m_gtComboScore.material.color = c;
                }
            }
        }
        else
        {
            m_nComboDisplayTime -= Time.deltaTime;
        }

        if (shake > 0)
        {
            camTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount * shake;

            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f;
            camTransform.localPosition = originalPosition;
        }
    }

    private void End()
    {
        GS = GameState.End;
        m_gtFinalCountBad.text = m_nCountBad.ToString();
        m_gtFinalCountGood.text = m_nCountGood.ToString();
        m_gtFinalScore.text = (m_nCombo * 100 + m_nCountBad * 100 - m_nCountGood * 1000).ToString();

        m_goFinishGUI.gameObject.SetActive(true);
        GetComponent<AudioSource>().clip = m_acFinishSound;
        GetComponent<AudioSource>().Play();
    }

    public void AddCombo()
    {
        Color c = m_gtComboScore.material.color;
        c.a = 1;
        m_gtComboScore.material.color = c;
        m_nComboDisplayTime = 1;
        m_nCombo++;
    }

    public void EndCombo()
    {
        m_nCombo = 0;
        shake = 0.5f;
    }
}