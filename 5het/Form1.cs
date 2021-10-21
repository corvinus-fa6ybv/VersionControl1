﻿using _5het.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5het
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();
        List<Tick> Ticks;
        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;
            CreatePortfolio();


            //var kiszamítása

            List<decimal> Nyereségek = new List<decimal>();
            int intervalum = 30;
            DateTime kezdoDatum = (from x in Ticks
                                   select x.TradingDay).Min();

            DateTime zaroDatum = new DateTime(2016, 12, 30);

            TimeSpan z = zaroDatum - kezdoDatum;

            for (int i = 0; i < z.Days-intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdoDatum.AddDays(i + intervalum))
                               - GetPortfolioValue(kezdoDatum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + "" + ny);

            }

            var nyeresegekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x).ToList();
            MessageBox.Show(nyeresegekRendezve[nyeresegekRendezve.Count() / 5].ToString());
        }

        private void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem()
            {
                Index = "OTP", Volume = 10
            });
            Portfolio.Add(new PortfolioItem()
            {
                Index = "ZWACK",
                Volume = 10
            });
            Portfolio.Add(new PortfolioItem()
            {
                Index = "ELMU",
                Volume = 10
            });
            dataGridView2.DataSource = Portfolio;
        }

        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                            && date <= x.TradingDay
                            select x
                            ).First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }
    }
}
