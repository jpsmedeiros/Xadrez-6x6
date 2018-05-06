using System;
using TypesNS;

namespace StateNS
{
    class State
    {
        public char[,] board;
        public int currentPlayer;
        public int[] lastMove;
        public int playsCount;

        public State(char[,] board, int currentPlayer, int[] lastMove, int playsCount){
            this.board = (char[,])board.Clone();
            this.currentPlayer = currentPlayer;
            this.lastMove = lastMove;
            this.playsCount = playsCount;
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
            if(lastMove != null){
                Console.WriteLine($"LastMove: [{lastMove[0]}][{lastMove[1]}] to [{lastMove[2]}][{lastMove[3]}]");
            }
            //Console.WriteLine($"Plays Count: {playsCount}");
        }

        public static State result(State old, int[] action){
            State newState = new State(old.board,old.currentPlayer,old.lastMove,old.playsCount);
            
            char piece = newState.board[action[0],action[1]];
            newState.board[action[0],action[1]] = Types.EMPTY;     
            newState.board[action[2],action[3]] = piece;
            newState.currentPlayer = newState.currentPlayer == 1 ? 2 : 1;
            newState.lastMove = action;
            newState.playsCount++;
            
            return newState;
        }
    }
}
