using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GoalSeeker : MonoBehaviour
{

    Goal[] mGoals;
    Action[] mActions;
    public float tickLength = 5f;

    Action ChangeOverTime;


    // Start is called before the first frame update
    void Start()
    {
        mGoals = new Goal[3];
        mGoals[0] = new Goal(GoalList.Eat, 4);
        mGoals[1] = new Goal(GoalList.Sleep, 3);
        mGoals[2] = new Goal(GoalList.Bathroom, 3);

        foreach(Goal goal in mGoals)
        {
            TextMeshProUGUI TextGO = GameObject.FindGameObjectWithTag(goal.GoalName.ToString()).GetComponent<TextMeshProUGUI>();
            goal.textObject = TextGO;
        }

        mActions = new Action[6];

        mActions[0] = new Action("eat some food");
        mActions[0].targetGoals.Add(new Goal(GoalList.Eat, -3f));
        mActions[0].targetGoals.Add(new Goal(GoalList.Sleep, +2f));
        mActions[0].targetGoals.Add(new Goal(GoalList.Bathroom, +3f));

        mActions[1] = new Action("eat a snack");
        mActions[1].targetGoals.Add(new Goal(GoalList.Eat, -2f));
        mActions[1].targetGoals.Add(new Goal(GoalList.Sleep, -1f));
        mActions[1].targetGoals.Add(new Goal(GoalList.Bathroom, +1f));

        mActions[2] = new Action("Sleep in the bed");
        mActions[2].targetGoals.Add(new Goal(GoalList.Eat, +2f));
        mActions[2].targetGoals.Add(new Goal(GoalList.Sleep, -4f));
        mActions[2].targetGoals.Add(new Goal(GoalList.Bathroom, +2f));

        mActions[3] = new Action("Sleep on the couch");
        mActions[3].targetGoals.Add(new Goal(GoalList.Eat, +1f));
        mActions[3].targetGoals.Add(new Goal(GoalList.Sleep, -2f));
        mActions[3].targetGoals.Add(new Goal(GoalList.Bathroom, +1f));

        mActions[4] = new Action("drink a soda can");
        mActions[4].targetGoals.Add(new Goal(GoalList.Eat, -1f));
        mActions[4].targetGoals.Add(new Goal(GoalList.Sleep, -2f));
        mActions[4].targetGoals.Add(new Goal(GoalList.Bathroom, +3f));

        mActions[5] = new Action("visit the bathroom");
        mActions[5].targetGoals.Add(new Goal(GoalList.Eat, +0f));
        mActions[5].targetGoals.Add(new Goal(GoalList.Sleep, +0f));
        mActions[5].targetGoals.Add(new Goal(GoalList.Bathroom, -4f));


        ChangeOverTime = new Action("tick");
        ChangeOverTime.targetGoals.Add(new Goal(GoalList.Eat, +4f));
        ChangeOverTime.targetGoals.Add(new Goal(GoalList.Sleep, +1f));
        ChangeOverTime.targetGoals.Add(new Goal(GoalList.Bathroom, +2f));


        Debug.Log("Starting Clock, one hour will pass every " + tickLength + " seconds");
        Debug.Log("Press E to do something");

        InvokeRepeating("Tick", 0f, tickLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Action BestThingToDo = ChooseBestAction(mActions, mGoals);

            Debug.Log("Best thing to do is " + BestThingToDo.name);

            foreach(Goal goal in mGoals)
            {
                goal.value += BestThingToDo.GetGoalChange(goal);

            }

            PrintGoals();
        }
    }

    public void Tick()
    {
        Debug.Log("Tick Occured");
        foreach(Goal goal in mGoals)
        {
            goal.value += ChangeOverTime.GetGoalChange(goal);

        }
        PrintGoals();
    }


    void PrintGoals()
    {
        string goalString = "";
        foreach(Goal goal in mGoals)
        {
            goal.textObject.text = goal.value.ToString();
        }

        goalString += "Discontentment: " + CurrentDiscontentment();
        Debug.Log(goalString);
    }

    public Action ChooseBestAction(Action[] actions, Goal[] goals)
    {
        float BestValue = float.PositiveInfinity;
        Action BestAction = null;

        foreach(Action action in actions)
        {
            float thisValue = Discontentment(action, goals);

            if(thisValue < BestValue)
            {
                BestValue = thisValue;
                BestAction = action;
            }
        }

        return BestAction;
    }

    float Discontentment(Action action, Goal[] goals)
    {
        float discontnetment = 0f;

        foreach (Goal goal in goals)
        {
            float newValue = goal.value + action.GetGoalChange(goal);

            discontnetment += goal.GetDiscontentment(newValue);
        }
        return discontnetment;
    }

    float CurrentDiscontentment()
    {
        float total = 0f; 
        foreach(Goal goal in mGoals)
        {
            total += (goal.value * goal.value);
        }
        return total;
    }
}
