public class ArriveDecision : AIDecision
{
    public override bool MakeDecision()
    {
        return _aIActionData.IsArrived;
    }
}
