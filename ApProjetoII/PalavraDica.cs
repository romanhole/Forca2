// Gabriel Alves de Arruda 19170    
// Angelo Gomes Pescarini 19161

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApProjetoII
{
    class PalavraDica : IComparable<PalavraDica>, IRegistro 
    {
        string palavraUsada;
        string dicaUsada;


        const int tamanhoPalavra = 15;
        const int tamanhoDica = 100;

        const int inicioPalavra = 0;
        const int inicioDica = inicioPalavra + tamanhoPalavra;

        public PalavraDica(string palavra, string dica) //são lidos e divididos em strings a palavra e sua respectiva dica
        {
            palavraUsada = palavra;
            dicaUsada = dica;
        }

        public PalavraDica() //construtor vazio, por causa da interface IRegistro
        {

        }

        public string PalavraUsada { get => palavraUsada; set => palavraUsada = value; } //acessam as palavras utilizadas
        public string DicaUsada { get => dicaUsada; set => dicaUsada = value; }  //Protegem as strings
                                                                                
        public void LerRegistro(StreamReader arq) //le o registro de um arquivo passado como parâmetro
        {
            if (!arq.EndOfStream) //se não chegou ao fim do arquivo
            {
                String linha = arq.ReadLine(); 
                palavraUsada = linha.Substring(inicioPalavra, tamanhoPalavra); //divide a linha em strings palavraUsada e dicaUsada
                dicaUsada = linha.Substring(inicioDica, tamanhoDica);
            }
        }       

        public String ParaArquivo()
        {
            return palavraUsada.PadRight(15, ' ') + dicaUsada.PadRight(100,' ');
        }

        public int CompareTo(PalavraDica outra)
        {
            return palavraUsada.Trim().CompareTo(outra.palavraUsada.Trim());
        }
    }
}
