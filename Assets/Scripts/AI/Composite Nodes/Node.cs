using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree
{
    [System.Serializable]
    public abstract class Node
    {
        protected NodeState nodeState;

        public abstract NodeState Evaluate();
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE,
        NUM_OF_NODESTATES
    }
}