# Atividade - Desafio de Labirinto

## Descrição
Este repositório contém uma implementação em C# de um desafio de labirinto. O código aborda a movimentação dentro de um tabuleiro com validações de posições para determinar a viabilidade de um caminho. O objetivo é encontrar a saída do labirinto a partir de uma posição inicial.

## Funcionalidades Principais
- Leitura de um arquivo de entrada representando o labirinto.
- Identificação da posição inicial e saída no labirinto.
- Movimentação priorizando direções específicas.
- Tratamento de caminhos sem saída.
- Geração de um arquivo de saída indicando o caminho percorrido.

## Utilização
1. Clone este repositório localmente.
2. Compile e execute o projeto usando um ambiente de desenvolvimento compatível com C#.
3. Insira o caminho do arquivo de entrada no campo apropriado na interface gráfica.
4. Clique no botão "Run" para iniciar a busca da saída do labirinto.
5. O resultado será exibido em uma mensagem e um arquivo de saída será gerado.

## Estrutura do Código
- A lógica principal está contida no arquivo `frmAtividade.cs`.
- A classe `Possibility` e o método `MaisDeUmCaminho` são usados para identificar possíveis direções a partir de uma posição.
- A lógica de movimentação e busca da saída é implementada no método `CodigoAtividade`.

## Observações
- O código está estruturado de forma a simplificar a leitura sequencial, evitando o uso de múltiplas classes para processos menores.
- Foram adotadas práticas para garantir a clareza e simplicidade na implementação.

## Requisitos
- .NET Framework ou .NET Core instalados.

## Autor
Este código foi desenvolvido por Abimael Mendes.
