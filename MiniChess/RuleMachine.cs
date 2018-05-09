using System;
using System.Collections.Generic;
using TypesNS;
using StateNS;
using MiniChess;
using System.Diagnostics;

namespace RuleMachineNS
{
    class RuleMachine
    {
        public static bool validateMove(int[] coordinates, char[,] board, int currentPlayer, bool isCheckCall = false){
            for(int i=0;i<coordinates.Length;i++){
                int atual = coordinates[i]+1;
                if(atual > 6 || atual < 1){
                    Program.messageHandler("Jogada fora do tabuleiro");
                    return false;
                }
            }
            char piece = board[coordinates[0], coordinates[1]];
            if(Types.isEmpty(piece)){
                Program.messageHandler("Casa vazia, tente selecionar outra casa");
                return false;
            }
            if(!Types.isPlayerX(piece, currentPlayer)){
                Program.messageHandler("Peça não pertence ao jogador atual, tente mover uma outra peça");
                return false;
            }
            char destination = board[coordinates[2], coordinates[3]];
            if(Types.isPlayerX(destination, currentPlayer)){
                Program.messageHandler("Peça pertence ao jogador atual, não pode capturar");
                return false;
            }
            if((coordinates[0] == coordinates[2]) && coordinates[1] == coordinates[3]){
                Program.messageHandler("Você deve movimentar a peça para uma posição diferente da atual");
                return false;
            }
            return isValidForPiece(piece, coordinates,board, currentPlayer, isCheckCall);
        }

        public static bool isValidForPiece(char piece, int[] coordinates, char[,] board, int currentPlayer, bool isCheckCall = false){
            piece = Types.getPlayer1Piece(piece);
            bool result;

            switch(piece){
                case Types.KING:
                    result = isValidForKing(piece, coordinates, board, currentPlayer);
                    break;
                case Types.QUEEN:
                    result = isValidForQueen(piece, coordinates, board, currentPlayer);
                    break;
                case Types.ROOK:
                    result = isValidForRook(piece, coordinates, board, currentPlayer);
                    break;
                case Types.BISHOP:
                    result = isValidForBishop(piece, coordinates, board, currentPlayer);
                    break;
                case Types.PAWN:
                    result = isValidForPawn(piece, coordinates, board, currentPlayer);
                    break;
                default:
                    return false;
            }
            if(result){
                if(isCheckCall) return result;
                

                int checkResult = isCheck(board, currentPlayer, coordinates);
                if(checkResult == 1){
                    Program.messageHandler("Movimento inválido, peça em xeque");
                    return false;
                }else if(checkResult == 2){
                    int otherPlayer = currentPlayer == 1 ? 2 : 1;
                    Program.messageHandler("Rei do jogador "+otherPlayer+" está em xeque");
                }
            }
            return result;
        }
        public static bool isValidForKing(char piece, int[] coordinates, char[,] board, int currentPlayer){
            /*
            The king piece can move one single square in any direction.
            The king cannot move onto a square that is currently occupied by a piece from its own team.
            **The king piece cannot move to any square that puts them into a "check" position.
            */
            int linInicial, colInicial, linFinal, colFinal;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];

            int movedLines = Math.Abs(linFinal - linInicial);
            int movedColumns = Math.Abs(colFinal - colInicial);
            if(movedLines < 2 && movedColumns < 2) {
                return true;
            }else{
                Program.messageHandler("Movimento inválido para o Rei. Movimento maior que o possível.");
                return false;
            }
        }
        public static bool isValidForQueen(char piece, int[] coordinates, char[,] board, int currentPlayer){
            /*
            The queen can move in any direction on a straight or diagonal path.
            The queen cannot "jump" over any piece on the board, so its movements are restricted to any direction of unoccupied squares.
            The queen can be used to capture any of your opponent's pieces on the board.
             */
            int linInicial, colInicial, linFinal, colFinal;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];
            string name = "a Rainha";
            if((linInicial != linFinal) && (colInicial != colFinal)) return isValidDiagonal(piece, coordinates, board, name);
            else return isValidHorizontalVertical(piece, coordinates, board, name);
        }
        public static bool isValidHorizontalVertical(char piece, int[] coordinates, char[,] board, string pieceName){
            int linInicial, colInicial, linFinal, colFinal;
            int moveOne;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];
            if((linInicial < linFinal) || (colInicial < colFinal)){
                moveOne = 1;
            }else{
                moveOne = -1;
            }
            if(linInicial != linFinal){
                int lin;
                for(lin = linInicial+moveOne; lin != linFinal+moveOne; lin=lin+moveOne){
                    char currentPiece = board[lin, colInicial];
                    if(!Types.isEmpty(currentPiece) && (lin != linFinal)){
                        Program.messageHandler("Movimento inválido para "+pieceName+". Existe outra peça no caminho.");
                        return false;
                    }
                }
                return true;
            }
            if(colInicial != colFinal){
                int col;
                for(col = colInicial+moveOne; col != colFinal+moveOne; col=col+moveOne){
                    char currentPiece = board[linInicial, col];
                    if(!Types.isEmpty(currentPiece) && (col != colFinal)){
                        Program.messageHandler("Movimento inválido para "+pieceName+". Existe outra peça no caminho.");
                        return false;
                    }
                }
                return true;
            }

            Program.messageHandler("ERRO: Movimento inválido para"+pieceName+". MOVIMENTO NÃO TRATADO.");
            return false;
        }
        public static bool isValidForRook(char piece, int[] coordinates, char[,] board, int currentPlayer){
            /*
            The rook piece can move forward, backward, left or right at any time.
            The rook piece can move anywhere from 1 to 5 squares in any direction, so long as it is not obstructed by any other piece.
             */
            int linInicial, colInicial, linFinal, colFinal;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];
            if((linInicial != linFinal) && (colInicial != colFinal)){
                Program.messageHandler("Movimento inválido para a Torre. Movimento vertical e horizontal.");
                return false;
            }
            string name = "a Torre";
            return isValidHorizontalVertical(piece, coordinates, board, name);
        }
        public static bool isValidDiagonal(char piece, int[] coordinates, char[,] board, string pieceName){
            int linInicial, colInicial, linFinal, colFinal;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];
            int distY = Math.Abs(linFinal - linInicial);
            int distX = Math.Abs(colFinal - colInicial);

            if(distY - distX != 0){
                Program.messageHandler("Movimento inválido, é necessário que seja na diagonal.");
                return false; //checa se não está na diagonal
            }
            Program.messageHandler("Distância igual.");
            int moveX = (colFinal - colInicial);
            moveX = moveX/distX;
            int moveY = (linFinal - linInicial);
            moveY = moveY/distY;

            int lin = linInicial, col = colInicial, i;
            for(i = 0; i < distX; i++){
                lin += moveY;
                col += moveX;
                char currentPiece = board[lin, col];
                if(!Types.isEmpty(currentPiece) && ((col != colFinal) || (lin != linFinal))){
                    Program.messageHandler("Movimento inválido para "+pieceName+". Existe outra peça no caminho.");
                    return false;
                }
            }
            return true;
        }
        public static bool isValidForBishop(char piece, int[] coordinates, char[,] board, int currentPlayer){
            /*
            The bishop can move in any direction diagonally, so long as it is not obstructed by another piece.
            The bishop piece cannot move past any piece that is obstructing its path.
            The bishop can take any other piece on the board that is within its bounds of movement.
             */
            int linInicial, colInicial, linFinal, colFinal;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];
            if((linInicial == linFinal) || (colInicial == colFinal)){
                Program.messageHandler("Movimento inválido para o Bispo. São permitidos somente movimentos nas diagonais.");
                return false;
            }
            string name = "o Bispo";
            return isValidDiagonal(piece, coordinates, board, name);
        }
        public static bool isValidForPawn(char piece, int[] coordinates, char[,] board, int currentPlayer){
            /*
            Pawn chess pieces can only directly forward one square, with two exceptions.
            Pawns can move diagonally forward when capturing an opponent's chess piece.
            Once a pawn chess piece reaches the other side of the chess board, the player may "trade" the pawn in for any other chess piece if they choose, except another king.
             
             coordinates[0] linha1
             coordinates[1] coluna1
             coordinates[2] linha2
             coordinates[3] coluna2
             */
            bool isAttack = isAttackMove(coordinates, board);
            int moveOne;
            if(currentPlayer == 1){//move pra cima ou pra baixo
                moveOne = 1;
            }else{
                moveOne = -1;
            }
            if(isAttack){
                if((coordinates[2] == coordinates[0]+moveOne) && ((coordinates[3] == coordinates[1]+1) || (coordinates[3] == coordinates[1]-1))){
                    return true;//é movimento de ataque válido, capturou uma peça
                }else{
                    Program.messageHandler("Movimento inválido para o peão. Movimento de ataque inválido.");
                    return false;
                }
            }
            //ações daqui pra baixo não serão ataques
            if( (coordinates[2] == coordinates[0]+moveOne) && (coordinates[1] == coordinates[3])){
                return true;
            }
            Program.messageHandler("Movimento inválido para o peão. Movimento para o lado ou acima da quantidade esperada de casas");
            return false;
        }
        public static bool isAttackMove(int[] coordinates, char[,] board){
            char piece = board[coordinates[2], coordinates[3]];
            return !Types.isEmpty(piece);
        }
        public static LinkedList<int[]> possible_moves(State state, char checkCall = 'X'){
            LinkedList<int[]> moves = new LinkedList<int[]>();
            char currentPiece;
            int[] currentMove = new int[4];
            int lin1,col1,lin2,col2, size;
            /*
            percorrer toda a matriz procurando por peças do jogador atual
            para toda peça encontrada deve-se verificar para todas as casas da matriz se uma movimentação para aquela casa
            é uma movimentação válida.
            Se for uma movimentação válida, adicionar a lista encadeada a movimentação atual(currentMove)
            */
            currentMove = fillMove(currentMove, -1, -1, -1, -1);
            size = state.board.GetLength(0);
            Guid key = Guid.NewGuid();
            Program.deactivateMessaHandler(key);
            for(lin1 = 0 ; lin1 < size ; lin1++){//pega todas as peças do jogador
                for(col1 = 0; col1 < size; col1++){
                    currentPiece = state.board[lin1, col1];
                    if(Types.isPlayerX(currentPiece, state.currentPlayer)){//é peça do jogador
                        for(lin2 = 0; lin2 < size ; lin2++){//verifica para todas as casas do tabuleiro se um movimento para aquela casa é válido
                            for(col2 = 0; col2 < size ; col2++){
                                currentMove = fillMove(currentMove, lin1, col1, lin2, col2);
                                if(checkCall != 'X'){
                                    if (state.board[currentMove[2], currentMove[3]] == checkCall){
                                        if (validateMove(currentMove, state.board, state.currentPlayer, true)){
                                            LinkedList<int[]> moves_check_only = new LinkedList<int[]>();
                                            moves_check_only.AddLast(currentMove);
                                            return moves_check_only;
                                        }
                                    }
                                }else if(validateMove(currentMove, state.board, state.currentPlayer)){//movimento é válido
                                    moves.AddLast(currentMove);//coloca na lista de movimentos válidos
                                }
                                currentMove = fillMove(currentMove, -1, -1, -1, -1);//reseta
                            }
                        }
                    }
                }
            }
            Program.activateMessaHandler(key);
            
            return moves;
        }
        public static int[] fillMove(int[] move, int lin1, int col1, int lin2, int col2){
            int[] newMove = new int[4];
            newMove[0] = lin1;
            newMove[1] = col1;
            newMove[2] = lin2;
            newMove[3] = col2;
            return newMove;
        }
        public static int isCheck(char[,] board, int currentPlayer, int[] move){
            // 0 = não leva a xeque
            // 1 = leva a xeque de rei de currentPlayer
            // 2 = leva a xeque de rei inimigo
            // O QUE EU QUERO SABER
            // SE EU FIZER ESTE MOVIMENTO MEU REI ESTÁ EM XEQUE?
            // SE EU FIZER ESTE MOVIMENTO O REI DO OUTRO ESTÁ EM XEQUE?
            // ==> Verificar se movimento do jogador currentPlayer leva a um xeque do rei dele OK
            // ==> Verificar se movimento do jogador currentPlayer leva a um xeque do rei inimigo 

            int otherPlayer = currentPlayer == 1 ? 2 : 1;
            State state = new State(board, otherPlayer, null, 0);
            if(move != null){//sigfica que quero avaliar um movimento
                state.board[move[2], move[3]] = board[move[0], move[1]];//cria tabuleiro futuro a partir de movimento
                state.board[move[0], move[1]] = Types.EMPTY; // PROBLEMA AQUI colocar o 0 está fazendo mostrar mensagem de xeque indevidamente
            }//se move == null significa que quero saber se o estado atual está em xeque
            int[] kingPositionCurrentPlayer = findKingX(state.board, currentPlayer);
            int[] kingPositionOtherPlayer   = findKingX(state.board, otherPlayer);

            LinkedList<int[]> possibleMoves = possible_moves(state, Types.getPlayerXPiece(Types.KING, state.getOpponent()));//movimentos possiveis do inimigo
            
            foreach(int[] possibleMove in possibleMoves){//possiveis movimentos do INIMIGO de currentPlayer
                if(possibleMove[2] == kingPositionCurrentPlayer[0] && possibleMove[3] == kingPositionCurrentPlayer[1]){
                    Program.messageHandler("Seu rei em cheque se fizer essa jogada!");
                    return 1;
                }
            }
            state = new State(state.board, currentPlayer, null, 0);
            possibleMoves = possible_moves(state, Types.getPlayerXPiece(Types.KING, state.getOpponent()));//meus movimentos possíveis
            foreach(int[] possibleMove in possibleMoves){
                if(possibleMove[2] == kingPositionOtherPlayer[0] && possibleMove[3] == kingPositionOtherPlayer[1]){
                    return 2;
                }
            }
            return 0;
        }
        public static bool isCheckmate(State state){
            LinkedList<int[]> possibleMoves = possible_moves(state);
            foreach(int[] possibleMove in possibleMoves){
                if(isCheck(state.board, state.currentPlayer, possibleMove) != 1){//se movimento possibleMove não leva xeque do meu rei(=1)
                    return false;
                }
            }
            return true;
        }
        /*ver se é cheque mate, eu pego todos os movimentos possíveis do jogador que está com o rei sob xeque e vejo se todos deles o inCheck(...) 
        retorna verdadeiro, se para todos os movimentos possíveis do jogador ele ainda estiver em xeque, é xeque mate */
        public static int[] findKingX(char[,] board, int currentPlayer){
            int[] position = new int[2];
            int lin, col, size;
            size = board.GetLength(0);
            char currentPiece;
            for(lin = 0 ; lin < size ; lin++){ //procura pela posição do rei
                for(col = 0; col < size; col++){
                    currentPiece = board[lin, col];
                    if(Types.getPlayer1Piece(currentPiece) == Types.KING){ //é rei
                        if(Types.isPlayerX(currentPiece, currentPlayer)){ //é do jogador atual
                            position[0] = lin;
                            position[1] = col;
                            return position;
                        }
                    }
                }
            }
            position[0] = -1;
            position[1] = -1;
            Program.messageHandler("ERRO. rei não encontrado");
            return position;
        }
    }
}