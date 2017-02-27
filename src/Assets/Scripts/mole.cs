using System.Collections;
using UnityEngine;

public enum MoleState
{
    None,
    Open,
    Idle,
    Close,
    Catch
}

public class mole : MonoBehaviour
{
    public MoleState MS;

    public Texture[] g_OpenImages;
    public Texture[] g_IdleImages;
    public Texture[] g_CloseImages;
    public Texture[] g_CatchImages;

    public Texture[] g_OpenImages2;
    public Texture[] g_IdleImages2;
    public Texture[] g_CloseImages2;
    public Texture[] g_CatchImages2;

    public bool GoodMole;
    public int PerGood = 15;

    public AudioClip g_OpenSound;
    public AudioClip g_CatchSound;

    public float g_AniSpeed = 0;
    public float g_NowAniTime = 0;

    private int g_AniCount;

    public GameManager GM;

    public float g_MaxLimitTime = 0.0f;

    // Use this for initialization
    private void Start()
    {
        g_MaxLimitTime = GM.m_nLimitTime;

        if (GM.GS == GameState.Ready)
            GM.Go();

        //MS = MoleState.Open;
        //if (GM.GS == GameState.Ready)
        //{
        //    GetComponent<AudioSource>().clip = g_OpenSound;
        //    GetComponent<AudioSource>().Play();
        //}
        StartCoroutine("WaitForStart");
    }

    public IEnumerator WaitForStart()
    {
        g_AniCount = 0;
        float waittime = Random.Range(0.5f, g_MaxLimitTime / 4);
        yield return new WaitForSeconds(waittime);
        Open_on();
    }

    public IEnumerator Wait()
    {
        MS = MoleState.None;
        g_AniCount = 0;
        float waittime = Random.Range(0.5f, 4.5f);
        yield return new WaitForSeconds(waittime);
        Open_on();
    }

    public void OnMouseDown()
    {
        if (MS == MoleState.Idle || MS == MoleState.Open)
        {
            Catch_on();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (g_NowAniTime >= g_AniSpeed)
        {
            switch (MS)
            {
                case MoleState.Open:
                    Open_Ing();
                    break;

                case MoleState.Idle:
                    Idle_Ing();
                    break;

                case MoleState.Catch:
                    Catch_Ing();
                    break;

                case MoleState.Close:
                    Close_Ing();
                    break;

                default:
                    break;
            }
            g_NowAniTime = 0;
        }
        else
        {
            g_NowAniTime += Time.deltaTime;
        }
    }

    public void Open_on()
    {
        MS = MoleState.Open;
        g_AniCount = 0;
        g_AniSpeed = (float)(GM.m_nLimitTime / g_MaxLimitTime * 0.05);

        int a = Random.Range(0, 100);
        if (a <= PerGood)
        {
            GoodMole = true;
        }
        else
        {
            GoodMole = false;
        }
    }

    public void Open_Ing()
    {
        if (GoodMole)
            GetComponent<Renderer>().material.mainTexture = g_OpenImages2[g_AniCount];
        else
            GetComponent<Renderer>().material.mainTexture = g_OpenImages[g_AniCount];
        g_AniCount++;
        if (g_AniCount >= g_OpenImages.Length)
        {
            Idle_on();
            // End Open
        }
    }

    public void Idle_on()
    {
        MS = MoleState.Idle;
        g_AniCount = 0;
    }

    public void Idle_Ing()
    {
        if (GoodMole)
            GetComponent<Renderer>().material.mainTexture = g_IdleImages2[g_AniCount];
        else
            GetComponent<Renderer>().material.mainTexture = g_IdleImages[g_AniCount];
        g_AniCount++;
        if (g_AniCount >= g_IdleImages.Length)
        {
            // End idle
            //g_AniCount = 0;
            Close_on();
        }
    }

    public void Close_on()
    {
        MS = MoleState.Close;

        g_AniCount = 0;
    }

    public void Close_Ing()
    {
        if (GoodMole)
            GetComponent<Renderer>().material.mainTexture = g_CloseImages2[g_AniCount];
        else
            GetComponent<Renderer>().material.mainTexture = g_CloseImages[g_AniCount];
        g_AniCount++;
        if (g_AniCount >= g_CloseImages.Length)
        {
            // End Close
            StartCoroutine("Wait");
            MS = MoleState.None;
            g_AniCount = 0;
        }
    }

    public void Catch_on()
    {
        MS = MoleState.Catch;
        g_AniCount = 0;

        GetComponent<AudioSource>().clip = g_CatchSound;
        GetComponent<AudioSource>().Play();

        if (!GoodMole)
        {
            GM.m_nCountBad++;
            GM.AddCombo();
        }
        else
        {
            GM.m_nCountGood++;
            GM.EndCombo();
        }
    }

    public void Catch_Ing()
    {
        if (GoodMole)
            GetComponent<Renderer>().material.mainTexture = g_CatchImages2[g_AniCount];
        else
            GetComponent<Renderer>().material.mainTexture = g_CatchImages[g_AniCount];
        g_AniCount++;
        if (g_AniCount >= g_CatchImages.Length)
        {
            StartCoroutine("Wait");
            //Close_on();
            MS = MoleState.None;
            g_AniCount = 0;
            // End Catch
        }
    }
}