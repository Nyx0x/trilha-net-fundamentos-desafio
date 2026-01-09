using DesafioFundamentos.Models;


Console.OutputEncoding = System.Text.Encoding.UTF8;

decimal precoInicial = 0;
decimal precoPorHora = 0;

Console.WriteLine("Seja bem vindo ao sistema de estacionamento!");

// Blindagem de erro
while (true)
{
    Console.WriteLine("Digite o preço inicial:");
    string input = Console.ReadLine();
    if (decimal.TryParse(input, out precoInicial))
    {
        break;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Erro: Digite apenas números válidos (ex: 5 ou 5,50).");
        Console.ResetColor();
    }
}

// Blindagem de erro
while (true)
{
    Console.WriteLine("Agora digite o preço por hora:");
    string input = Console.ReadLine();

    if (decimal.TryParse(input, out precoPorHora))
    {
        break;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Erro: Digite apenas números válidos (ex: 5 ou 5,50).");
        Console.ResetColor();
    }
}


// Instancia a classe Estacionamento, já com os valores
Estacionamento es = new Estacionamento(precoInicial, precoPorHora);

string opcao = string.Empty;
bool exibirMenu = true;

// Loop principal
while (exibirMenu)
{
    Console.Clear();
    Console.WriteLine("Digite a sua opção:");
    Console.WriteLine("1 - Cadastrar veículo");
    Console.WriteLine("2 - Remover veículo");
    Console.WriteLine("3 - Listar veículos");
    Console.WriteLine("4 - Encerrar");

    switch (Console.ReadLine())
    {
        case "1":
            es.AdicionarVeiculo();
            break;

        case "2":
            es.RemoverVeiculo();
            break;

        case "3":
            es.ListarVeiculos();
            break;

        case "4":
            // Lógica de confirmação de saída
            Console.WriteLine("Tem certeza que deseja encerrar o programa? (s/n)");
            string confirmacao = Console.ReadLine().ToLower();

            if (confirmacao == "s")
            {
                exibirMenu = false;
            }
            else
            {
                Console.WriteLine("Operação cancelada. Retornando ao menu principal...");
            }
            break;

        default:
            Console.WriteLine("Opção inválida");
            break;
    }
    if (exibirMenu)
    {
        Console.WriteLine("\nPressione Enter para continuar");
        Console.ReadLine();
    }
}

Console.WriteLine("O programa se encerrou. Obrigada pela preferência do Sistema Nyxys!");