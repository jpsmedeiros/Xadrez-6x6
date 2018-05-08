using System;

namespace TypesNS
{
    class Types
    {
        public const char KING = 'K';
        public const char QUEEN = 'Q';
        public const char ROOK = 'T';
        public const char BISHOP = 'B';
        public const char PAWN = 'P';
        public const char EMPTY = '0';

        public const char KING2 = 'k';
        public const char QUEEN2 = 'q';
        public const char ROOK2 = 't';
        public const char BISHOP2 = 'b';
        public const char PAWN2 = 'p';

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
        public static char getPlayer1Piece(char piece){
            return Char.ToUpper(piece);
        }
        public static char getPlayerXPiece(char piece, int player){
            if (player == 1)
                return getPlayer1Piece(piece);
            else
                return getPlayer2Piece(piece);
        }
        public static int getPiecePlayer(char piece){
            return isPlayer1(piece) ? 1 : 2;
        }
    }
}
