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
            if(Program.gameIsOver(state)){
                Console.WriteLine($"A IA jogador:{this.playerId} está tentando jogar depois do jogo ter terminado");
                return null;
            }

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

        public int min_value(State state, int alfa, int beta){
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
                    return evalSimples(state);
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

        //função de avaliação simples
        public static int evalSimples(State state){
            int p=0, b=0, t=0, q=0;
            int eval = 0;
            // char chP = Char.ToLower("P");
            // char "b" = Char.ToLower("B");
            // char "t" = Char.ToLower("T");
            // char "q" = Char.ToLower("Q");

            switch(Program.getCurrentPlayer()){
                case 1:
                    for(int i=0; i<6; i++){
                        for(int j=0; j<6; j++){
                            switch(state.board[i,j]){
                                case 'p':
                                    p-=1;
                                    break;
                                case 'P':
                                    p+=1;
                                    break;
                                case 'b':
                                    b-=1;
                                    break;
                                case 'B':
                                    b+=1;
                                    break;
                                case't':
                                    t-=1;
                                    break;
                                case 'T':
                                    t+=1;
                                    break;
                                case 'q':
                                    q-=1;
                                    break;
                                case 'Q':
                                    q+=1;
                                    break;
                            }
                        }
                    }
                    break;
                case 2:
                    for(int i=0; i<6; i++){
                        for(int j=0; j<6; j++){
                            switch(state.board[i,j]){
                                case 'p':
                                    p+=1;
                                    break;
                                case 'P':
                                    p-=1;
                                    break;
                                case 'b':
                                    b+=1;
                                    break;
                                case 'B':
                                    b-=1;
                                    break;
                                case 't':
                                    t+=1;
                                    break;
                                case 'T':
                                    t-=1;
                                    break;
                                case 'q':
                                    q+=1;
                                    break;
                                case 'Q':
                                    q-=1;
                                    break;
                            }
                        }
                    }
                    break;
            }
            eval = p + 3*b + 5*t + 9*q;
            return eval;
        }
        //função de utilidade de estado final
        public static int evalUtility(State state){
            if(state.checkDraw()){
                return 0;
            }

            int p=0, b=0, t=0, q=0;
            int util =0;
            // char "p" = Char.ToLower("P");
            // char "b" = Char.ToLower("B");
            // char "t" = Char.ToLower("T");
            // char "q" = Char.ToLower("Q");
            
            if(Program.gameIsOver(state)){
                switch(Program.getCurrentPlayer()){
                    case 1:
                        for(int i=0; i<6; i++){
                            for(int j=0; j<6; j++){
                                switch(state.board[i,j]){
                                    case 'p':
                                        p-=1;
                                        break;
                                    case 'P':
                                        p+=1;
                                        break;
                                    case 'b':
                                        b-=1;
                                        break;
                                    case 'B':
                                        b+=1;
                                        break;
                                    case 't':
                                        t-=1;
                                        break;
                                    case 'T':
                                        t+=1;
                                        break;
                                    case 'q':
                                        q-=1;
                                        break;
                                    case 'Q':
                                        q+=1;
                                        break;
                                }
                            }
                        }
                        if(Program.getWinner(state)==2){
                            util -= 100;
                        }
                        else{
                            util += 100;
                        }
                        break;
                    case 2:
                        for(int i=0; i<6; i++){
                            for(int j=0; j<6; j++){
                                switch(state.board[i,j]){
                                    case 'p':
                                        p+=1;
                                        break;
                                    case 'P':
                                        p-=1;
                                        break;
                                    case 'b':
                                        b+=1;
                                        break;
                                    case 'B':
                                        b-=1;
                                        break;
                                    case 't':
                                        t+=1;
                                        break;
                                    case 'T':
                                        t-=1;
                                        break;
                                    case 'q':
                                        q+=1;
                                        break;
                                    case 'Q':
                                        q-=1;
                                        break;
                                }
                            }
                        }
                        if(Program.getWinner(state)==1){
                            util -= 100;
                        }
                        else{
                            util += 100;
                        }
                        break;
                }
                
            }   
            util += p + 3*b + 5*t + 9*q;
            util = util/(state.playsCount);
            return util;
            
        }

    }
}
