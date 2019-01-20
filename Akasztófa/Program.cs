using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;

namespace Akasztófa
{
    class Program
    {
        static Random random = new Random();
        static string word;
        static int fails = 0;
        static bool gameover = false;
        static string[] solution;
        static List<string> fail = new List<string>(); 


        static List<string> getWords(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            List<string> words = new List<string>();

            while(!sr.EndOfStream)
            {
                words.Add(sr.ReadLine());
            }
            sr.Close();
            return words;
        }

        static void pickWord(List<string> words)
        {
            word = words[random.Next(0, words.Count)];
            solution = new string[word.Length];

            for (int i = 0; i < solution.Length; i++)
            {
                solution[i] = "_";
            }
        }

        static void gameLoop()
        {
            Clear();

            WriteLine("Tries left: {0}", 12-fails);

            //Print current solution
            foreach(var c in solution)
            {
                Write(c + " ");
            }
            Write("\n");

            //TODO: print fancy ascii graphics

            //Check if player won
            bool win = true;
            for (int i = 0; i < word.Length; i++)
            {
                if (solution[i].CompareTo("_") == 0)
                {
                    win = false;
                    break;
                }
            }
            if (win)
            {
                WriteLine("You won!");
                gameover = win;
                return;
            }

            //check if player failed
            if (fails == 12)
            {
                WriteLine("You lost...");
                gameover = true;
            }

            //print previous failed attempts
            Write("\nThese didn't work: ");
            foreach (var c in fail)
            {
                Write(c + " ");
            }

            //get guess
            Write("\nEnter your guess: ");
            string guess = Console.ReadLine();
            
            //loop until guess matches rquirements
            while (((guess.Length != 1 && guess.Length != word.Length) || (fail.Contains(guess) || solution.Contains(guess))) && guess.CompareTo("exit") != 0)
            {
                if (fail.Contains(guess)) { Write("That already didn't work\nTrysomething else: "); }
                else if (solution.Contains(guess)) { Write("You already got that\nTry something else: "); }
                else { Write("Please enter only one character, or the whole word\nTry agin: "); }
                guess = Console.ReadLine();
            }
            //if the word contains the letter, add it to the solution
            if (guess.Length == 1)
            {
                if (word.Contains(guess))
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (guess.CompareTo(Convert.ToString(word[i])) == 0)
                        {
                            solution[i] = guess;
                        }
                    }
                }
                //else add it to failed tries list
                else
                {
                    fail.Add(guess);
                    fails++;
                }
            }
            //if player entered the whole word at once, check if it's correct
            if (guess.CompareTo(word) == 0)
            {
                WriteLine("You won!");
                gameover = true;
            }
            
            if (guess.CompareTo("exit") == 0) { gameover = true; }
            
            if (!gameover) { gameLoop(); }
        }

        static void Main(string[] args)
        {
            List<string> words = getWords("words.txt");
            pickWord(words);
            gameLoop();
        }
    }
}
