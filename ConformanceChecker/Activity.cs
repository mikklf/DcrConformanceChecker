namespace DcrConformanceChecker.ConformanceChecker;

class Activity
{
    // Data
    public string label;

    // Marking
    public bool included;
    public bool executed;
    public bool pending;

    // Relations
    public HashSet<Activity> conditionIn;
    public HashSet<Activity> milestoneIn;
    public HashSet<Activity> responseOut;
    public HashSet<Activity> excludeOut;
    public HashSet<Activity> includeOut;

    public Activity(string label, bool included = true, bool executed = false, bool pending = false)
    {
        this.label = label;
        this.included = included;
        this.executed = executed;
        this.pending = pending;
        conditionIn = new HashSet<Activity>();
        milestoneIn = new HashSet<Activity>();
        responseOut = new HashSet<Activity>();
        excludeOut = new HashSet<Activity>();
        includeOut = new HashSet<Activity>();
    }

    public bool isEnabled()
    {
        // Check if activity is included
        if (!included)
        {
            return false;
        }

        // Check if all included conditions have been executed
        foreach (Activity activity in conditionIn)
        {
            if (activity.included && !activity.executed)
                Console.WriteLine(activity.label);
            return false;
        }

        // Check if all included milestone has a pending response
        foreach (Activity activity in milestoneIn)
        {
            if (activity.included && activity.pending)
                return false;
        }

        return true;
    }

    public void Execute()
    {
        // Check if the event is enabled
        if (!isEnabled())
            return;

        // Update marking
        executed = true;
        pending = false;

        // Set reponses to pending
        foreach (Activity activity in responseOut)
        {
            activity.pending = true;
        }

        // Exclude Activitys
        foreach (Activity activity in excludeOut)
        {
            activity.included = false;
        }

        // Include Activitys
        foreach (Activity activity in includeOut)
        {
            activity.included = true;
        }

    }

    public bool IsAccepting()
    {
        return !included || !pending;
    }

    public override string ToString()
    {
        return $"name: {label}, included: {included}, executed: {executed}, pending: {pending}";
    }

    public override int GetHashCode()
    {
        return label.GetHashCode();
    }

}