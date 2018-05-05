using System;
using StateNS;
using TypesNS;
using RuleMachineNS;

namespace MiniChess
{
    class Program
    {
        
        public static int currentPlayer;
        private static bool game;
        public static int gameMode = 1;// default = 1: Player vs Player
        public static char[,] board;
        private static bool messageHandlerActive = true;
        public static Types types = new Types();
        
        State currentState = new State(board,currentPlayer, null);

        static void Main(string[] args)
        {
            // configuração inicial do jogo
            initializeGame();
            board = initializeBoard();
            menuInterface();

            // game loop
            while(game){
                printBoard(board);
                RuleMachine.possible_moves(board, currentPlayer);
                movementInterface();
                printBoard(board);
            
                if (gameIsOver(board)){
                    Console.WriteLine("GAME OVER, player" + getWinner(board) + " ganhou a partida!");
                    game = false;
                };
            }       
        }
        
        public static void initializeGame(){
            Console.Clear();
            currentPlayer = 1;
            game = true;
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
                Console.WriteLine("Digite:\n 1- Jogar\n 2- Opcoes\n 3- Ajuda\n 4- Creditos\n 5- Sair\n");
                input = Console.ReadLine();
            } while(handleOptionInput(input));
        }

        private static void movementInterface(){
            String input;
            do{
                if(gameMode == 1){// P vs P => sem chamada a IA
                    Console.WriteLine("Movimente sua peça:\n");
                    input = Console.ReadLine();
                }else if(gameMode == 2){// P vs IA => chama IA se currentPlayer == 2
                    if(currentPlayer == 1){
                        Console.WriteLine("Movimente sua peça:\n");
                        input = Console.ReadLine();
                    }else{
                        //TODO
                        Console.WriteLine("Esperando input da IA...");
                        //CHAMA A IA
                        //input = callIAAction(...);
                        input = "0 0 0 0";//Só para tirar o erro
                    }
                }else{// IA vs IA => sempre chama a IA
                    //TODO
                    Console.WriteLine("Esperando input da IA " +currentPlayer +"...");
                    //CHAMA A IA
                    //input = callIAAction(...);
                    input = "0 0 0 0";//Só para tirar o erro
                }

            }while(handleMovementInput(input));
        }

        public static void modeInterface(){
            String input;
            do{
                Console.WriteLine("Digite:\n 1- Player vs Player\n 2- Player vs IA\n 3- IA vs IA\n");
                input = Console.ReadLine();
            }while(handleModeInput(input));
        }
        public static bool handleModeInput(string input){
                        try{
                int option = Int32.Parse(input);
                switch(option){
                    case 1:
                    //Player vs Player
                        gameMode = 1;
                        return false;
                    case 2:
                    //Player vs IA
                        gameMode = 2;
                        return false;
                    case 3:
                    //IA vs IA
                        gameMode = 3;
                        return false;
                }
            }catch(System.FormatException){
                Console.WriteLine("Opção inválida");
                return true;
            }
            Console.WriteLine("Opção inválida");
            return true;
        }

        public static bool handleOptionInput(String input){
            try{
                int option = Int32.Parse(input);
                switch(option){
                    case 1:
                        return false;
                    case 2:
                        modeInterface();//modo de jogo
                        return true;
                    case 3:
                        Console.WriteLine("Bem vindo ao Xadrez 6x6\nPara movimentar sua peça digite seu movimento no formato: x1 y1 x2 y2\n"+
                         "(Ex: 2 4 1 2) onde os dois primeiros números são as coordendas iniciais da peça que deseja mover, e os seguintes"+ 
                         "as coordenadas finais.\n\nBom jogo!\n\n");
                        return true;
                    case 4:
                        Console.WriteLine("TODO");
                        return true;
                    case 5:
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
            if(RuleMachine.isAttackMove(coordinates, board)){
                capture(coordinates, board);
            }
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
        public static void messageHandler(String msg){
            if(messageHandlerActive){
                Console.WriteLine(msg);
            }
        }

        public static void activateOrDeactivateMessageHandler(){
            messageHandlerActive = !messageHandlerActive;
        }

        public static void changeCurrentPlayer(){
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            return;
        }

        public static void capture(int[] coordinates, char[,] board){
            Program.messageHandler("Peça capturada na Linha: "+coordinates[2]+" Coluna: "+coordinates[3]+" Peça capturada: "+board[coordinates[2], coordinates[3]]);
        }

        public static char player2Piece(char piece){
            return Char.ToLower(piece);
        }

        public static bool gameIsOver(char [,] board){
            int number_of_kings = 0;
            foreach (char piece in board)
                if (Types.getPlayer1Piece(piece) == Types.KING) number_of_kings++;
        
            return number_of_kings < 2;
        }

        public static int getWinner(char [,] board){
            // só retorna o correto caso o jogo já tenha acabado. (use gameIsOver)
            foreach (char piece in board){
                bool is_king = Types.getPlayer1Piece(piece) == Types.KING;
                int player = Types.getPiecePlayer(piece);
                if (is_king) return player;
            }
            return -1;
        }
    }
}
