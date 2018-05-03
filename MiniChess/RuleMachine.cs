using System;
using TypesNS;
using MiniChess;

namespace RuleMachineNS
{
    class RuleMachine
    {
        public static bool validateMove(int[] coordinates, char[,] board, int currentPlayer){
            for(int i=0;i<coordinates.Length;i++){
                if(coordinates[i] > 6 || coordinates[i] < 1){
                    Program.messageHandler("Jogada fora do tabuleiro");
                    return false;
                }
            }
            char piece = board[coordinates[0], coordinates[1]];
            Console.WriteLine("PEÇA SENDO AVALIADA: "+piece);
            if(Types.isEmpty(piece)){
                Program.messageHandler("Casa vazia, tente selecionar outra casa");
                return false;
            }
            if(!Types.isPlayerX(piece, currentPlayer)){
                Program.messageHandler("Peça não pertence ao jogador atual, tente mover uma outra peça");
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
                    Console.WriteLine("Verificando para peão");
                    return isValidForPawn(piece, coordinates, board, currentPlayer);
                default:
                    return false;
            }
        }
        public static bool isValidForKing(char piece, int[] coordinates, char[,] board){
            return false;//TODO
        }
        public static bool isValidForQueen(char piece, int[] coordinates, char[,] board){
            return false;//TODO
        }
        public static bool isValidForRook(char piece, int[] coordinates, char[,] board){
            return false;//TODO
        }
        public static bool isValidForBishop(char piece, int[] coordinates, char[,] board){
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
                    Program.messageHandler("Peça capturada na Linha: "+coordinates[2]+" Coluna: "+coordinates[3]+" Peça capturada: "+board[coordinates[2], coordinates[3]]);
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
    }
}