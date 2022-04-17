
public enum EQSNodeType
{
    Filter,
    Score,
    Filter_And_Score,
    Num_Of_EQSNode_Types
}

internal interface IEQSNode<T>
{
    public T EvaluateQuery();
}