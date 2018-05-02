using System;

namespace TypesNS
{
    class Types
    {
        public char KING = 'K';
        public char QUEEN = 'Q';
        public char ROOK = 'T';
        public char BISHOP = 'B';
        public char PAWN = 'P';
        public char EMPTY = '0';

        public bool isPlayer1(char piece){
            return Char.IsUpper(piece);
        }
        public bool isPlayer2(char piece){
            return Char.IsLower(piece);
        }
        public bool isPlayer(char piece, int player){
            if(player == 1){
                return isPlayer1(piece);
            }
            return isPlayer2(piece);
        }
        public bool isEmpty(char piece){
            if(piece == EMPTY)
                return true;
            else return false;
        }
    }
}
