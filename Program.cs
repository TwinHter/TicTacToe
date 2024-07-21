using System.Security.Cryptography;

namespace TicTacToe
{
    class Program {
        public class GameState {
            public int[,] current_table, previous_table;
            private int gameSize;
            private bool isExit;
            public int Move {get; set;}
            private int ConvertCell(int x, int y) {
                return x * gameSize + y + 1;
            }
            public GameState(int gameSize = 3) {
                this.gameSize = gameSize;
                current_table = new int[gameSize, gameSize];
                previous_table = new int[gameSize, gameSize];
                for(int i=0; i<gameSize; i++)
                for(int j=0; j<gameSize; j++) {
                    current_table[i, j] = previous_table[i, j] = ConvertCell(i, j);
                }
                Move = 0;
                isExit = false;
            }
            public void PrintGame() {
                for(int i=0; i<gameSize; i++) {
                    for(int j=0; j<gameSize; j++) {
                        string tmp = current_table[i, j].ToString();
                        if(current_table[i, j] == -2) tmp = "O";
                        if(current_table[i, j] == -1) tmp = "X"; 
                        Console.Write($"{tmp} |");
                    }
                    Console.WriteLine();
                }
            }
            public void HelpMessage() {
                System.Console.WriteLine("This is Help Message");
            }
            public void Restart() {
                for(int i=0; i<gameSize; i++)
                for(int j=0; j<gameSize; j++) {
                    current_table[i, j] = previous_table[i, j] = ConvertCell(i, j);
                }
            }
            public void PreviousMove() {
                current_table = previous_table;
            }
            public void HintMessage() {
                System.Console.WriteLine("This is hint message");
            }
            public bool UserMove() {
                Console.WriteLine("Type your move: ");
                string inp = "";
                while(string.IsNullOrEmpty(inp)) {
                    inp = Console.ReadLine();
                }
                
                int num = 0;
                if(int.TryParse(inp, out num)) {
                    if(num > 0 && num < 9) {
                        int x = (num-1)/gameSize, y = (num-1)%gameSize;
                        if(current_table[x, y] < 0) {
                            System.Console.WriteLine("Cell have been choosen!");
                            return false;
                        }
                        else {
                            previous_table = current_table;
                            current_table[x, y] = -1;
                            return true;
                        }
                    }
                    else {
                        System.Console.WriteLine("Invalid Move");
                        return false;
                    }
                }
                else {
                    switch (inp)
                    {
                        case "previous":
                            PreviousMove();
                            break;
                        case "restart":
                            Restart();
                            break;
                        case "help":
                            HelpMessage();
                            break;
                        case "exit":
                            this.isExit = true;
                            break;
                        case "hint":
                            HintMessage();
                            break;
                        default:
                            Console.WriteLine("Invalid input");
                            break;
                    }
                    return false;
                }
            }
            private void CPUMove() {
                while(true) {
                    System.Random random = new System.Random(); 
                    int move = random.Next(0, 9) + 1;
                    int x = (move-1)/gameSize, y = (move-1)%gameSize;
                    if(current_table[x, y] > 0) {
                        previous_table = current_table;
                        current_table[x, y] = -2;
                        break;
                    }
                }
            }
            private void CPUAdvanceMove() {

            }
            public int CheckResult() {
                // Check row
                for(int i=0; i<gameSize; i++) {
                    if(current_table[i, 0] < 0) {
                        bool ok = true;
                        for(int j=0; j<gameSize; j++) {
                            if(current_table[i, j] != current_table[i, 0]) {
                                ok = false;
                                break;
                            }
                        }
                        if(ok) {
                            return (current_table[i, 0] == -1) ? 1 : 2;
                        }
                    }
                }
                // Check column
                for(int i=0; i<gameSize; i++) {
                    if(current_table[0, i] < 0) {
                        bool ok = true;
                        for(int j=0; j<gameSize; j++) {
                            if(current_table[j, i] != current_table[0, i]) {
                                ok = false;
                                break;
                            }
                        }
                        if(ok) {
                            return (current_table[i, 0] == -1) ? 1 : 2;
                        }
                    }
                }
                // Check 2 diagonal line
                bool isTopLeft = true, isTopRight = true;
                for(int i=0; i<gameSize; i++) {
                    if(current_table[i, i] != current_table[0, 0] || current_table[0, 0] > 0) isTopLeft = false;
                    if(current_table[i, gameSize-i-1] != current_table[0, gameSize-1] || current_table[0, gameSize-1] > 0) isTopRight = false;
                }

                if(isTopLeft) {
                    return (current_table[0, 0] == -1) ? 1 : 2;
                }
                if(isTopRight) {
                    return (current_table[0, gameSize-1] == -1) ? 1 : 2;
                }
                return 0;
            }
            public void ShowResult(int state) {
                if(state == 1) {
                    System.Console.WriteLine("Congratulation ! You are the winner");
                }
                if(state == 2) {
                    System.Console.WriteLine("Try your best next time.");
                }
                if(state == 3) {
                    System.Console.WriteLine("Draw !!! Another game?");
                }
            }
            public void GamePlay(int firstToMove = 1) {
                this.isExit = false;
                int current_turn = firstToMove;
                // current_turn = 1 -> UserMove, = 2 -> CPUMove
                while(true) {
                    if(this.isExit) break;
                    int result = CheckResult();
                    if(result > 0) {
                        if(result == 1) ShowResult(1); // UserWin
                        else ShowResult(2); // CPUWin
                        break;
                    }
                    if(Move == 9) {
                        ShowResult(3); // Draw
                        break;
                    }
                    if(current_turn == 1) {
                        while(true) {
                            bool isValid = UserMove();
                            if(this.isExit == true) break;
                            if(isValid) {
                                current_turn = 3 - current_turn;
                                Move += 1;
                                break;
                            }
                        }
                        if(this.isExit) break;
                    }
                    else {
                        CPUMove();
                        Move += 1;
                    }
                }
                System.Console.WriteLine("Thank you for your playing");
            }
        }  
        static void Main(string[] args) {
            System.Console.WriteLine("Hello, this is TwinHter's Tic Tac Toe");
            System.Console.WriteLine("Type in 1 if you want to play first, otherwise type in integer larger than 1");
            int num = -1; 
            while(num < 1) {
                try {
                    num = Convert.ToInt32(Console.ReadLine());
                    if(num < 1) {
                        System.Console.WriteLine("Input number must >= 1");
                    }
                }
                catch (Exception) {
                    System.Console.WriteLine("Something wrong");
                }
            }

            GameState new_game = new GameState();
            if(num == 1) new_game.GamePlay(1);
            else new_game.GamePlay(2);
        }
    }
}