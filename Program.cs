using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ellenallaszh
{
    class Program
    {
        static public Random rnd = new Random();
        static public List<double> lista = new List<double>();
        static public List<string> anyagok = new List<string>();
        static public int counter = 60;
        static public int eddigVolt = 0;
      

        static void Main(string[] args)
        {
            Termelo t1 = new Termelo(3, 200);
            Termelo t2 = new Termelo(5, 100);
            Termelo t3 = new Termelo(7, 100);

            Thread szal1 = new Thread(t1.Termel);
            Thread szal2 = new Thread(t2.Termel);
            Thread szal3 = new Thread(t3.Termel);
            szal1.Start();
            szal2.Start();
            szal3.Start();

            Fogyaszto f1 = new Fogyaszto(ConsoleColor.Red);
            Fogyaszto f2 = new Fogyaszto(ConsoleColor.Yellow);
            Fogyaszto f3 = new Fogyaszto(ConsoleColor.Green);

            Thread szal4 = new Thread(f1.Fogyaszt);
            Thread szal5 = new Thread(f2.Fogyaszt);
            Thread szal6 = new Thread(f3.Fogyaszt);
            szal4.Start();

            szal5.Start();
            szal6.Start();
            szal4.Join();
            szal5.Join();
            szal6.Join();


            //Console.ReadLine();
            Console.WriteLine("A kapott ellenállások: ");
            for (int i = 0; i < anyagok.Count; i++)
            {
                Console.Write("{0}, ", anyagok[i].ToString());
            }
            Console.WriteLine("\n{0}",anyagok.Count);
            Console.ReadLine();
           
        }

         class Termelo
        {
            int db;
            public int oszthato;

            public Termelo(int n, int m)
            {
                db = m;
                oszthato = n;
            }

            public void Termel()
            {
                for (int i = 0; i < db; i++)
                {
                    Monitor.Enter(Program.lista);
                    //StreamWriter sr = new StreamWriter("bun.txt");

                    while (Program.lista.Count >= 60)
                    {
                        Monitor.Wait(Program.lista);
                    }

                    double R = Program.rnd.Next(0,100);
                    double A = Program.rnd.Next(0,10);
                    double l = Program.rnd.Next(0,250);
                    double p = (R * A) / l;
              
                    int sleep = Program.rnd.Next(1, 5);
                    Thread.Sleep(sleep);


                  
         
                    Program.lista.Add(p);
                    Monitor.Pulse(Program.lista);
                    Monitor.Exit(Program.lista);

                } 
                Console.WriteLine("A száll leállt");

                }
             
            }


         class Fogyaszto
         {
             ConsoleColor c;
             public Fogyaszto(ConsoleColor c)
             {
                 this.c = c;
             }

             public void Fogyaszt()
             {

                 while (Program.counter > Program.eddigVolt)
                 {
                     Program.eddigVolt++;
                     Monitor.Enter(Program.lista);
                     while (Program.lista.Count == 0)
                     {
                         Monitor.Wait(Program.lista);
                     }
                     Console.ForegroundColor = c;
                     Console.WriteLine(Program.lista[0]);
                     if (lista[0] < 0.016 )
                     {
                         Console.WriteLine("Ezüst");
                         Program.anyagok.Add("Ezüst");
                     } else if (0.017 < lista[0] && lista[0] < 0.023)
                     {
                         Console.WriteLine("Réz");
                         Program.anyagok.Add("Réz");
                     } else if (0.024 < lista[0] && lista[0] < 0.028)
                     {
                         Console.WriteLine("Arany");
                         Program.anyagok.Add("Arany");
                     } else if (0.029 < lista[0] && lista[0] < 0.50)
                     {
                         Console.WriteLine("Alumínium");
                         Program.anyagok.Add("Alumínium");
                     } else if (0.51 < lista[0] && lista[0] < 0.957)
                     {
                         Console.WriteLine("Konstantán");
                         Program.anyagok.Add("Konstantán");
                     } else if (0.959 < lista[0] && lista[0] < Math.Pow(10,17))
                     {
                         Console.WriteLine("Higany");
                         Program.anyagok.Add("Higany");
                     }else
                     {
                         Console.WriteLine("Üveg");
                         Program.anyagok.Add("Üveg");
                     }

                     Program.lista.RemoveAt(0);
                     this.FejreszKiir();
                     int sleep = Program.rnd.Next(3, 8);
                     Thread.Sleep(sleep);
                     Monitor.Pulse(Program.lista);
                     Monitor.Exit(Program.lista);

                 }

                 Console.WriteLine("Fogyasztó szál leállt.");
             }
             public void FejreszKiir()
             {
                 if (lista.Count == 0)
                     Console.Title = "Üres";
                 else
                 {
                     if (Program.lista.Count == 60)
                        Console.Title = "Teli";
                     else
                         Console.Title = Program.lista.Count.ToString();
                 }
             }
         }
        }
    }


