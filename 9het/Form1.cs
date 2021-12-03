using _9het.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _9het
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        List<int> nok = new List<int>();
        List<int> ferfiak = new List<int>();

        Random rng = new Random(1234);
        int zaroev;

        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Minimum = 2005;
            numericUpDown1.Maximum = 2025;

        }

        public void Simulation()
        {
            richTextBox1.Clear();
            nok.Clear();
            ferfiak.Clear();
            zaroev = int.Parse(numericUpDown1.Value.ToString());

            Population = GetPopulation(textBox1.Text);
            BirthProbabilities = GetBirthProbabilities(@"C:\Windows\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Windows\Temp\halál.csv");


            for (int year = 2005; year <= zaroev; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                
                nok.Add(nbrOfFemales);
                ferfiak.Add(nbrOfMales);


                //    Console.WriteLine(
                //        string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
                //
                DisplayResult(year, nbrOfFemales, nbrOfMales);
            }
           
        }

        private void DisplayResult(int year, int nbrOffFemales, int nbrOfMales)
        {

            richTextBox1.AppendText("\n"+"Szimulációs év: " + year.ToString() + "\n\t"+  "Lányok: " + nbrOffFemales.ToString() + "\n\t"+ "Fiúk: " + nbrOfMales.ToString() +"\n");
            
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> population = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new BirthProbability()
                    {
                        Age = byte.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2]),
                    });
                }
            }

            return population;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> population = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new DeathProbability()
                    {
                        Age = byte.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        P = double.Parse(line[2]),
                    });
                }
            }

            return population;
        }

        private void SimStep(int year, Person perosn)
        {
            if (perosn.IsAlive == false) return;

            byte age = (byte)(year - perosn.BirthYear);

            double pD = (from x in DeathProbabilities
                         where x.Gender == perosn.Gender && x.Age == age
                         select x.P).FirstOrDefault();

            if (rng.NextDouble() <= pD) perosn.IsAlive = false;

            if (perosn.IsAlive && perosn.Gender == Gender.Female)
            {
                double pB = (from x in BirthProbabilities
                             where x.Age == age
                             select x.P).FirstOrDefault();

                if (rng.NextDouble() <= pB)
                {
                    Person p = new Person();
                    p.BirthYear = year;
                    p.NbrOfChildren = 0;
                    p.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(p);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Simulation();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Windows\Temp\";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }




        }
    }
}
