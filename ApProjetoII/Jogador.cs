using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApProjetoII
{
    class Jogador
    {
        private int ptsJogador;
        private string nomeJogador;

        public Jogador(string nome, int pts) // construtor da classe jogador que recebu como parâmetro o nome e a pontuação do jogador
        {
            nomeJogador = nome; // atribuimos o nome ao nomeJogador
            ptsJogador = pts; // atribuímos a pontação ao pontosJogador
        }

        public int PtsJogador { get => ptsJogador; set => ptsJogador = value; }  
        
        // encapsulamos para que possamos acessar seus valores fora da classe
        public string NomeJogador { get => nomeJogador; set => nomeJogador = value; }
    }
}
