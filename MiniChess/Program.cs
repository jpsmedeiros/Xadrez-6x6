using System;
using StateNS;
using TypesNS;

namespace MiniChess
{
    class Program
    {
        public static int currentPlayer;
        private static bool game;
        static void Main(string[] args)
        {
            char[,] board = initializeGame();
            String input = "";
            do{
                Console.WriteLine("Digite:\n 1- Jogar\n 2- Ajuda\n 3- Creditos\n 4- Sair\n");
                input = Console.ReadLine();
            } while(handleOptionInput(input));

            while(game){
                Console.WriteLine("Movimente sua peça:\n");
                input = Console.ReadLine();
                Console.WriteLine(handleMovementInput(input)[2]);
                input = "";
                game = false;
            }
            //int cp = 1;
            //int[] lm = { 3, 2, 3, 3 };
            //State inicial = new State(board, cp, lm);
            printBoard(board);
        }
        
        public static char[,] initializeGame(){
            Console.Clear();
            currentPlayer = 1;
            game = true;
            return initializeBoard();
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

        public static bool handleOptionInput(String input){
            try{
                int option = Int32.Parse(input);
                switch(option){
                    case 1:
                        return false;
                    case 2:
                        Console.WriteLine("Bem vindo ao Xadrez 6x6\nPara movimentar sua peça digite seu movimento no formato: x1 y1 x2 y2\n"+
                         "(Ex: 2 4 1 2) onde os dois primeiros números são as coordendas iniciais da peça que deseja mover, e os seguintes"+ 
                         "as coordenadas finais.\n\nBom jogo!\n\n");
                        return true;
                    case 3:
                        Console.WriteLine("TODO");
                        return true;
                    case 4:
                        game = false;
                        return false;
                }
            }catch(System.FormatException){
                Console.WriteLine("Opção inválida");
                return true;
            }
            return false;
            
        }
        public static string[] handleMovementInput(String input){
            string[] answer;
            answer = input.Split(" ");
            return answer;
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
            Console.WriteLine($"Current Player: {currentPlayer}");//TODO Adicionar jogador atual
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
        public int getCurrentPlayer(){
            return currentPlayer;
        }
    }
}
