using System;
using static System.Console;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;


public enum Situacao
{
    navegando, pesquisando, incluindo, editando, excluindo
}
class VetorPalavra<Registro>: IVetorDados<Registro> where
        Registro : IComparable<Registro>, IRegistro, new()
{
    int tamanhoMaximo;     //tamanho físico do vetorPalavra
        int qtsDados;      //tamanho lógico do vetorPalavra
        Situacao situacaoAtual; 
        int posicaoAtual;  //posição atual no vetorPalavra
        private Registro[] palavra; //vetor dados do objeto genérico registro

        public VetorPalavra(int tamanhoDesejado)
        {
            palavra = new Registro[tamanhoDesejado]; // instanciamos o vetor
            qtsDados = 0;
            tamanhoMaximo = tamanhoDesejado;
        }

        public void LerUmRegistro(string nomeArq)   // ler de um arquivo texto
        {
           if (!File.Exists(nomeArq))   // se o arquivo não existe
           {
               var arqNovo = File.CreateText(nomeArq);  // criamos o arquivo vazio
               arqNovo.Close();
           }
          var arq = new StreamReader(nomeArq, System.Text.Encoding.UTF7);
          qtsDados = 0;
            while (!arq.EndOfStream)
            {
                 var umRegistro = new Registro(); // o objeto PalavraEDica da classe PalavraDica, passa a linha lida como parâmetro
                 umRegistro.LerRegistro(arq);
                 Incluir(umRegistro);                                                  // e dessa linha, na classe, serão guardados a palavra e a dica
            }
            arq.Close();
        }
        public void InserirAposFim(Registro valorAInserir) // o valor a ser inserido, será um objeto da classe PalavraDica
        {
            if (qtsDados >= tamanhoMaximo)
                ExpandirVetor();

            palavra[qtsDados] = valorAInserir;
            qtsDados++;
        }
        private void ExpandirVetor()
        {
            tamanhoMaximo += 10;
            Registro[] vetorMaior = new Registro[tamanhoMaximo];
            for (int indice = 0; indice < qtsDados; indice++)
                vetorMaior[indice] = palavra[indice];

            palavra = vetorMaior;
        }

        public void Excluir(int posicaoAExcluir)
        {
            qtsDados--;
            for (int indice = posicaoAExcluir; indice < qtsDados; indice++)
                palavra[indice] = palavra[indice + 1];

        }

        public void GravarDados(string nomeArquivo)
        {
            var arquivo = new StreamWriter(nomeArquivo);             //abre arquivo para escrita
            for (int indice = 0; indice < qtsDados; indice++)        //percorre elementos do vetor
                arquivo.WriteLine(palavra[indice].ParaArquivo());    //grava cada elemento
            arquivo.Close();
        }
        public override string ToString()  //retorna lista de valores separados por 
        {                                  //espaço
            return ToString(" ");
        }

        public string ToString(string separador) //retorna lista de valores separados 
        {                                        //por separador
            string resultado = "";
            for (int indice = 0; indice < qtsDados; indice++)
                resultado += palavra[indice] + separador;
            return resultado;
        }

    public int Tamanho  //permite à aplicação consultar o número de registros armazenados
    {
        get => qtsDados;
    }
    public int PosicaoAtual //retorna qual indice está sendo exibido na tela de manutenção
    {
        get => posicaoAtual;
        set
        {
            if (value >= 0 && value < qtsDados)
                posicaoAtual = value;
        }
    }
    public Registro this[int indice] //retorna o valor do vetor no registro passado como parâmetro
    {
        get
        {
            if (indice < 0 || indice >= qtsDados)  //inválido
                throw new Exception("Índice inválido!");

            return palavra[indice];
        }
        set
        {
            if (indice < 0 || indice >= qtsDados)
                throw new Exception("Índice fora dos limites do vetor!");

            palavra[indice] = value;
        }
    }
    public void Ordenar()
    {
        for (int lento = 0; lento < qtsDados; lento++)
        {
            int indiceDoMenor = lento;
            for (int rapido = lento + 1; rapido < qtsDados; rapido++)
                if (palavra[rapido].CompareTo(palavra[indiceDoMenor]) < 0)
                    indiceDoMenor = rapido;
            if (indiceDoMenor != lento)
            {
                Registro aux = palavra[lento];
                palavra[lento] = palavra[indiceDoMenor];
                palavra[indiceDoMenor] = aux;
            }
        }
    }

    public void Incluir(Registro valorAInserir) // insere após o final do vetor e o expande se necessário
    {
        if (qtsDados >= palavra.Length)
            ExpandirVetor();

        palavra[qtsDados] = valorAInserir;
        qtsDados++;
    }

    //insere o novo dado na posição indicada por ondeIncluir
    public void Incluir(Registro valorAInserir, int ondeIncluir) //inclui o objeto passado como parâmetro na posição desejada
    {
        if (qtsDados >= palavra.Length)
            ExpandirVetor();

        //desloca para frente os palavra posteriores ao novo dado
        for (int indice = qtsDados - 1; indice >= ondeIncluir; indice--)
            palavra[indice + 1] = palavra[indice];

        palavra[ondeIncluir] = valorAInserir;
        qtsDados++;
    }

    public bool Existe(Registro procurado, ref int onde) //Vê se existe o objeto passado como parâmetro e devolve o índice em que ele foi encontrado
    {
        bool achou = false;
        int inicio = 0;
        int fim = qtsDados - 1;
        while (!achou && inicio <= fim)
        {
            onde = (inicio + fim) / 2;
            if (palavra[onde].CompareTo(procurado) == 0)
                achou = true;
            else
              if (procurado.CompareTo(palavra[onde]) < 0)
                fim = onde - 1;
            else
                inicio = onde + 1;
        }
        if (!achou)
            onde = inicio; //onde deverá ser incluído o novo registro caso não tenha sido achado
        return achou;
    }

    public void ExibirDados()
    {
        for (int indice = 0; indice < qtsDados; indice++)
            WriteLine($"{palavra[indice],5} ");
    }   

    public void ExibirDados(DataGridView lista)
    {
    }
    public Situacao SituacaoAtual // atributo público para a situação atual
    {
        get => situacaoAtual;
        set => situacaoAtual = value;
    }
    public bool EstaVazio //permite à aplicação saber se o vetor palavra está vazio
    {
        get => qtsDados <= 0; //se qtosDados <= 0, retorna true
    }

    public void PosicionarNoPrimeiro() //determina a poisção atual no primeiro
    {
        if (!EstaVazio)
            posicaoAtual = 0; //primeiro elemento do vetor
        else
            posicaoAtual = -1; //antes do início do vetor
    }
    public void PosicionarNoUltimo() //determina a poisção atual no último
    {
        if (!EstaVazio)
            posicaoAtual = qtsDados - 1; //última posição usada do vetor
        else
            posicaoAtual = -1; //indica antes do vetor vazio
    }
    public void AvancarPosicao() //avança 1 na posição atual
    {
        if (!EstaNoFim)
            posicaoAtual++;
    }
    public void RetrocederPosicao() //subtrai 1 da posição atual
    {
        if (!EstaNoInicio)
            posicaoAtual--;
    }
    public bool EstaNoInicio //retorna true se a posição atual estiver no primeiro índice
    {
        get => posicaoAtual <= 0; //primeiro índice
    }
    public bool EstaNoFim //retorna true se a posição atual estiver no último índice
    {
        get => posicaoAtual >= qtsDados - 1; //último índice
    }


}
