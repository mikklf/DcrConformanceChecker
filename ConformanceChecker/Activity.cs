namespace DcrConformanceChecker.ConformanceChecker;

public class Activity
{
    // Data
    public string Label;

    // Marking
    public bool Executed;
    public bool Included;
    public bool Pending;

    // Relations
    public readonly HashSet<Activity> ConditionIn;
    public readonly HashSet<Activity> MilestoneIn;
    public readonly HashSet<Activity> ResponseOut;
    public readonly HashSet<Activity> ExcludeOut;
    public readonly HashSet<Activity> IncludeOut;
    
    public Activity(string label, bool executed = false, bool included = true, bool pending = false)
    {
        this.Label = label;
        this.Executed = executed;
        this.Included = included;
        this.Pending = pending;

        ConditionIn = new();
        MilestoneIn = new();
        ResponseOut = new();
        ExcludeOut = new();
        IncludeOut = new();
    }

    public bool IsEnabled()
    {
        // Check if activity is included
        if (!Included) 
            return false;

        // Check if all included conditions have been executed
        foreach (Activity activity in ConditionIn)
        {
            if (activity.Included && !activity.Executed)
                return false;
        }

        // Check if all included milestone has a pending response
        foreach (Activity activity in MilestoneIn)
        {
            if (activity.Included && activity.Pending)
                return false;
        }

        return true;
    }

    public void Execute()
    {
        // Check if the event is enabled
        if (!IsEnabled())
            return;

        // Update marking
        Executed = true;
        Pending = false;

        // Set reponses to pending
        foreach (Activity activity in ResponseOut)
        {
            activity.Pending = true;
        }

        // Exclude Activitys
        foreach (Activity activity in ExcludeOut)
        {
            activity.Included = false;
        }

        // Include Activitys
        foreach (Activity activity in IncludeOut)
        {
            activity.Included = true;
        }

    }

    public bool IsAccepting()
    {
        return !Included || !Pending;
    }

    public override string ToString()
    {
        return $"name: {Label}, executed: {Executed}, included: {Included}, pending: {Pending}";
    }

    // Activitys are considered equal if they have the same label
    // This ensures that the HashSet<Activity> Activities in DCRGraph.cs works as intended
    public override int GetHashCode()
    {
        return Label.GetHashCode();
    }

}