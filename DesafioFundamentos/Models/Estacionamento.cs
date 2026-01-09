using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal precoInicial = 0;
        private decimal precoPorHora = 0;
        private List<string> veiculos = new List<string>();

        private const string arquivoBanco = "banco_veiculos.txt";

        // Define os preços assim que o sistema inicia
        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;

            CarregarVeiculos();
        }

        public void AdicionarVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para estacionar:");
            string placa = Console.ReadLine().ToUpper(); // Joga tudo pra maiúsculo

            // Validação de Placa (Regex) - Aceita ABC-1234 ou ABC1234
            if (ValidarPlaca(placa))
            {
                // Verifica se já não existe
                if (veiculos.Any(x => x.ToUpper() == placa.ToUpper()))
                {
                    MensagemErro("Desculpe, esse veículo já está estacionado aqui.");
                }
                else
                {
                    veiculos.Add(placa);
                    SalvarVeiculos();
                    MensagemSucesso($"Veículo {placa} estacionado com sucesso!");
                }
            }
            else
            {
                MensagemErro("Erro: Placa inválida! Use o formato ABC-1234 ou ABC1D23.");
            }
        }

        public void RemoverVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para remover:");
            string placa = Console.ReadLine().ToUpper();

            // Verifica se o veículo existe na lista
            if (veiculos.Any(x => x.ToUpper() == placa.ToUpper()))
            {
                Console.WriteLine("Digite a quantidade de horas que o veículo permaneceu estacionado:");

                // Tratamento de erro caso a pessoa digite letras nas horas
                if (int.TryParse(Console.ReadLine(), out int horas))
                {
                    decimal valorTotal = precoInicial + (precoPorHora * horas);

                    // Remove da lista
                    veiculos.Remove(placa);
                    SalvarVeiculos(); // Atualiza o arquivo

                    MensagemSucesso($"O veículo {placa} foi removido e o preço total foi de: R$ {valorTotal}");

                    // Recibo Fiscal
                    GerarRecibo(placa, horas, valorTotal);
                }
                else
                {
                    MensagemErro("Valor inválido para horas. Operação cancelada.");
                }
            }
            else
            {
                MensagemErro("Desculpe, esse veículo não está estacionado aqui. Confira se digitou a placa corretamente");
            }
        }

        public void ListarVeiculos()
        {
            // Verifica se há veículos no estacionamento
            if (veiculos.Any())
            {
                MensagemDestaque("Os veículos estacionados são:");

                // Laço FOREACH
                foreach (string v in veiculos)
                {
                    MensagemDestaque($" - {v}");
                }
            }
            else
            {
                MensagemErro("Não há veículos estacionados.");
            }
        }

        // --- MÉTODOS DE PERSISTÊNCIA ---
        private void SalvarVeiculos()
        {
            try
            {
                File.WriteAllLines(arquivoBanco, veiculos);
            }
            catch (Exception ex)
            {
                MensagemErro($"Erro ao salvar dados: {ex.Message}");
            }
        }

        private void CarregarVeiculos()
        {
            if (File.Exists(arquivoBanco))
            {
                try
                {
                    string[] placasSalvas = File.ReadAllLines(arquivoBanco);
                    veiculos.AddRange(placasSalvas);
                    MensagemSucesso($"[Sistema]: {placasSalvas.Length} veículo(s) carregado(s) do banco de dados.");
                }
                catch (Exception ex)
                {
                    MensagemErro($"Erro ao carregar dados: {ex.Message}");
                }
            }
        }

        // --- MÉTODOS AUXILIARES (PRIVADOS) ---

        private bool ValidarPlaca(string placa)
        {
            // Regex simples para padrão antigo ou Mercosul
            string padrao = @"^[a-zA-Z]{3}-?[0-9][a-zA-Z0-9][0-9]{2}$";
            return Regex.IsMatch(placa, padrao);
        }

        private void GerarRecibo(string placa, int horas, decimal valor)
        {
            try
            {
                string nomeArquivo = $"Recibo_{placa}_{DateTime.Now:yyyyMMddHHmmss}.txt";
                string conteudo = $"=== RECIBO ESTACIONAMENTO NYX ===\n" +
                                  $"Data: {DateTime.Now}\n" +
                                  $"Placa: {placa}\n" +
                                  $"Preço Fixo: R$ {precoInicial:F2}\n" +
                                  $"Preço por Hora: R$ {precoPorHora:F2}\n" +
                                  $"Tempo: {horas} horas\n" +
                                  $"--------------------------------\n" +
                                  $"TOTAL: R$ {valor:F2}\n" +
                                  $"================================";

                File.WriteAllText(nomeArquivo, conteudo);
                MensagemSucesso($"[Sistema]: Recibo gerado em '{nomeArquivo}'");
            }
            catch (Exception ex)
            {
                MensagemErro($"Erro ao gerar recibo: {ex.Message}");
            }
        }

        // --- MÉTODOS DE ESTILO ---

        private void MensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }

        private void MensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }

        private void MensagemDestaque(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }

    }
}