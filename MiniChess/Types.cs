using System;

namespace TypesNS
{
    class Types
    {
        public static char KING = 'K';
        public static char QUEEN = 'Q';
        public static char ROOK = 'T';
        public static char BISHOP = 'B';
        public static char PAWN = 'P';
        public static char EMPTY = '0';

        public static bool isPlayer1(char piece){
            return Char.IsUpper(piece);
        }
        public static bool isPlayer2(char piece){
            return Char.IsLower(piece);
        }
        public static bool isPlayerX(char piece, int player){
            if(player == 1)
                return isPlayer1(piece);
            return isPlayer2(piece);
        }
        public static bool isEmpty(char piece){
            if(piece == EMPTY)
                return true;
            else return false;
        }
        public static char getPlayer2Piece(char piece){
            return Char.ToLower(piece);
        }
    }
}
