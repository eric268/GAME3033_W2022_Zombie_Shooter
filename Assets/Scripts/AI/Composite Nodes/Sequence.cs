using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree
{
    public class Sequence : Node
    {
        protected List<Node> childNodes = new List<Node>();

        public Sequence(List<Node> nodes)
        {
            childNodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool isAnyNodeRunning = false;

            foreach (Node node in childNodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        isAnyNodeRunning = true;
                        break;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;
                    default:
                        break;
                }
            }
            nodeState = isAnyNodeRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return nodeState;
        }
    }
}
