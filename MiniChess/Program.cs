using System;
using StateNS;

namespace MiniChess
{
    class Program
    {
        static void Main(string[] args)
        {
            int i,j;
            char[,] b = new char[6,6];
            for(i=0; i<6; i++){
                for(j=0; j<6; j++){
                    b[i,j] = '0';
                }
            }
            int cp = 1;
            int[] lm = {3,2,3,3};
            State inicial = new State(b,cp,lm);
            inicial.print();
        }
    }
}
