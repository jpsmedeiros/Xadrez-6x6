using System;
using TypesNS;
using MiniChess;

namespace RuleMachineNS
{
    class RuleMachine
    {
        public static bool validateMove(int[] coordinates, char[,] board, int currentPlayer){
            char piece = board[coordinates[0], coordinates[1]];
            Console.WriteLine("PEÇA SENDO AVALIADA: "+piece);
            if(Types.isEmpty(piece)){
                Program.errorHandler("Casa vazia, tente selecionar outra casa");
                return false;
            }
            if(!Types.isPlayerX(piece, currentPlayer)){
                Program.errorHandler("Peça não pertence ao jogador atual, tente mover uma outra peça");
                return false;
            }
            return isValidForPiece(piece, coordinates,board);
        }

        public static bool isValidForPiece(char piece, int[] coordinates, char[,] board){
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
                    return isValidForPawn(piece, coordinates, board);
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
        public static bool isValidForPawn(char piece, int[] coordinates, char[,] board){
            return false;//TODO
        }
    }
}