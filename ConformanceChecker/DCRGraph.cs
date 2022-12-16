using DcrConformanceChecker.Parsers.DcrParser;

namespace DcrConformanceChecker.ConformanceChecker;

public class DCRGraph
{
    // All activities in the graph
    public HashSet<Activity> Activities;

    public DCRGraph()
    {
        Activities = new HashSet<Activity>();
    }

    /// <summary>
    /// Get all activities that are pending
    /// </summary>
    public List<Activity> GetPendingActivities()
    {
        return Activities.Where(a => a.Pending).ToList();
    }

    /// <summary>
    /// Checks if the activity with the given label is exists
    /// </summary>
    public bool HasActivity(string label)
    {
        return Activities.Any(a => a.Label == label);
    }

    /// <summary>
    /// Returns the activity with the given label. Throws an exception if the activity does not exist
    /// </summary>
    public Activity GetActivity(string label)
    {
        return Activities.Where(a => a.Label == label).First();
    }

    /// <summary>
    /// Adds Activity to DCR graph. <br/>
    /// If the activity already exists, it will update the marking.
    /// </summary>
    public void AddActivity(string label, bool executed = false, bool included = true, bool pending = false)
    {
        if (!HasActivity(label))
        {
            var activity = new Activity(label, executed, included, pending);
            Activities.Add(activity);
        }
        else
        {
            var activity = GetActivity(label);
            activity.Executed = executed;
            activity.Included = included;
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

    /// <summary>
    /// Returns true if the graph is accepting, false otherwise
    /// </summary>
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