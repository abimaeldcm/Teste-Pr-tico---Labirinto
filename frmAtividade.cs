using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atividade
{
    public partial class frmAtividade : Form
    {
        public frmAtividade()
        {
            InitializeComponent();
        }



        private void btnRun_Click(object sender, EventArgs e)
        {
            if (txtArquivo.Text.Trim().Equals(""))
            {
                MessageBox.Show(this, "Caminho do arquivo deve ser informado");
                txtArquivo.Focus();
                return;
            }

            if (!File.Exists(txtArquivo.Text.Trim()))
            {
                MessageBox.Show(this, "Arquivo inexistente!");
                txtArquivo.Focus();
                return;
            }

            Thread thread = new Thread(() => ExecutaAtividade(txtArquivo.Text.Trim()));
            thread.Name = "Atividade - Run";
            thread.Start();
        }


        private void ExecutaAtividade(string filePath)
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                txtArquivo.Enabled = false;
                btnRun.Enabled = false;
            }));

            try
            {
                CodigoAtividade(filePath);

                this.Invoke(new MethodInvoker(delegate ()
                {
                    MessageBox.Show(this, "Finalizado!");
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    MessageBox.Show(this, ex.Message);
                }));
            }
            finally
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    txtArquivo.Enabled = true;
                    btnRun.Enabled = true;
                }));
            }
        }


        public List<(int, int)> MaisDeUmCaminho(string[,] lista, int linha, int coluna)
        {
            List<(int, int)> possibilidades = new List<(int, int)>();
            if (linha >= 0 && lista[linha - 1, coluna] == "0") possibilidades.Add((linha - 1, coluna));
            if (coluna > 0 && (lista[linha, coluna - 1] == "0")) possibilidades.Add((linha, coluna - 1)); ;
            if (coluna + 1 < lista.GetLength(1) && lista[linha, coluna + 1] == "0") possibilidades.Add((linha, coluna + 1));
            if (linha <= (lista.GetLength(0) - 1) && lista[linha + 1, coluna] == "0") possibilidades.Add((linha + 1, coluna)); ;

            return possibilidades;
        }

        private string ObterDirecaoDoCaminho(string caminhoAtual, string proximoCaminho)
        {
            // Remova os parênteses e separe os números
            caminhoAtual = caminhoAtual.Replace("(", "").Replace(")", "");
            string[] caminhoAtualnumeros = caminhoAtual.Split(',');

            // Remova os parênteses e separe os números
            proximoCaminho = proximoCaminho.Replace("(", "").Replace(")", "");
            string[] proximoCaminhonumeros = proximoCaminho.Split(',');

            string direcao = "";

            // Verifica a direção com base nas coordenadas
            int linhaProximoCaminho = int.Parse(proximoCaminhonumeros[0]);
            int colunaProximoCaminho = int.Parse(proximoCaminhonumeros[1]);

            int linhaCaminhoAtual = int.Parse(caminhoAtualnumeros[0]);
            int colunaCaminhoAtual = int.Parse(caminhoAtualnumeros[1]);


            // Verifica a direção com base nas coordenadas
            if (linhaProximoCaminho > linhaCaminhoAtual)
            {
                direcao = "B"; // Para baixo
            }
            else if (linhaProximoCaminho < linhaCaminhoAtual)
            {
                direcao = "C"; // Para cima
            }
            else
            {
                // Mesma linha, verifica as colunas
                if (colunaProximoCaminho > colunaCaminhoAtual)
                {
                    direcao = "D"; // Para a direita
                }
                else if (colunaProximoCaminho < colunaCaminhoAtual)
                {
                    direcao = "E"; // Para a esquerda
                }
                else
                {
                    // Mesma posição, caso especial
                    direcao = "Igual"; // Indicação de posição igual
                }
            }

            return direcao;
        }

        /*
           Teste Prático de Abimael da Cruz Mendes para Desenvolvedor BackEnd Jr.

           O código aborda a movimentação dentro do tabuleiro com validações das posições para determinar a viabilidade.
           Se uma posição for válida, verifica-se se ela tem mais de uma opção. Em caso afirmativo, é marcada com "M" para múltiplas,
           e as posições válidas são armazenadas para futuros caminhos.

           Posições simples são marcadas com "P" de percorridas.

           Quando uma posição sem saída é alcançada, o código usa o histórico do percurso para retornar ao último ponto com múltiplas opções,
           explorando um novo caminho até encontrar a saída.

           Observações:
           Optou-se por reduzir a quantidade de classes para leitura sequencial do código, criando classes apenas para processos maiores
           e frequentes no código.
        */

        private void CodigoAtividade(string filePath)
        {
            // Lê o arquivo de entrada e o converte num array de string
            string[] lines = File.ReadAllLines(filePath);

            // Lê a primeira linha, quebrando ela em informações separadas pelo espaço
            string[] firstLine = lines[0].Split(' ');

            // Primeira informação é a quantidade de linhas
            int linhas = Convert.ToInt32(firstLine[0]);
            // Segunda informação é a quantidade de colunas
            int colunas = Convert.ToInt32(firstLine[1]);

            // Preenche matriz do labirinto
            string[,] matriz = new string[linhas, colunas];
            int lAtual = -1; // Posição inicial: linha
            int cAtual = -1; // Posição inicial: coluna
            int lSaida = -1; // Saída: linha
            int cSaida = -1; // Saída: coluna

            // percorre toda a matriz (a partir da segunda linha do arquivo texto) para identificar a posição inicial e a saída
            for (int l = 1; l < lines.Length; l++)
            {
                string[] line = lines[l].Split(' ');
                for (int c = 0; c < line.Length; c++)
                {
                    string ll = line[c];
                    matriz[l - 1, c] = ll;

                    if (ll.Equals("X"))
                    {
                        // Posição inicial
                        lAtual = l - 1;
                        cAtual = c;
                    }
                    else if (ll.Equals("0") && (l == 1 || c == 0 || l == lines.Length - 1 || c == line.Length - 1))
                    {
                        // Saída
                        lSaida = l - 1;
                        cSaida = c;
                    }
                }
            }

            // Posição máxima de linha e coluna que pode ser movida (borda)
            int extremidadeLinha = linhas - 1;
            int extremidadeColuna = colunas - 1;

            // Guarda o trajeto em uma list de string e já inicia com a posição de origem
            List<string> resultado = new List<string>();
            resultado.Add("O [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");


            // Percorre a matriz (labirinto) até encontrar a saída, usando as regras de prioridade e posições não visitadas, e vai armazenando o trajeto na list resultado
            bool achouSaida = lAtual == lSaida && cAtual == cSaida;

            // Lista para armazenar as posições visitadas
            List<(int, int)> posicoesVisitadas = new List<(int, int)>();
            Dictionary<(int, int), List<(int, int)>> maisDeUmaOpcao = new Dictionary<(int, int), List<(int, int)>>();

            while (!achouSaida)
            {
                if (lAtual == lSaida && cAtual == cSaida)
                {
                    break;
                }
                // Verifica se há mais de uma opção de caminho a partir da posição atual
                var maisDeUmCaminho = MaisDeUmCaminho(matriz, lAtual, cAtual);
                if (maisDeUmCaminho.Count >= 2 && !maisDeUmaOpcao.ContainsKey((lAtual, cAtual)))
                {
                    // Se houver mais de uma opção e ainda não foi registrada, adiciona ao dicionário
                    maisDeUmaOpcao.Add((lAtual, cAtual), maisDeUmCaminho);
                    matriz[lAtual, cAtual] = "M";  // Marca a posição como mais de uma possibilidade
                }

                // Movimento prioritário para cima (C)
                if (lAtual > 0 && matriz[lAtual - 1, cAtual] == "0")
                {
                    if (!maisDeUmaOpcao.ContainsKey((lAtual, cAtual)))
                    {
                        matriz[lAtual, cAtual] = "P";  // Marca a posição como visitada se não houver opções adicionais
                    }

                    posicoesVisitadas.Add((lAtual, cAtual));  // Adiciona a posição atual às posições visitadas
                    lAtual--;
                    resultado.Add("C [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");  // Adiciona movimento ao resultado que serão salvos posteriormente
                }
                // Movimento prioritário para a esquerda (E)
                else if (cAtual > 0 && matriz[lAtual, cAtual - 1] == "0")
                {
                    // Verifica se a posição atual ainda não foi registrada no dicionário maisDeUmaOpcao
                    if (!maisDeUmaOpcao.ContainsKey((lAtual, cAtual)))
                    {
                        matriz[lAtual, cAtual] = "P";  // Marca a posição como visitada se não houver opções adicionais
                    }

                    posicoesVisitadas.Add((lAtual, cAtual));  // Adiciona a posição atual às posições visitadas
                    cAtual--;
                    resultado.Add("E [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");  // Adiciona movimento ao resultado
                }
                // Movimento prioritário para a direita (D)
                else if (cAtual < extremidadeColuna && matriz[lAtual, cAtual + 1] == "0")
                {
                    // Verifica se a posição atual ainda não foi registrada no dicionário maisDeUmaOpcao
                    if (!maisDeUmaOpcao.ContainsKey((lAtual, cAtual)))
                    {
                        matriz[lAtual, cAtual] = "P";  // Marca a posição como visitada se não houver opções adicionais
                    }

                    posicoesVisitadas.Add((lAtual, cAtual));  // Adiciona a posição atual às posições visitadas
                    cAtual++;
                    resultado.Add("D [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");  // Adiciona movimento ao resultado
                }
                // Movimento prioritário para baixo (B)
                else if (lAtual < extremidadeLinha && matriz[lAtual + 1, cAtual] == "0")
                {
                    // Verifica se a posição atual ainda não foi registrada no dicionário maisDeUmaOpcao
                    if (!maisDeUmaOpcao.ContainsKey((lAtual, cAtual)))
                    {
                        matriz[lAtual, cAtual] = "P";  // Marca a posição como visitada se não houver opções adicionais
                    }

                    posicoesVisitadas.Add((lAtual, cAtual));  // Adiciona a posição atual às posições visitadas
                    lAtual++;
                    resultado.Add("B [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");  // Adiciona movimento ao resultado
                }

                //Caso nenhum das posições acima seja válida, o código vai identificar que é um caminho sem saída.

                else
                {
                    // Verifica se a posição atual ainda não foi registrada no dicionário maisDeUmaOpcao
                    if (!maisDeUmaOpcao.ContainsKey((lAtual, cAtual)))
                    {
                        matriz[lAtual, cAtual] = "P"; // Marca a posição como visitada se não houver opções adicionais
                    }

                    //A variavel vai ser necessária para realizar a voltar para o último caminho com mais de uma opção.
                    int contador = posicoesVisitadas.Count;
                    bool FinalizarLacoWhile = true;
                    while (FinalizarLacoWhile)
                    {
                        // Enquanto a posição atual for diferente da última posição com mais de uma possibilidade.
                        while ((lAtual, cAtual) != maisDeUmaOpcao.Keys.Last())
                        {
                            // Obtém a direção que será salva na lista de resultados.
                            string direcaoCEDB = ObterDirecaoDoCaminho((lAtual, cAtual).ToString(), posicoesVisitadas[contador - 1].ToString());

                            posicoesVisitadas.Add((lAtual, cAtual));
                            (lAtual, cAtual) = posicoesVisitadas[contador - 1];
                            contador--;
                            resultado.Add($"{direcaoCEDB} [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");
                        }

                        if (maisDeUmaOpcao.ContainsKey((lAtual, cAtual)))
                        {
                            var opcoes = maisDeUmaOpcao[(lAtual, cAtual)];

                            foreach (var opcao in opcoes)
                            {
                                // Verifica se a próxima posição é uma saída
                                if (matriz[opcao.Item1, opcao.Item2] == "0")
                                {
                                    posicoesVisitadas.Add((lAtual, cAtual));

                                    // Obtém a direção para a próxima posição e a atualiza
                                    string direcao = ObterDirecaoDoCaminho((lAtual, cAtual).ToString(), (opcao.Item1, opcao.Item2).ToString());
                                    (lAtual, cAtual) = (opcao.Item1, opcao.Item2);
                                    resultado.Add($"{direcao} [" + (lAtual + 1) + ", " + (cAtual + 1) + "]");
                                    FinalizarLacoWhile = false;
                                    break;
                                }
                            }

                            // Se a posição atual for a saída, encerra o loop principal
                            if (lAtual == lSaida && cAtual == cSaida)
                            {
                                achouSaida = true;
                                break;
                            }

                            // Remove a última chave do dicionário maisDeUmaOpcao
                            if (maisDeUmaOpcao.Count > 0)
                            {
                                var ultimaChave = maisDeUmaOpcao.Keys.Last();
                                maisDeUmaOpcao.Remove(ultimaChave);
                            }
                        }
                    }
                }
            }

            // Salva arquivo texto de saída com o trajeto
            string folderPath = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            File.WriteAllLines(Path.Combine(folderPath, "saida-" + fileName + ".txt"), resultado.ToArray(), Encoding.UTF8);
        }
    }
}
