using System;

namespace StateNS
{
    class State
    {
        public char[,] board;
        public int currentPlayer;
        public int[] lastMove;

        public State(char[,] board, int currentPlayer, int[] lastMove){
            this.board = board;
            this.currentPlayer = currentPlayer;
            this.lastMove = lastMove;
        }

        public void print(){
            Console.WriteLine($"Current Player:{currentPlayer}");
            Console.WriteLine("Board:");
            for(int i=0; i<6; i++){
                for(int j=0; j<6; j++){
                    Console.Write($"{board[i,j]} ");
                }
                Console.Write("\n");
            }
            Console.WriteLine($"LastMove: [{lastMove[0]}][{lastMove[1]}] to [{lastMove[2]}][{lastMove[3]}]");
        }
    }
}
