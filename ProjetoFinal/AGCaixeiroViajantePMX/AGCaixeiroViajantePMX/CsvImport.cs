﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using AGCaixeiroViajantePMX.Model;
using AGCaixeiroViajantePMX;

namespace AGCaixeiroViajantePMX
{
    public class CsvImport
    {
        public static List<Agencia> LerBancos()
        {
            //StreamReader sr = new StreamReader(@"c:\users\felipe.almeida1.senacedu.000\documents\visual studio 2010\Projects\ReconhecimentoZona\ReconhecimentoZona\Data\bancos_final.csv", Encoding.UTF7);
            List<Agencia> bancos = new List<Agencia>();
            string bancosString = Resources.Bancos;//sr.ReadToEnd();
            int i = 1;
            foreach (string linha in bancosString.Split('\n').Skip(1))
            {
                if (linha.Length < 10)
                    continue;

                Agencia banco = new Agencia();
                string[] colunas = linha.Replace("São Paulo - ", "").Replace("\r", "").Split('\t');
                banco.Id = i++;
                banco.Nome = colunas[0];
                banco.Longitude = Convert.ToDouble(colunas[1]);
                banco.Latitude = Convert.ToDouble(colunas[2]);
                if (Math.Abs(banco.Latitude) > 1000)
                {
                    banco.Latitude = Convert.ToDouble(colunas[1].Replace('.', ','));
                    banco.Longitude = Convert.ToDouble(colunas[2].Replace('.', ','));
                }
                banco.Zona = colunas[3];

                bancos.Add(banco);
            }

            return bancos;
        }

    }

}
