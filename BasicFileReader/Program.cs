using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Program
{
    static string filePath = "Samples/dados.txt";
    static string logPath = "Samples/log.txt";

    static void Main()
    {
        Console.WriteLine("=== Comparação: Acesso Sequencial vs Direto ===\n");

        CriarArquivoSeNaoExistir();

        // Acesso Sequencial
        var tempoSequencial = AcessoSequencial();

        // Acesso Direto
        Console.Write("\nDigite o número da linha para acesso direto: ");
        int linha = int.Parse(Console.ReadLine());

        var tempoDireto = AcessoDireto(linha);

        // Log final
        RegistrarLog($"\nResumo:");
        RegistrarLog($"Tempo Sequencial: {tempoSequencial} ms");
        RegistrarLog($"Tempo Direto: {tempoDireto} ms");

        Console.WriteLine("\nExecução finalizada. Veja o log.txt");
    }

    static void CriarArquivoSeNaoExistir()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Criando arquivo com dados...");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 1; i <= 100000; i++)
                {
                    writer.WriteLine($"Linha {i}");
                }
            }

            Console.WriteLine("Arquivo criado com 100.000 linhas.\n");
        }
    }

    static long AcessoSequencial()
    {
        Console.WriteLine("Executando acesso sequencial...");

        Stopwatch sw = Stopwatch.StartNew();

        int count = 0;
        foreach (var linha in File.ReadLines(filePath))
        {
            count++;
        }

        sw.Stop();

        RegistrarLog($"[Sequencial] Linhas lidas: {count}");
        RegistrarLog($"[Sequencial] Tempo: {sw.ElapsedMilliseconds} ms");

        return sw.ElapsedMilliseconds;
    }

    static long AcessoDireto(int numeroLinha)
    {
        Console.WriteLine("\nExecutando acesso direto...");

        Stopwatch sw = Stopwatch.StartNew();

        string linha = File.ReadLines(filePath)
                           .Skip(numeroLinha - 1)
                           .FirstOrDefault();

        sw.Stop();

        RegistrarLog($"[Direto] Linha solicitada: {numeroLinha}");
        RegistrarLog($"[Direto] Conteúdo: {linha}");
        RegistrarLog($"[Direto] Tempo: {sw.ElapsedMilliseconds} ms");

        return sw.ElapsedMilliseconds;
    }

    static void RegistrarLog(string mensagem)
    {
        Console.WriteLine(mensagem);
        File.AppendAllText(logPath, mensagem + Environment.NewLine);
    }
}