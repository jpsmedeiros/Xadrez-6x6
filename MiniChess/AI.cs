using System;
using System.Collections.Generic;
using StateNS;
using TypesNS;
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

            int v    = Int32.MinValue;
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
            }
            return e.Current;
        }

        public int max_value(State state, int alfa, int beta){
            if(cut(state) || state.gameIsOver()){
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

        public int min_value(State state, int alfa, int beta){
            if(cut(state) || state.gameIsOver()){
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
                    return eval1(state);
                case 2:
                    return eval2(state);
                default:
                    return eval1(state);
            }
        }

        public bool cut(State state){
           switch(this.type){
               case 1:
                   return cutoff_test(state);
               case 2:
                   return cutoff_test(state);
               default:
                   return false;
           }
        }

        //função de avaliação sobre a quantidade de movimentos possiveis
        //retorna a qtd de movimentos possiveis
        public static int evalMobility(State state){
            LinkedList<int[]> moves = RuleMachine.possible_moves(state);
            return moves.Count;
        }
        //função de avaliação do controle do centro do tabuleiro
        public static int evalCenterControl(State state){
            int count=0;
            char piece;
            for(int i=2; i<=3; i++){
                for(int j=2; j<=3; j++){
                    if(Program.getCurrentPlayer()==1){
                        piece = state.board[i,j];
                        if(!(piece=='0')){
                            if(Char.IsUpper(piece)){
                                count+=1;
                            }
                            else{
                                count-=1;
                            }
                        }
                    }
                    else{
                        piece = state.board[i,j];
                        if(!(piece=='0')){
                            if(Char.IsLower(piece)){
                                count+=1;
                            }
                            else{
                                count-=1;
                            }
                        }
                    }
                }
            }
            return count;
        }   

        //função de avaliação simples
        public static int evalMaterial(State state){
            int p=0, b=0, t=0, q=0, eval=0;
            
            for(int i=0; i<6; i++){
                for(int j=0; j<6; j++){
                    switch(state.board[i,j]){
                        case Types.PAWN2:
                            p-= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.PAWN:
                            p+= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.BISHOP2:
                            b-= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.BISHOP:
                            b+= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.ROOK2:
                            t-= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.ROOK:
                            t+= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.QUEEN2:
                            q-= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.QUEEN:
                            q+= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                    }
                }
            }
            eval = p + 3*b + 5*t + 9*q;
            return eval;
        }

        public bool cutoff_test(State state){
            return state.playsCount - Program.currentState.playsCount > 4;
        }
        //funcao de avaliacao usando material e quantidade de jogadas
        public static int eval1(State state){
            if(state.checkDraw()){
                return 0;
            }

            int p=0, b=0, t=0, q=0, util=0, winner=-1;
            
            util = evalMaterial(state);
            winner = Program.getWinner(state);

            if((Program.getCurrentPlayer())==1) util += winner < 2 ? -100 : 100;
            else util += winner == 2 ? 100 : -100;
               
            util += p + 3*b + 5*t + 9*q;
            util = (int) util/(state.playsCount);
            return util;
        }
    
        public static int eval2(State state){
            int c1=25, c2=35, c3=40;
            return (int)( (c1*evalCenterControl(state)) + (c2*evalMaterial(state)) + (c3*evalMobility(state)) )/100; 
        }
    }
}
