using System;

namespace Hassium
{
    public class FunctionCallNode: OldAstNode
    {
        public OldAstNode Target
        {
            get
            {
                return this.Children[0];
            }
        }
        public OldAstNode Arguments
        {
            get
            {
                return this.Children[1];
            }
        }

        public FunctionCallNode(OldAstNode target, ArgListNode arguments)
        {
            this.Children.Add(target);
            this.Children.Add(arguments);
        }
    }
}

