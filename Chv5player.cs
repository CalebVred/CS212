/*
 * Name: chv5
 * Date: December 9, 2019
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Timers;
namespace Mankalah
{
    //Class moveResult allows storage, change and retreival of move and score values
    public class moveResult
    {
        int move;
        int score;
        public moveResult(int mv, int scr){
            move = mv;
            score = scr;
        }

        //Accessor returns the value of move
        public int getMove()
        {
            return move;
        }

        //Accessor returns the value of score
        public int getScore()
        {
            return score;
        }

        //Mutator changes the value of move to the value of newMove
        public void changeMove(int newMove)
        {
            move = newMove;
        }

        //Mutator changes the value of score to the value of newScore
        public void changeScore(int newScore)
        {
            score = newScore;
        }
    }
    public class Chv5Player : Player
    {
        bool timeOut = false;
        public Chv5Player(Position pos, int timeLimit) : base(pos, "Chv5", timeLimit){}

        public override string gloat()
        {
            return "Deal with it";
        }

        //Function to handle the timeout
        public void timeOutTrigger(Object source, ElapsedEventArgs e)
        {

            Console.WriteLine("Timeout triggered");
            timeOut = true;
            
        }

        public override int chooseMove(Board b)
        {
            
            int tL = 4;
            Timer timer = new Timer(tL);
            timeOut = false;
            timer.Elapsed += timeOutTrigger;
            timer.Enabled = true;
            timer.AutoReset = false;
            timer.Start();
            List<int> choices= new List<int>();
            int choice = 0;
            int startDepth = 11;

            //Pass in timer to minimax? 
           
            //If the elapsed time matches or exceeds the time limit, break the loop
            while (true)
            {
                if(timeOut || b.gameOver())
                {
                    break;
                }
                Console.WriteLine("Depth: " + startDepth);
                if( startDepth > 20)
                {
                    Console.WriteLine("We might have a problem");
                    
                }
                //If the value of choice is not 0, add it to the list of choices.
                moveResult choiceResult = new moveResult(0, 0);
                if (!timeOut)
                    choiceResult = miniMax(b, startDepth, tL, 1, timer);

                if (choiceResult.getScore() != 0 && b.legalMove(choiceResult.getMove()))
                    //TODO: Adding stuff to a list seems to not work
                    choices.Add(choiceResult.getMove());
                    //choice = miniMax(b, startDepth, tL).getScore();
           

                //TODO: Modify timer and Stop, store and print time elapsed
               
                //update depth.
                startDepth += 1;
                if (timeOut == true)
                {
                    timer.Stop();
                }
                
            }

            //Close the timer and return the move from the last item in the list of choices
            timer.Close();
            return choices.Last();
        }

        //int d is depth of miniMax search. int max should be 1 for getting the max result, -1 for min. Should return moveResult object
        public moveResult miniMax(Board b, int d, double timeLimit, int max, Timer timer)
        {
            //Set up variables 
            int bestVal = int.MinValue;
            int bestMove = 0;
            moveResult bestMR = new moveResult(bestMove, bestVal);
           
            //Base case: depth is 0, time is up or game is over. Returns a moveResult with the best score value and 0 move value
            if (d == 0 || timeOut || b.gameOver())
            {
                bestVal = evaluate(b);
                //Console.WriteLine("Best score found to be " + bestVal);
                moveResult bestMoveResult = new moveResult(bestMove, bestVal);
                return (bestMoveResult);
            }
            
            if (b.whoseMove() == Position.Top)
            {
                for (int move = 7; move <= 12; move++)
                {
                    //Console.WriteLine("");
                    //Recurse if the move is legal, time hasn't expired and game isn't over
                    int val = 0;
                    if (b.legalMove(move) && !timeOut && !b.gameOver())
                    {
                        Board b1 = new Board(b);
                        b1.makeMove(move, true);
                        int newDepth = d - 1;

                        //Switch the value of max to be the opposite for the recursion
                        int mnOrMx = max * -1;
                        //Console.WriteLine("Recursing from " + d + " to " + newDepth + "...");
                        val = miniMax(b1, newDepth, timeLimit, mnOrMx, timer).getScore();
                        

                        //If at "max" level, prioritize and return maximum value
                        if (max == 1)
                            //If there's a new max, update bestValue and bestMove
                            if (val > bestVal)
                            {
                                bestVal = val;
                                bestMove = move;
                            }

                        //If at "min" level, prioritize and return minimum value
                        if (max == -1)
                            //If there's a new min, update bestValue and bestMove
                            if (val < bestVal || val != int.MinValue)
                            {
                                bestVal = val;
                                bestMove = move;
                            }
                        
                    }
                }
                bestMR = new moveResult(bestMove, bestVal);
                return bestMR;
            }
            else
            {
                for (int move = 0; move <= 5; move++)
                {
                    //Console.WriteLine("");
                    //Recurse if the move is legal, time hasn't expired and game isn't over
                    if (b.legalMove(move) && !timeOut && !b.gameOver())
                    {
                        Board b1 = new Board(b);
                        b1.makeMove(move, true);
                        int newDepth = d - 1;

                        //Switch the value of max to be the opposite for the recursion
                        int mnOrMx = max * -1;
                       // Console.WriteLine("Recursing from " + d + " to " + newDepth + "...");
                        int val = miniMax(b1, newDepth, timeLimit, mnOrMx, timer).getScore();
                       // Console.WriteLine("New Value: " + val);

                        //If at "max" level, prioritize and return maximum value
                        if (max == 1)
                            //If there's a new max, update bestValue and bestMove
                            if (val > bestVal)
                            {
                                bestVal = val;
                                bestMove = move;
                            }

                        //If at "min" level, prioritize and return minimum value
                        if (max == -1)
                            //If there's a new min, update bestValue and bestMove
                            if (val < bestVal || val != int.MinValue)
                            {
                                bestVal = val;
                                bestMove = move;
                            }
                        

                    }

                    
                }
                bestMR = new moveResult(bestMove, bestVal);
                return bestMR;

            }

            //Console.WriteLine("Best move: " + bestMove + " Best score: " + bestVal);
            bestMR = new moveResult(bestMove, bestVal);
            return bestMR;


        }

        //Function evaulates the board and returns a score paired with a board space value
        public int evaluate(Board b)
        {
            int score = 0;
            if (b.whoseMove() == Position.Top)
            {
                //oppoDiff is used to keep the difference between the current hole and the opposite one across the board
                for (int i = 12; i >= 7; i--)
                {
                    int lastHole = (i + b.stonesAt(i)) % 13;
                    // CASE 1.0: If there's a go-again available, add 1 to the score
                    if (b.stonesAt(i) == 13 - i) score += 2;

                    // CASE 1.1: If there's a go-again available for the opponent, subtract 1 from the score
                    //also use loop for...
                    // CASE 2.0: If the holes are empty, check to see if they can be filled
                    // ...and...
                    //CASE 2.1: Check opponent's holes to see if they can be filled
                    for (int j = 5; j >= 0; j--)
                    {
                        int lastOppoHole = (j + b.stonesAt(j)) % 13;

                        //CASE 1.1 check...
                        if (b.stonesAt(j) == 6 - j)
                        {
                            score -= 2;
                            //Counter-attack: If this opponent hole can be filled by the stones in the current hole,
                            //add to score
                            if (b.stonesAt(j) < lastHole)
                            {
                                score += 5;
                            }
                        }
                        //CASE 2.0 check...
                        if (b.stonesAt(j) == 0)
                        {
                            if (lastHole >= j)
                                score += 5;
                        }

                        //CASE 2.1 check...
                        if (j + lastOppoHole >= i)
                        {
                            score -= 2;
                        }
                    }

                    
                        

                    
                    
                    //If the last stone is placed within the top...
                    if (lastHole < 13)
                        // CASE 3.0: Possible stone capture
                        if (b.stonesAt(lastHole) == 0)
                        {
                            //if the last stone is placed in an empty hole on the top
                            score += 1;
                            
                            if (b.stonesAt(getOpposite(lastHole)) != 0)
                                //if there are stones found in the opposite hole, add them to the score
                                score += b.stonesAt(getOpposite(lastHole));
                        }
                        //CASE 3.1: Last stone placed will contribute to a possible capture from your opponent
                        else
                        {
                            //If the opposite hole is empty and not 0
                            if(b.stonesAt(getOpposite(lastHole)) == 0 && getOpposite(lastHole) != 0)
                            {
                                for (int j = b.stonesAt(getOpposite(lastHole-1)); j >= 0; j--)
                                {   //If any of the holes leading up to the opposite empty hole have enough stones
                                    //to put one in the empty hole and capture, subtract from the score
                                    if (b.stonesAt(j) + j == b.stonesAt(getOpposite(lastHole)))
                                        score -= b.stonesAt(i);
                                }
                            }
                        }
                }
            }
            else
            {
                
                for (int i = 5; i >= 0; i--)
                {
                    //int lasthole is the hole that gets the last stone from hole i
                    int lastHole = (i + b.stonesAt(i)) % 13;

                    // CASE 1.0: If there's a go-again available, add 2 to the score
                    if (b.stonesAt(i) == 6 - i) score += 2;

                    // CASE 1.1: If there's a go-again available for the opponent, subtract 1 from the score
                    //also use loop for...
                    // CASE 2.0: if the holes are empty, check to see if they can be filled
                    for (int j = 12; j >= 7; j--) 
                    {
                        int lastOppoHole = (j + b.stonesAt(j)) % 13;
                        if (b.stonesAt(j) == 13 - j)
                        {
                            score -= 2;
                            //Counter-attack: If this opponent hole can be filled by the stones in the current hole,
                            //add to score
                            if (b.stonesAt(j) < lastHole)
                            {
                                score += 5;
                            }
                        }

                        //CASE 2.0 case check...
                        if (b.stonesAt(j) == 0)
                        {
                        if (lastHole >= j)
                            score += 5;
                        }

                        //CASE 2.1 check...
                        if (j + lastOppoHole >= i)
                        {
                            score -= 2;
                        }
                    }


                    //If the last stone is placed within the bottom...
                    if (lastHole < 6)
                        //CASE 3.0: Possible stone capture
                        if (b.stonesAt(lastHole) == 0)
                        {
                            //if the last stone is placed in an empty hole on the bottom
                            score += 1;

                            //if there are stones found in the opposite hole, add them to the score
                            if (b.stonesAt(getOpposite(lastHole)) != 0){
                                score += b.stonesAt(getOpposite(lastHole));
                            }                          
                        }
                        //CASE 3.1: Last stone placed will contribute to a possible capture from your opponent
                        else
                        {
                            //If the opposite hole is empty and not 7
                            if (b.stonesAt(getOpposite(lastHole)) == 0 && getOpposite(lastHole) != 7)
                            {
                                for (int j = b.stonesAt(getOpposite(lastHole - 1)); j >= 0; j--)
                                {   //If any of the holes leading up to the opposite empty hole have enough stones
                                    //to put one in the empty hole and capture, subtract from the score
                                    if (b.stonesAt(j) + j == b.stonesAt(getOpposite(lastHole)))
                                        score -= b.stonesAt(i);
                                }
                            }
                        }

                    
                }
            }
            return score;
        }

        //Calculate opposite hole from given hole
        public int getOpposite(int hole)
        {
            int opposite = 0;
            if (hole < 6)
            {
                int oppoDiff = 0;
                for (int i = 5; i >= 0; i--)
                {
                    if (i == hole)
                        opposite = hole + oppoDiff;
                    oppoDiff += 2;
                }
            }
            else
            {
                int oppoDiff = 12;
                for (int i = 12; i >= 7; i--)
                {
                    if (i == hole)
                        opposite = hole - oppoDiff;
                    oppoDiff -= 2;
                }
            }
            return opposite;
        }

        public String getImage() { return "Caleb.jpg"; }

    }
}
/*
 *  if (b.whoseMove() == Position.Top)
            {
                for (int i = 12; i >= 7; i--)               // try first go-again
                    if (b.stonesAt(i) == 13 - i) return i;
                for (int i = 12; i >= 7; i--)               // otherwise, first
                    if (b.stonesAt(i) > 0) return i;        // available move
            }
            else
            {
                for (int i = 5; i >= 0; i--)
                    if (b.stonesAt(i) == 6 - i) return i;
                for (int i = 5; i >= 0; i--)
                    if (b.stonesAt(i) > 0) return i;
            }
 */
