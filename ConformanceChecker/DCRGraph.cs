using DcrConformanceChecker.Parsers.DcrParser;

namespace DcrConformanceChecker.ConformanceChecker;

public class DCRGraph
{
    public HashSet<Activity> Activities;

    public DCRGraph()
    {
        Activities = new HashSet<Activity>();
    }

    public bool HasActivity(string label)
    {
        return Activities.Any(a => a.Label == label);
    }

    public Activity GetActivity(string label)
    {
        return Activities.Where(a => a.Label == label).First();
    }

    public void AddActivity(string label, bool executed = false, bool included = true, bool pending = false)
    {
        if (!HasActivity(label))
        {
            var activity = new Activity(label, included, executed, pending);
            Activities.Add(activity);
        }
        else
        {
            var activity = GetActivity(label);
            activity.Included = included;
            activity.Executed = executed;
            activity.Pending = pending;
        }
    }

    /// <summary>
    /// Add a condition relation from src to trg and add the activities if they are not already added <br/>
    /// Notation: src -->* trg
    /// </summary>
    public void AddCondition(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        trgActivity.ConditionIn.Add(srcActivity);
    }

    /// <summary>
    /// Add a milestone relation from src to trg and add the activities if they are not already added <br/>
    /// Notation: src --><> trg
    /// </summary>
    public void AddMilestone(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        trgActivity.MilestoneIn.Add(srcActivity);
    }

    /// <summary>
    /// Add a response relation from src to trg and add the activities if they are not already added <br/>
    /// Notation: src *--> trg
    /// </summary>
    public void AddResponse(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        srcActivity.ResponseOut.Add(trgActivity);
    }

    /// <summary>
    /// Add an include relation from src to trg and add the activities if they are not already added <br/>
    /// Notation: src -->+ trg
    /// </summary>
    public void AddInclude(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        srcActivity.IncludeOut.Add(trgActivity);
    }

    /// <summary>
    /// Add an exclude relation from src to trg and add the activities if they are not already added <br/>
    /// Notation: src -->% trg
    /// </summary>
    public void AddExclude(string src, string trg)
    {
        if (!HasActivity(src))
            AddActivity(src);

        Activity srcActivity = GetActivity(src);

        if (!HasActivity(trg))
            AddActivity(trg);

        Activity trgActivity = GetActivity(trg);

        srcActivity.ExcludeOut.Add(trgActivity);
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