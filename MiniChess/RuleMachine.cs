using System;
using System.Collections.Generic;
using TypesNS;
using MiniChess;

namespace RuleMachineNS
{
    class RuleMachine
    {
        public static bool validateMove(int[] coordinates, char[,] board, int currentPlayer){
            for(int i=0;i<coordinates.Length;i++){
                int atual = coordinates[i]+1;
                if(atual > 6 || atual < 1){
                    Program.messageHandler("Jogada fora do tabuleiro");

                    return false;
                }
            }
            char piece = board[coordinates[0], coordinates[1]];
            //Console.WriteLine("PEÇA SENDO AVALIADA: "+piece);
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
            return isValidForPiece(piece, coordinates,board, currentPlayer);
        }

        public static bool isValidForPiece(char piece, int[] coordinates, char[,] board, int currentPlayer){
            piece = Types.getPlayer1Piece(piece);
            switch(piece){
                case Types.KING:
                    return isValidForKing(piece, coordinates, board);
                case Types.QUEEN:
                    return isValidForQueen(piece, coordinates, board);
                case Types.ROOK:
                    return isValidForRook(piece, coordinates, board);
                case Types.BISHOP:
                    return isValidForBishop(piece, coordinates, board);
                case Types.PAWN:
                    return isValidForPawn(piece, coordinates, board, currentPlayer);
                default:
                    return false;
            }
        }
        public static bool isValidForKing(char piece, int[] coordinates, char[,] board){
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
            
            return isCheck();//TODO
        }
        public static bool isValidForQueen(char piece, int[] coordinates, char[,] board){
            /*
            The queen can move in any direction on a straight or diagonal path.
            The queen cannot "jump" over any piece on the board, so its movements are restricted to any direction of unoccupied squares.
            The queen can be used to capture any of your opponent's pieces on the board.
             */
            return false;//TODO
        }
        public static bool isValidForRook(char piece, int[] coordinates, char[,] board){
            /*
            The rook piece can move forward, backward, left or right at any time.
            The rook piece can move anywhere from 1 to 5 squares in any direction, so long as it is not obstructed by any other piece.
             */
            int lin,col;
            int linInicial, colInicial, linFinal, colFinal;
            int moveOne;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];
            if((linInicial != linFinal) && (colInicial != colFinal)){
                Program.messageHandler("Movimento inválido para a Torre. Movimento vertical e horizontal.");
                return false;
            }
            if((linInicial < linFinal) || (colInicial < colFinal)){
                moveOne = 1;
            }else{
                moveOne = -1;
            }
            if(linInicial != linFinal){
                for(lin = linInicial+moveOne; lin != linFinal+moveOne; lin=lin+moveOne){
                    char currentPiece = board[lin, colInicial];
                    if(!Types.isEmpty(currentPiece) && (lin != linFinal)){
                        Program.messageHandler("Movimento inválido para a Torre. Existe outra peça no caminho.");
                        return false;
                    }
                }
                return true;
            }
            if(colInicial != colFinal){
                for(col = colInicial+moveOne; col != colFinal+moveOne; col=col+moveOne){
                    char currentPiece = board[linInicial, col];
                    if(!Types.isEmpty(currentPiece) && (col != colFinal)){
                        Program.messageHandler("Movimento inválido para a Torre. Existe outra peça no caminho.");
                        return false;
                    }
                }
                return true;
            }
            Program.messageHandler("ERRO: Movimento inválido para a Torre. MOVIMENTO NÃO TRATADO.");
            return false;//TODO
        }
        public static bool isValidForBishop(char piece, int[] coordinates, char[,] board){
            /*
            The bishop can move in any direction diagonally, so long as it is not obstructed by another piece.
            The bishop piece cannot move past any piece that is obstructing its path.
            The bishop can take any other piece on the board that is within its bounds of movement.
             */
            int lin, col;
            int linInicial, colInicial, linFinal, colFinal;
            int moveX, moveY;
            linInicial = coordinates[0];
            colInicial = coordinates[1];
            linFinal = coordinates[2];
            colFinal = coordinates[3];

            if((linInicial == linFinal) || (colInicial == colFinal)){
                Program.messageHandler("Movimento inválido para o Bispo. São permitidos somente movimentos nas diagonais.");
                return false;
            }

            if(linInicial < linFinal){
                moveX = 1;
                if(colInicial < colFinal) moveY = 1;
                else moveY = -1;
            } else{
                moveX = -1;
                if(colInicial < colFinal) moveY = 1;
                else moveY = -1;
            }

            //Console.WriteLine("MEU MOVEX: "+moveX);
            //Console.WriteLine("MEU MOVEY: "+moveY);

            int auxX = 0, auxY = 0;
            lin = linInicial+moveX; //3+1
            col = colInicial+moveY; //4+1
            while((lin != linFinal+moveX) && (col != colFinal+moveY)){ // x6 y6
                auxX++;
                auxY++;
                char currentPiece = board[lin, col];
                if(!Types.isEmpty(currentPiece) && ((col != colFinal) || (lin != linFinal))){
                    Program.messageHandler("Movimento inválido para o Bispo. Existe outra peça no caminho.");
                    return false;
                }
                lin += moveX;
                col += moveY;
            }

            /*for(lin = linInicial+moveX; lin != linFinal+moveX; lin+=moveX){
                auxX++;
                for(col = colInicial+moveY; col != colFinal+moveY; col+=moveY){
                    auxY++;
                    char currentPiece = board[lin, col];
                    if(!Types.isEmpty(currentPiece) && ((col != colFinal) || (lin != linFinal))){
                        Program.messageHandler("Movimento inválido para o Bispo. Existe outra peça no caminho.");
                        return false;
                    }
                }
            }*/

            //Console.WriteLine("AUXX: "+auxX);
            //Console.WriteLine("AUXY: "+auxY);
            if(auxX == auxY) return true; //se auxX == auxY, significa que fez a mesma quantidade de movimento na vertical e na horizontal, ou seja, manteve-se na diagonal
            else{
                Program.messageHandler("Movimento inválido para o Bispo. São permitidos somente movimentos nas diagonais.");
                return false;
            }

            Program.messageHandler("ERRO: Movimento inválido para o Bispo. MOVIMENTO NÃO TRATADO.");
            return false;//TODO
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
            if(coordinates[2] != coordinates[0]+moveOne){
                Program.messageHandler("Movimento inválido para o peão. Movimento para o lado ou acima da quantidade esperadade casas");
                return false;
            }
            //if((coordinates[1] != coordinates[3])){//não é ataque e movimentou na diagonal
            //    Program.messageHandler("Movimento inválido para o peão. Movimento na diagonal fora de movimento de ataque.");
            //    return false;
            //}
            //if((coordinates[3] > coordinates[1]+1) || coordinates[3] < coordinates[1]-1){
            //    Program.messageHandler("Movimento inválido para o peão. Movimento muito grande na diagonal.");
            //    return false;
            //}
            return true;
        }
        public static bool isAttackMove(int[] coordinates, char[,] board){
            char piece = board[coordinates[2], coordinates[3]];
            return !Types.isEmpty(piece);
        }
        public static bool isCheck(){
            return false;
        }

        public static LinkedList<int[]> possible_moves(char[,] board, int currentPlayer){
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
            size = board.GetLength(0);
            int contador = 0;
            int contador2 = 0;
            Program.activateOrDeactivateMessageHandler();
            for(lin1 = 0 ; lin1 < size ; lin1++){//pega todas as peças do jogador
                for(col1 = 0; col1 < size; col1++){
                    currentPiece = board[lin1, col1];
                    if(Types.isPlayerX(currentPiece, currentPlayer)){//é peça do jogador
                        for(lin2 = 0; lin2 < size ; lin2++){//verifica para todas as casas do tabuleiro se um movimento para aquela casa é válido
                            for(col2 = 0; col2 < size ; col2++){
                                contador++;
                                currentMove = fillMove(currentMove, lin1, col1, lin2, col2);
                                if(validateMove(currentMove, board, currentPlayer)){//movimento é válido
                                    moves.AddLast(currentMove);//coloca na lista de movimentos válidos
                                    contador2++;
                                    //TESTAR
                                }
                                currentMove = fillMove(currentMove, -1, -1, -1, -1);//reseta
                            }
                        }
                    }
                }
            }
            Program.activateOrDeactivateMessageHandler();
            Console.WriteLine("CONTADOR: " +contador);
            Console.WriteLine("QTD JOGADAS POSSIVEIS: "+contador2);
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
    }
}