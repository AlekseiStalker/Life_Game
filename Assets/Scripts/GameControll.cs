using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameControll : MonoBehaviour
{
    public GameObject baseButton;
    Transform container; 

    public Color lifeColor;
    public Color deadColor;

    public float timeRate;

    int[,] universe;
    Image[,] buttonImages;

    private int Row = 32;
    private int Col = 50;

    private void OnEnable()
    { 
        universe = new int[Row, Col];
        buttonImages = new Image[Row, Col];
    }

    private void Start()
    {
        container = GameObject.Find("Container").transform;
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                GameObject curBtn = Instantiate(baseButton, container, false);
                curBtn.name = "Button" + i + j;
                int curI = i;
                int curJ = j;
                curBtn.GetComponent<Button>().onClick.AddListener(()=>OnClick(curI, curJ));
                buttonImages[i, j] = curBtn.GetComponent<Image>();
            }
        }
    }

    void OnClick(int curI, int curJ)
    {
        universe[curI, curJ] = universe[curI, curJ] == 0 ? 1 : 0;
        UpdateColor(curI, curJ, universe[curI, curJ] == 1);
    }

    void UpdateColor(int i, int j, bool val)
    {
        buttonImages[i, j].color = val == true ? lifeColor : deadColor;
    }

    void NextGenerationLife()
    {
        int [,]newUniverse = new int[Row, Col];

        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                int sum = 0;
                for (int k = i - 1; k < i+2; k++)
                {
                    int kk = k;
                    if (kk == -1) kk = Row - 1;
                    if (kk == Row) kk = 0;
                    for (int l = j -1; l < j +2; l++)
                    {
                        int ll = l;
                        if (ll == -1) ll = 49;
                        if (ll == Col) ll = 0;
                        if (kk == i && ll == j) continue;

                        sum += universe[kk, ll];
                    }
                }
                if (universe[i,j] == 0)
                {
                    if (sum == 3)
                    {
                        newUniverse[i, j] = 1;
                        UpdateColor(i, j, true);
                    }
                }
                else
                {
                    if (sum == 2 || sum == 3)
                    {
                        newUniverse[i, j] = 1;
                    }
                    else if (sum == 0 || sum == 1 || (sum >= 4 && sum <=8))
                    {
                        newUniverse[i, j] = 0;
                        UpdateColor(i, j, false);
                    }
                }
            }
        }
        universe = newUniverse;
    }
    public void OnStartClick()
    {
        InvokeRepeating("NextGenerationLife", 0, timeRate);
    }
    public void OnPauseClick()
    {
        CancelInvoke("NextGenerationLife");
    }
    public void OnClearGenerate()
    {
        CancelInvoke("NextGenerationLife");
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                if (universe[i,j] == 1)
                {
                    universe[i, j] = 0;
                    UpdateColor(i, j, false);
                }
            }
        }
    }
    public void OnNextGenerateClick()
    {
        NextGenerationLife();
    }
    
    [Range(0,1)]
    public float LifeChance;
    public void OnRandomGenerateClick()
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                int rand = Random.Range(0.0f, 1.0f) > LifeChance ? 0 : 1;
                universe[i, j] = rand;
                UpdateColor(i,j, universe[i, j] == 1);
            }
        }
    }
}
