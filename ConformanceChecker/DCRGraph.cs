namespace DcrConformanceChecker.ConformanceChecker;

class DCRGraph
{
    public HashSet<Activity> Activities;

    public DCRGraph()
    {
        Activities = new HashSet<Activity>();
    }

    public bool HasActivity(string label)
    {
        return Activities.Any(a => a.label == label);
    }

    public Activity GetActivity(string label)
    {
        return Activities.Where(a => a.label == label).First();
    }

    public void AddActivity(string label, bool included = true, bool executed = false, bool pending = false)
    {
        if (!HasActivity(label))
        {
            var activity = new Activity(label, included, executed, pending);
            Activities.Add(activity);
        }
    }

    // src -->* trg
    public void addCondition(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        trgActivity.conditionIn.Add(srcActivity);
    }

    // src --><> trg
    public void addMilestone(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        trgActivity.milestoneIn.Add(srcActivity);
    }

    // src *--> trg
    public void addResponse(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        srcActivity.responseOut.Add(trgActivity);
    }

    // src -->+ trg
    public void addInclude(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        srcActivity.includeOut.Add(trgActivity);
    }

    // src -->% trg
    public void addExclude(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        srcActivity.excludeOut.Add(trgActivity);
    }

    public bool IsAccepting()
    {
        foreach (Activity activity in Activities)
        {
            if (!activity.IsAccepting())
            {
                return false;
            }
        }

        return true;
    }

}