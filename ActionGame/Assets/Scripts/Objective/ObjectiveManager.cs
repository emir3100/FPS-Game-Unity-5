using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject ObjectiveContainer;
    public List<Objective> ObjectivesActive;
    public List<Objective> ObjectivesNonActive;
    void Start()
    {
        FetchObjectives(ObjectiveContainer);
    }

    // Update is called once per frame
    void Update()
    {
        if(ObjectivesActive.Count == 0)
        {
            AddObjectiveToActive(GetNextObjective(ObjectivesNonActive));
            ActivateNextObjective(GetNextObjective(ObjectivesNonActive));
        }

        AddObjectiveToNonActive(ObjectivesActive);
    }

    private void FetchObjectives(GameObject ObjectiveContainer)
    {
        foreach (Objective objective in ObjectiveContainer.GetComponents<Objective>())
        {
            if (objective.enabled == true)
                ObjectivesActive.Add(objective);
            else
                ObjectivesNonActive.Add(objective);
        }
    }

    private void ActivateNextObjective(Objective nextObjective)
    {
        nextObjective.enabled = true;
        nextObjective.IsActive = true;
    }

    private Objective GetNextObjective(List<Objective> objectives)
    {
        return objectives.First(obj => obj.enabled == false && obj.IsActive == false);
    }

    private void AddObjectiveToNonActive(List<Objective> objectives)
    {
        ObjectivesNonActive.Add(objectives.First(obj => obj.enabled == false && obj.IsActive == true));
        ObjectivesActive.Remove(ObjectivesActive.First());
    }

    private void AddObjectiveToActive(Objective objective)
    {
        ObjectivesActive.Add(objective);
    }
}
