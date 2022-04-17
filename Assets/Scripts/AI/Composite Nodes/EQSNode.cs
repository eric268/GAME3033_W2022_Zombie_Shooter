using AIBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EQSNode<T> : Node, IEQSNode<T>
{
    public EQSNodeType mEQSNodeType;
    public abstract override NodeState Evaluate();
    public abstract T EvaluateQuery();
}

public class EQSContainer<T>
{
    public EQSContainer(T _key, float _score)
    {
        Key = _key;
        Score = _score;
    }

    public T Key;
    public float Score;
}



