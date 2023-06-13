
public class CheckHitDecision : AIDecision
{
    public override bool MakeDecision()
    {
        return _aIActionData.IsHit;
    }
}
