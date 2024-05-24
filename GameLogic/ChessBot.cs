using System.Diagnostics;
using System.Text;

namespace GameLogic; 
internal abstract class ChessBot {
    public virtual int[] BotMove(Board gameBoard) { return []; }
      
}

internal class StockFishToChessBot : ChessBot
{
    static string ToFEN(Board gameBoard)
    {
        StringBuilder fenBuilder = new("position fen ");
        Piece piece;
        for (ushort r = 0; r < 8; r++)
        {
            int emptyCount = 0;
            for (ushort c = 0; c < 8; c++)
            {
                Position pos = new(r, c);


                if (gameBoard.IsEmpty(pos))
                {
                    emptyCount++;
                }
                else
                {
                    if (emptyCount > 0)
                    {
                        fenBuilder.Append(emptyCount);
                        emptyCount = 0;
                    }
                    PieceType a = gameBoard[pos].Type;
                    string res = "a";
                    switch (a)
                    {
                        case PieceType.Pawn:
                            if (gameBoard[pos].Color == Player.White)
                            {
                                res = "P"; break;
                            }
                            else res = "p"; break;
                        case PieceType.Bishop:
                            if (gameBoard[pos].Color == Player.White)
                            {
                                res = "B"; break;
                            }
                            else res = "b"; break;
                        case PieceType.Knight:
                            if (gameBoard[pos].Color == Player.White)
                            {
                                res = "N"; break;
                            }
                            else res = "n"; break;
                        case PieceType.Rook:
                            if (gameBoard[pos].Color == Player.White)
                            {
                                res = "R"; break;
                            }
                            else res = "r"; break;
                        case PieceType.GunKing:
                        case PieceType.King:
                            if (gameBoard[pos].Color == Player.White)
                            {
                                res = "K"; break;
                            }
                            else res = "k"; break;
                        case PieceType.Queen:
                            if (gameBoard[pos].Color == Player.White)
                            {
                                res = "Q"; break;
                            }
                            else res = "q"; break;

                    }
                    fenBuilder.Append(res);
                }

            }
            if (emptyCount > 0)
            {
                fenBuilder.Append(emptyCount);
            }
            if (r < 7)
            {
                fenBuilder.Append('/');
            }
        }

        // Добавление очереди хода, прав на рокировку, возможностей взятия на проходе, полходов и полного количества ходов.
        fenBuilder.Append(" b");

        return fenBuilder.ToString();
    }

    public async Task<string> GetBotMove(Board gameBoard)
    {
        string stockfishPath = @"stockfish\stockfish-windows-x86-64-avx2.exe";
        if (!File.Exists(stockfishPath))
        {
            Console.WriteLine("Error: Stockfish doesn't exist.");
        }
        // Создание процесса под названием Stockfish
        var engineProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = stockfishPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        try
        {
            engineProcess.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка запуска процесса: {ex.Message}");
            engineProcess.Kill();
            return "";
        }

        var inputWriter = engineProcess.StandardInput;
        var outputReader = engineProcess.StandardOutput;

        // Инициализация движка
        await inputWriter.WriteLineAsync("uci");
        await inputWriter.WriteLineAsync("setoption name Skill Level value 20");
        //await inputWriter.WriteLineAsync("isready");
        await inputWriter.FlushAsync();

        // Ждем ответа от движка
        string output;
        while ((output = await outputReader.ReadLineAsync()) != null && output != "uciok")
        {
            Console.WriteLine(output);
        }

        // Устанавливаем начальную позицию
        await inputWriter.WriteLineAsync("ucinewgame");
        await inputWriter.WriteLineAsync(ToFEN(gameBoard));
        await inputWriter.FlushAsync();

        // Получаем лучший ход за 2 сек.
        await inputWriter.WriteLineAsync("go movetime 1000");
        await inputWriter.FlushAsync();

        // Получаем ответ от движка
        while ((output = await outputReader.ReadLineAsync()) != null && !output.StartsWith("bestmove"))
        {
            Console.WriteLine(output);
        }

        // Выводим лучший ход
        if (output != null)
        {
            string result = output.Split(' ')[1];
            return result;
        }

        // Завершаем процесс движка

        return "";
    }

    public static int[] ConvertChessNotation(string notation)
    {
        if (notation.Length != 4)
        {
            if (notation.Length != 5)
            {
                throw new ArgumentException("Notation must be exactly 4 characters long");
            }
            else notation = notation.Remove(notation.Length - 1);
        }

        int[] result = new int[4];

        result[0] = 8 - CharToInt(notation[1]);
        result[1] = CharToDigit(notation[0]) - 1;
        result[2] = 8 - CharToInt(notation[3]);
        result[3] = CharToDigit(notation[2]) - 1;

        return result;
    }

    private static int CharToDigit(char c)
    {
        // Convert columns a-h to 1-8
        if (c < 'a' || c > 'h')
        {
            throw new ArgumentException("Column letter must be between 'a' and 'h'");
        }

        return c - 'a' + 1;
    }

    private static int CharToInt(char c)
    {
        // Convert rows 1-8 to integers 1-8
        if (c < '1' || c > '8')
        {
            throw new ArgumentException("Row number must be between '1' and '8'");
        }

        return c - '0';
    }

    public override int[] BotMove(Board gameBoard) {
        int[] result = new int[4];
        var cleanupTask = Task.Run(async () => {
            string a = await this.GetBotMove(gameBoard);
            result = ConvertChessNotation(a);
        });
        cleanupTask.Wait();
        return result;
    }
}
