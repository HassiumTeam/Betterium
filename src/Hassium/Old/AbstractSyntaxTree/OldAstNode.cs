using System;
using System.Collections.Generic;

namespace Hassium
{
    public abstract class OldAstNode
    {
        public List<OldAstNode> Children
        {
            get;
            private set;
        }

        public OldAstNode ()
        {
            this.Children = new List<OldAstNode>();
        }
    }
}

