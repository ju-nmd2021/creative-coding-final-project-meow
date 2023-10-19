using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public delegate void FuncDelegate();
public delegate bool BoolDelegate();
public class GameDirector : MonoBehaviour
{
    public int levelsCompleted = 0;
    private int moodInt = 70;
    private int prevMood = 70;
    List<FuncDelegate> accidents = new List<FuncDelegate>();
    List<FuncDelegate> opportunities = new List<FuncDelegate>();
    public List<BoolDelegate> randomStartEvents = new List<BoolDelegate>();
    public Image darkBox;
    public float transitionDuration = 2.0f;
    public Color startColor = new(1.0f, 0.0f, 0.0f, 0.0f);
    public Color targetColor = new(1.0f, 0.0f, 0.0f, 0.5f);
    public StatsPlayer playerStats;
    public GetWeather getWeather;
    public TextMeshProUGUI textField;
    public SwordAttack swordAttack;
    public MovementController movementController;
    public LevelCreator levelCreator;
    public GameObject Skeleton;
    public AttackController attackController;
    public class WeatherData
    {
        public string description;
    }

    public int Mood
    {
        get { return moodInt; }
        set {
            if (value >= 0 && value <= 100) {
                OnMoodChange(value);
                moodInt = value;
                ColorChange(value);
                print($"mood changed: {value}");
            } else
            {
                print("MOOD WAS NOT CHANGED!!"); 
            }
        }
    }

    public GameObject border;

    // Start is called before the first frame update
    void Start()
    {
        getWeather = new GetWeather();
        // opportunities
        opportunities.Add(AddHp);
        opportunities.Add(AddDamge);
        opportunities.Add(AddSpeed);
        opportunities.Add(SlowEnemies);
        // accidents
        accidents.Add(MakeEnemiesTransparent);
        accidents.Add(DarkenScreen);
        accidents.Add(ReduceHp);
        accidents.Add(IncreaseEnemiesRate);
        accidents.Add(AddEnemiesHp);

        // random events
        randomStartEvents.Add(GetWeatherCondition);
        randomStartEvents.Add(ChangeMoodRandomnly);
        randomStartEvents.Add(PunishClumsyWalk);
        randomStartEvents.Add(PunishInaccurateCombat);
        randomStartEvents.Add(EvaluateSpeed);
    }

    public bool GetWeatherCondition()
    {
        getWeather.GetWeatherConditions(this, (str) => {
            WeatherData json = JsonUtility.FromJson<WeatherData>(str);
            string description = json.description.ToLower();
            int moodChanged = 0;
            if (moodChanged > 0)
            {
                textField.text = "Weather is good today, game director is happy";
            } else
            {
                textField.text = "Weather is bad today, game director is sad";
            }
            if (description.Contains("warm"))
            {
                moodChanged += 10;
            } if (description.Contains("rain"))
            {
                moodChanged -= 30;
            } if (description.Contains("sunny"))
            {
                moodChanged += 15;
            } if (description.Contains("cloudy"))
            {
                moodChanged -= 10;
            }
            if (description.Contains("clear"))
            {
                moodChanged += 5;
            }
            if (description.Contains("fog"))
            {
                moodChanged -= 15;
            }
            if (description.Contains("wind"))
            {
                moodChanged -= 20;
            }
            if (description.Contains("snow"))
            {
                moodChanged -= 10;
            }
            if (description.Contains("thunder"))
            {
                moodChanged -= 30;
            }
            Mood += moodChanged;
        });
        return true;
    }

    public bool EvaluateSpeed()
    {
        if (levelCreator.timeSpent < 30)
        {
            Mood += (int)levelCreator.timeSpent;
        } else
        {
            Mood -= (int)levelCreator.timeSpent / 2;
            textField.text = "You are too slow, the game director does not like it";
        }
        return true;
    }
    public bool PunishInaccurateCombat()
    {
        int attacksMissed = attackController.timesAttacked - swordAttack.timesAttackedSuccessfully;
        if (attacksMissed > 5)
        {
            Mood -= attacksMissed * 4;
            textField.text = "You are too innacurate, the game director does not like it";
        } else
        {
            Mood += 10;
        }
        return true;
    }
    public bool PunishClumsyWalk()
    {
        if (movementController.stumbledTimes >= 5)
        {
            Mood -= movementController.stumbledTimes * 4;
            return true;
        }
        return false;
    }
    private void AddDamge()
    {
        swordAttack.damage += 2;
        textField.text = "Game director is happy, damage is increased";
    }

    public void IncreaseEnemiesRate()
    {
        LevelCreator.enemAmount += 5;
        textField.text = "Enemies rate increased";
    }

    public void AddEnemiesHp()
    {
        Skeleton.GetComponent<StatsController>().Health = 12;
        textField.text = "Enemies got stronger";
    }

    private void AddSpeed()
    {
        movementController.moveSpeed += 1f;
        textField.text = "Your speed increased!";
        print("increase speed");
    }

    public void SlowEnemies()
    {
        Skeleton.GetComponent<AIPath>().maxSpeed = 0.1f;
        print("slow enem");
        textField.text = "Enemies are slower now";
    }

    private void ReduceHp()
    {
        playerStats.Health = 2;
        print("set hp to 2");
    }


    private void AddHp()
    {
        playerStats.Health += 5;
        print("add hp");
        textField.text = "You are rewarded with extra HP";
    }
    public bool ChangeMoodBasedOnMovement()
    {
        if (movementController.stumbledTimes > 1500)
        {
            Mood -= movementController.stumbledTimes / 100;
            return true;
        }
        return false;
    }

    public bool ChangeMoodRandomnly()
    {
        float random = Random.value;
        print("change mood randomly");

        if (random > 0.5f)
        {
            Mood -= 10;
        } else
        {
            Mood += 10;
        }
        return true;
    }

    private void OnMoodChange(int mood)
    {
        int diff = mood - prevMood;
        decimal diffSeverity = (decimal)diff / (decimal)prevMood;
        prevMood = mood;
        List<FuncDelegate> eventList = diffSeverity < 0 ? accidents : opportunities;
        int indx = MapValue(Mathf.Abs((float)diffSeverity + Random.value), 0, 1, 0, eventList.Count - 1);
        print("MOOD CHANGED");
        eventList[indx]();
    }

    public static int MapValue(float value, float valueMin, float valueMax, double desiredMin, double desiredMax)
    {
        // Ensure that the value is within the source range
        value = Mathf.Max(value, valueMin);
        value = Mathf.Min(value, valueMax);

        // Calculate the ratio between the source and destination ranges
        double sourceRange = valueMax - valueMin;
        double destRange = desiredMax - desiredMin;

        // Map the value to the destination range
        double mappedValue = (value - valueMin) * (destRange / sourceRange) + desiredMin;

        return Mathf.RoundToInt((float)mappedValue);
    }

    private void ColorChange(int value)
    {
        foreach (Transform child in border.transform)
        {
            Color red = new(255, 0, 0);
            Color green = new(0, 255, 0);
            float t = Mathf.Clamp01(value / 100f);
            Color lerpedColor = Color.Lerp(red, green, t);
            child.GetComponent<Image>().color = lerpedColor;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Mood -= 10;
            // GetWeatherCondition();
        }


    }

    private void DarkenScreen()
    {
        StartCoroutine(ChangeColorSmoothly());
    }

    public void MakeEnemiesTransparent()
    {
        Skeleton.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.1f);
        textField.text = "Enemies are almost invisible now!";
    }

    public void ResetSettings()
    {
        movementController.moveSpeed = 1f;
        swordAttack.damage = 3;
        Skeleton.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        Skeleton.GetComponent<StatsController>().Health = 8;
        Skeleton.GetComponent<AIPath>().maxSpeed = 0.5f;
        LevelCreator.enemAmount = 5;
    }

    private IEnumerator ChangeColorSmoothly()
    {
        float elapsedTime = 0f;
        darkBox.color = startColor;
        while (elapsedTime < transitionDuration)
        {
            darkBox.color = new(0, 0, 0, (elapsedTime / transitionDuration) * 0.8f);
            print("CHANGE COLOR");
            elapsedTime += Time.deltaTime;
            yield return null;
            StartCoroutine(ResetColor());
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(15);
        darkBox.color = new(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
