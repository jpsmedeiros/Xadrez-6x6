using System;
using StateNS;
using TypesNS;
using RuleMachineNS;
using AINS;

namespace MiniChess
{
    class Program
    {
        private static bool game;
        public static int gameMode = 1; // default = 1: Player vs Player
        public static State currentState;
        private static bool messageHandlerActive = true;
        public static Types types = new Types();

        public static AI ia1 = new AI(2,2);
        public static AI ia2 = new AI(1,2);
    
        
        static void Main(string[] args)
        {
            // configuração inicial do jogo
            initializeGame();
            menuInterface();

            // game loop
            while(game){
                printBoard();                
                //RuleMachine.possible_moves(currentState);
                movementInterface();
                //printBoard();

                Console.WriteLine("Eval: " + evalSimples(currentState.board));

                if (gameIsOver(currentState)){
                    printBoard();
                    Console.WriteLine("GAME OVER, player" + getWinner(currentState) + " ganhou a partida!");
                    game = false;
                };
            }       
        }
        
        public static void initializeGame(){
            char[,] new_board = initializeBoard();
            int initialPlayer = 1;
            currentState = new State(new_board, initialPlayer, null, 0);
            //Console.Clear();
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
            return fillBoardPieces(board);
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
                }else if(gameMode == 2){// P vs IA => chama IA se state.currentPlayer == 2
                    if(currentState.currentPlayer == 1){
                        Console.WriteLine("Movimente sua peça:\n");
                        input = Console.ReadLine();
                    }else{
                        //TODO
                        Console.WriteLine("Esperando input da IA...");
                        //CHAMA A IA
                        input = ia1.play(currentState);
                    }
                }else{// IA vs IA => sempre chama a IA
                    //TODO
                    Console.WriteLine("Esperando input da IA " + currentState.currentPlayer +"...");
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
            if(!RuleMachine.validateMove(coordinates, currentState.board, currentState.currentPlayer)){
                return true;
            }
            //Console.WriteLine(RuleMachine.isCheck(currentState.board, currentState.currentPlayer, coordinates));
            movePiece(coordinates);
            return false;
        }

        public static void movePiece(int[] coordinates){
            char piece = currentState.board[coordinates[0], coordinates[1]];
            if(RuleMachine.isAttackMove(coordinates, currentState.board)){
                capture(coordinates, currentState.board);
            }
            currentState.board[coordinates[0], coordinates[1]] = Types.EMPTY;
            currentState.board[coordinates[2], coordinates[3]] = piece;
            changeCurrentPlayer();
            currentState.playsCount++;
        }

        public static char[,] fillBoardPieces(char[,] board){//prenche as peças nas suas posições iniciais do jogo
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

        public static void printBoard(State currentState){
            Console.WriteLine($"Current Player: {currentState.currentPlayer}");
            Console.WriteLine("     Board");
            int size = currentState.board.GetLength(0);
            Console.Write($"  1 2 3 4 5 6\n");
            for(int lin=0; lin<size; lin++){
                Console.Write($"{lin+1} ");
                for(int col=0; col<size; col++){
                    Console.Write($"{currentState.board[lin,col]} ");
                }
                Console.Write($"{lin+1} ");
                Console.Write("\n");
            }
            Console.Write($"  1 2 3 4 5 6\n");
        }

        public static void printBoard(){
            printBoard(currentState);
        }

        public int getCurrentPlayer(){
            return currentState.currentPlayer;
        }
        public static void messageHandler(String msg){
            if(messageHandlerActive){
                //Console.WriteLine(msg);
            }
        }

        public static void activateOrDeactivateMessageHandler(){
            messageHandlerActive = !messageHandlerActive;
        }

        public static void changeCurrentPlayer(){
            currentState.currentPlayer = currentState.currentPlayer == 1 ? 2 : 1;
            return;
        }

        public static void capture(int[] coordinates, char[,] board){
            Program.messageHandler("Peça capturada na Linha: "+coordinates[2]+" Coluna: "+coordinates[3]+" Peça capturada: "+board[coordinates[2], coordinates[3]]);
        }

        public static char player2Piece(char piece){
            return Char.ToLower(piece);
        }

        public static bool gameIsOver(State state){
            int number_of_kings = 0;
            foreach (char piece in state.board)
                if (Types.getPlayer1Piece(piece) == Types.KING) number_of_kings++;
        
            return number_of_kings < 2 || state.checkDraw();
        }

        // retorna -1 se o jogo não tiver ganhador ainda
        public static int getWinner(State state){
            if (gameIsOver(state) && !state.checkDraw()){
                foreach (char piece in state.board){
                    bool is_king = Types.getPlayer1Piece(piece) == Types.KING;
                    int player = Types.getPiecePlayer(piece);
                    if (is_king) return player;
                }    
            }
            
            return -1;
        }
        public static int evalSimples(char[,] atual){
            int p=0, b=0, t=0, q=0;
            //int k=0;
            //char[,] board = new char[6, 6];
            //board = State.board;
            //Types comp = new Types();
            char chP = Char.ToLower(Types.PAWN);
            char chB = Char.ToLower(Types.BISHOP);
            char chR = Char.ToLower(Types.ROOK);
            char chQ = Char.ToLower(Types.QUEEN);
            for(int i=0; i<6; i++){
                for(int j=0; j<6; j++){
                    if(atual[i,j].Equals(chP)){
                        p+=1;
                    }
                    if(atual[i,j].Equals(Types.PAWN)){
                        p-=1;
                    }
                    if(atual[i,j].Equals(chB)){
                        b+=1;
                    }
                    if(atual[i,j].Equals(Types.BISHOP)){
                        b-=1;
                    }
                    if(atual[i,j].Equals(chR)){
                        t+=1;
                    }
                    if(atual[i,j].Equals(Types.ROOK)){
                        t-=1;
                    }
                    if(atual[i,j].Equals(chQ)){
                        q+=1;
                    }
                    if(atual[i,j].Equals(Types.QUEEN)){
                        q-=1;
                    }
                }
            }
            int eval = p + 3*b + 5*t + 9*q;
            return eval;
        }
    }
}
