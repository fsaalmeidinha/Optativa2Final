using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Model;

namespace Utils
{
    public class CsvImport
    {
        public static List<Agencia> LerBancos()
        {
            List<Agencia> agencias = new List<Agencia>();
            string bancosString = Resources.Agencias;
            int i = 1;
            foreach (string linha in bancosString.Split('\n').Skip(1))
            {
                if (linha.Length < 10)
                    continue;

                Agencia banco = new Agencia();
                string[] colunas = linha.Replace("São Paulo - ", "").Replace("\r", "").Split('\t');
                banco.Id = i++;
                banco.Nome = colunas[0];
                banco.Latitude = Convert.ToDouble(colunas[2]);
                banco.Longitude = Convert.ToDouble(colunas[1]);
                if (Math.Abs(banco.Latitude) > 1000)
                {
                    banco.Latitude = Convert.ToDouble(colunas[2].Replace('.', ','));
                    banco.Longitude = Convert.ToDouble(colunas[1].Replace('.', ','));
                }
                banco.Zona = colunas[3];

                agencias.Add(banco);
            }

            return agencias;
        }
    }
}
