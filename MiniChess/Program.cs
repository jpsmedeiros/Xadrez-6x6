﻿using System;
using StateNS;
using TypesNS;
using RuleMachineNS;

namespace MiniChess
{
    class Program
    {
        public static int currentPlayer;
        private static bool game;
        public static char[,] board;
        public static Types types = new Types();
        static void Main(string[] args)
        {
            board = initializeGame();
            menuInterface();

            while(game){
                printBoard(board);
                movementInterface();
                printBoard(board);
            }
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

        private static void menuInterface(){
            String input;
            do{
                Console.WriteLine("Digite:\n 1- Jogar\n 2- Ajuda\n 3- Creditos\n 4- Sair\n");
                input = Console.ReadLine();
            } while(handleOptionInput(input));
        }

        private static void movementInterface(){
            String input;
            do{
                Console.WriteLine("Movimente sua peça:\n");
                input = Console.ReadLine();
            }while(handleMovementInput(input));
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
        public static bool handleMovementInput(String input){
            //TODO Verificar se formato da string está incorreto
            string[] answer;
            answer = input.Split(" ");
            if(answer.Length < 4){
                Console.WriteLine("Movimento em formato incorreto, tente novamente no formato x1 y1 x2 y2\n");
                return true;
            }
            int[] coordinates = new int[4];
            try{
                for(int i=0;i<answer.Length;i++){
                        coordinates[i] = Int32.Parse(answer[i])-1;
                }
            }catch(System.FormatException){
                Console.WriteLine("Movimento em formato incorreto, tente digitar apenas números separados por espaços\n");
            }
            if(!RuleMachine.validateMove(coordinates, board, currentPlayer)){
                return true; //HOMENS TRABALHANDO CUIDADO   
            }
            movePiece(coordinates);
            return false;
        }

        public static void movePiece(int[] coordinates){
            char piece = board[coordinates[0], coordinates[1]];
            board[coordinates[0], coordinates[1]] = Types.EMPTY;
            board[coordinates[2], coordinates[3]] = piece;
            changeCurrentPlayer();
        }

        public static char[,] fillPieces(char[,] board){//prenche as peças nas suas posições iniciais do jogo
            int lin, col;
            int tamanho = board.GetLength(0);
            lin = 1;
            for(col = 0; col < tamanho; col++){
                board[lin, col] = Types.PAWN;
            }
            lin = 4;
            for(col = 0; col < tamanho; col++){
                board[lin, col] = Types.getPlayer2Piece(Types.PAWN);
            }
            //Jogador 1
            board[0, 0] = board[0, 5] = Types.ROOK; // TORRE
            board[0, 1] = board[0, 4] = Types.BISHOP; // BISPO
            board[0, 2] = Types.QUEEN;
            board[0, 3] = Types.KING;
            //Jogador 2
            board[5, 0] = board[5, 5] = Types.getPlayer2Piece(Types.ROOK);
            board[5, 1] = board[5, 4] = Types.getPlayer2Piece(Types.BISHOP);
            board[5, 2] = Types.getPlayer2Piece(Types.KING);
            board[5, 3] = Types.getPlayer2Piece(Types.QUEEN);
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
        public int getCurrentPlayer(){
            return currentPlayer;
        }
        public static void errorHandler(String msg){
            //TODO implementar exibição de erro
            Console.WriteLine(msg);
        }

        public static void changeCurrentPlayer(){
            if(currentPlayer == 1){
                currentPlayer = 2;
                return;
            }   
            currentPlayer = 1;
        }
    }
}
