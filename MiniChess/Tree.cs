using System;
using StateNS;
using System.Collections.Generic;

namespace TreeNS
{
    class Tree
    {
        State state;
        int minimax; //0->min / 1->max
        LinkedList<Tree> children;

        public Tree(State state, int minimax){
            this.state = state;
            this.minimax = minimax;
            this.children = null;
        }

    }
}
