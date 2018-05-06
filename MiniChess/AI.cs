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
            
            //ajusta os indices o padrao que aparece na tela
            for(int i=0; i<p.Length; i++){
                p[i] = p[i] + 1;
            }

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
                if(v < temp){
                    v = temp;
                    moveIndex = i;    
                }

                if(v >= beta){
                    return move;
                }
                
                //MAX(alfa,v)
                if(alfa < v){
                    alfa = v;
                }
                
                i++;
            }

            LinkedList<int[]>.Enumerator e = moves.GetEnumerator();
            for(i=0; i<moveIndex+1; i++){
                e.MoveNext();
                //Console.WriteLine($"{e.Current[0]} {e.Current[1]} {e.Current[2]} {e.Current[3]}");
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
            if(cut(state)){
                return eval(state);
            }
            
            if(Program.gameIsOver(state)){
                return eval(state);
            }

            int v = Int32.MinValue;
            int temp;
            LinkedList<int[]> moves = RuleMachine.possible_moves(state);
            foreach(var move in moves){
                //MAX(v,min_value(result(s,a),alfa,beta))
                temp = min_value(State.result(state,move),alfa,beta);
                if(v < temp){
                    v = temp;
                }
                
                if(v >= beta){
                    return v;
                }
                
                //MAX(alfa,v)
                if(alfa < v){
                    alfa = v;
                }
            }
            return v;
        }

        public int min_value(State state,int alfa, int beta){
            if(cut(state)){
                return eval(state);
            }
            
            if(Program.gameIsOver(state)){
                return eval(state);
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

                if(v <= alfa){
                    return v;
                }

                //MIN(beta,v)
                if(beta > v){
                    beta = v;
                }
            }
            return v;
        }

        public int eval(State state){
            switch(this.type){
                case 1:
                    return utility(state);
                case 2:
                    return Program.evalSimples(state.board);
                default:
                    return utility(state);
            }

        }

        //Utility avalia apenas estados terminais
        public int utility(State state){
            if(state.currentPlayer == playerId){
                return Int32.MinValue;
            }
            else{
                return Int32.MaxValue;
            }

            //return Program.evalSimples(state.board);
        }

        public bool cut(State state){
            switch(this.type){
                case 1:
                    return false;
                case 2:
                    return cutoff_test(state);
                default:
                    return false;
            }
        }

        public bool cutoff_test(State state){
            if((state.playsCount - Program.currentState.playsCount) <= 8){
                //Console.WriteLine($"AAAAAA {state.playsCount}");
                return false;
            }
            else{
                return true;
            }
        }

    }
}
