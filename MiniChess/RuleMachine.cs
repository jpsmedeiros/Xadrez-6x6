using System;
using TypesNS;
using MiniChess;

namespace RuleMachineNS
{
    class RuleMachine
    {
        public static bool validateMove(int[] coordinates, char[,] board, int currentPlayer){
            char piece = board[coordinates[0], coordinates[1]];
            if(Types.isEmpty(piece)){
                Program.errorHandler("Casa vazia, tente selecionar outra casa");
                return false;
            }
            if(!Types.isPlayerX(piece, currentPlayer)){
                Program.errorHandler("Peça não pertence ao jogador atual, tente mover uma outra peça");
                return false;
            }
            return true;
        }
    }
}