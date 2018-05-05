using System;
using System.Collections.Generic;
using StateNS;
using RuleMachineNS;
using MiniChess;

namespace AINS
{
    class AI
    {
        int playerId;
        int type;
        public AI(int playerId, int type){
            this.playerId = playerId;
            this.type = type;
        }

        public string play(State state){
            int[] p = alpha_beta_search(state);
            string result = string.Join(" ", p);
            Console.WriteLine(result);
            return result;
        }

        public int[] alpha_beta_search(State state){
            if(Program.gameIsOver(state)){
                Console.WriteLine($"A IA jogador:{this.playerId} está tentando jogar depois do jogo ter terminado");
                return null;
            }

            int v = Int32.MinValue;
            int alfa = Int32.MinValue;
            int beta = Int32.MaxValue;
            
            LinkedList<int[]> moves = RuleMachine.possible_moves(state);
            int i=0, temp, moveIndex=0;           
            foreach(var move in moves){
                //MAX(v,min_value(result(s,a),alfa,beta))
                temp = min_value(State.result(state,move),alfa,beta);
                if(v < temp)
                    v = temp;
                    moveIndex = i;    
                
                if(v >= beta)
                    return move;
                
                //MAX(alfa,v)
                if(alfa < v)
                    alfa = v;
                
                i++;
            }

            LinkedList<int[]>.Enumerator e = moves.GetEnumerator();
            for(i=0; i<moveIndex; i++){
                e.MoveNext();
            }
            return e.Current;
            // i=0;
            // foreach(var move in moves){
            //     if(i==moveIndex)
            //         return move;
            //     i++;
            // }
        }

        public int max_value(State state, int alfa, int beta){
            if(Program.gameIsOver(state)){
                return utility(state);
            }

            int v = Int32.MinValue;
            int temp;
            LinkedList<int[]> moves = RuleMachine.possible_moves(state);
            foreach(var move in moves){
                //MAX(v,min_value(result(s,a),alfa,beta))
                temp = min_value(State.result(state,move),alfa,beta);
                if(v < temp)
                    v = temp;
                
                if(v >= beta)
                    return v;
                
                //MAX(alfa,v)
                if(alfa < v)
                    alfa = v;
            }
            return v;
        }

        public int min_value(State state,int alfa, int beta){
            if(Program.gameIsOver(state)){
                return utility(state);
            }

            int v = Int32.MaxValue;
            int temp;
            LinkedList<int[]> moves = RuleMachine.possible_moves(state);
            foreach(var move in moves){
                //MIN(v,max_value(result(s,a),alfa,beta))
                temp = max_value(State.result(state,move),alfa,beta);
                if(v > temp){
                    v = temp;
                }

                if(v <= alfa)
                    return v;

                //MIN(beta,v)
                if(beta > v)
                    beta = v;
            }
            return v;
        }

        //Utility avalia apenas estados terminais
        public int utility(State state){
            if(state.currentPlayer == playerId){
                return Int32.MinValue;
            }
            else{
                return Int32.MaxValue;
            }
        }

    }
}
