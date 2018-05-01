using System;
using StateNS;

namespace MiniChess
{
    class Program
    {
        static void Main(string[] args)
        {

            char[,] board = initializeBoard();
            //int cp = 1;
            //int[] lm = { 3, 2, 3, 3 };
            //State inicial = new State(board, cp, lm);
            printBoard(board);
        }
        public static char[,] initializeBoard()
        {
            int lin, col, size;
            char[,] board = new char[6, 6];
            size = board.GetLength(0);
            for (lin = 0; lin < size; lin++)
            {
                for (col = 0; col < size; col++)
                {
                    board[lin, col] = '0'; //inicaliza com 0's representado casas vazias
                }
            }
            fillPieces(board);
            return board;
        }

        public static char[,] fillPieces(char[,] board){//prenche as peças nas suas posições iniciais do jogo
            int lin, col;
            int tamanho = board.GetLength(0);
            Types types = new Types();
            lin = 1;
            for(col = 0; col < tamanho; col++){
                board[lin, col] = 'P';
            }
            lin = 4;
            for(col = 0; col < tamanho; col++){
                board[lin, col] = 'p';
            }
            //Jogador 1
            board[0, 0] = board[0, 5] = types.ROOK; // TORRE
            board[0, 1] = board[0, 4] = types.BISHOP; // BISPO
            board[0, 2] = types.QUEEN;
            board[0, 3] = types.KING;
            //Jogador 2
            board[5, 0] = board[5, 5] = player2Piece(types.ROOK);
            board[5, 1] = board[5, 4] = player2Piece(types.BISHOP);
            board[5, 2] = player2Piece(types.KING);
            board[5, 3] = player2Piece(types.QUEEN);
            return board;
        }
        public static void printBoard(char[,] board){
            Console.WriteLine($"Current Player:");//TODO Adicionar jogador atual
            Console.WriteLine("     Board");
            int size = board.GetLength(0);
            Console.Write($"  1 2 3 4 5 6\n");
            for(int lin=0; lin<size; lin++){
                Console.Write($"{lin+1} ");
                for(int col=0; col<size; col++){
                    Console.Write($"{board[lin,col]} ");
                }
                Console.Write($"{lin+1} ");
                Console.Write("\n");
            }
            Console.Write($"  1 2 3 4 5 6\n");
        }

        public static char player2Piece(char piece){
            return Char.ToLower(piece);
        }
    }
}
